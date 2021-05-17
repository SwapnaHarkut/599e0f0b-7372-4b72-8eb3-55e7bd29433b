using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Robot
    {
        public const string OUT_OF_BOUNDS_MESSAGE = "Command ignored - out of bounds";
        public const string NOT_PLACED_YET_MESSAGE = "Command ignored - robot not placed yet";
        public const string COMMAND_NOT_RECOGNISED_MESSAGE = "Command ignored - robot did not understand this command";
        public const string VALID_COMMANDS_MESSAGE = "Error during command handling.\nValid commands are:\nPLACE X,Y\nDETECT\nDROP\nMOVE N, S, E or W\nREPORT";

        private Tuple<int, int> position;
        private int[,] plate;
        private int matrixSize = 0;
        private bool isPlaced = false;

        public Robot(int size)
        {
            matrixSize = size;
            plate = new int[size, size];
            position = null;
        }

        public int getMatrixSize()
        {
            return plate[0, 0];
        }

        public string cmdPlace(String location)
        {
            String[] args = location.Split(',');
            int x = Int32.Parse(args[0]);
            int y = Int32.Parse(args[1]);
            if (x >= matrixSize || y >= matrixSize) return OUT_OF_BOUNDS_MESSAGE;
            if (x < 0 || y < 0) return OUT_OF_BOUNDS_MESSAGE;
            position = new Tuple<int, int>(x, y);
            isPlaced = true;
            return "";
        }

        public String cmdDetect()
        {
            if (position == null) return "ERR";
            if (plate[position.Item1, position.Item2] == 1) return "FULL";
            else return "EMPTY";
        }

        public string cmdDrop()
        {
            if (position == null) return NOT_PLACED_YET_MESSAGE;
            plate[position.Item1, position.Item2] = 1;
            return "";
        }

        public string cmdMove(String direction)
        {
            if (position == null) return OUT_OF_BOUNDS_MESSAGE;
            switch (direction)
            {
                case "N":
                    if (position.Item2 + 1 < matrixSize)
                    {
                        position = new Tuple<int, int>(position.Item1, position.Item2 + 1);
                    }
                    else
                        return OUT_OF_BOUNDS_MESSAGE;
                    break;
                case "S":
                    if (position.Item2 > 0)
                    {
                        position = new Tuple<int, int>(position.Item1, position.Item2 - 1);
                    }
                    else
                        return OUT_OF_BOUNDS_MESSAGE;
                    break;
                case "E":
                    if (position.Item1 + 1 < matrixSize)
                    {
                        position = new Tuple<int, int>(position.Item1 + 1, position.Item2);
                    }
                    else
                        return OUT_OF_BOUNDS_MESSAGE;
                    break;
                case "W":
                    if (position.Item1 > 0)
                    {
                        position = new Tuple<int, int>(position.Item1 - 1, position.Item2);
                    }
                    else
                        return OUT_OF_BOUNDS_MESSAGE;
                    break;
                default:
                    return VALID_COMMANDS_MESSAGE;
            }
            return "";
        }

        public string cmdReport()
        {
            if (position == null) return OUT_OF_BOUNDS_MESSAGE;
            string output = position.Item1 + "," + position.Item2 + "," + cmdDetect();
            return output;
        }

        public string performAction(String cmd)
        {
            string command = cmd.ToUpper();
            string result = string.Empty;
            String[] args = cmd.Split(' ');
            try
            {
                switch (args[0])
                {
                    case "PLACE":
                        if (args.Length > 1)
                        {
                            result = cmdPlace(args[1]);
                        }
                        break;
                    case "DETECT":
                        if (isPlaced)
                            result = cmdDetect();
                        break;
                    case "DROP":
                        result = cmdDrop();
                        break;
                    case "MOVE":
                        if (isPlaced)
                            if (args.Length > 1)
                                result = cmdMove(args[1]);
                        break;
                    case "REPORT":
                        result = cmdReport();
                        break;
                    default:
                        result = COMMAND_NOT_RECOGNISED_MESSAGE;
                        break;
                }
            }
            catch (Exception ex)
            {
                result = VALID_COMMANDS_MESSAGE;
            }
            return result;
        }
    }
}
