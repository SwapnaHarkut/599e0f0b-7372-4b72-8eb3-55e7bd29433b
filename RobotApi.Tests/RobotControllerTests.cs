using Microsoft.AspNetCore.Mvc.Testing;
using System;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace RobotApi.Tests
{
    public class RobotControllerTests //: IClassFixture<WebApplicationFactory<RobotApi.Startup>>
    {
        //public HttpClient Client { get; }

        //class Commands
        //{
        //    public string[] inputCommand { get; set; }
        //}

        //Commands inputCmd1 = new Commands()
        //{
        //    inputCommand = new string[] { "PLACE 0,0", "MOVE E", "REPORT" }
        //};

        ////public RobotControllerTests(WebApplicationFactory<RobotApi.Startup> fixture)
        ////{
        ////    Client = fixture.CreateClient();
        ////}

        //[Fact]
        //public async Task PostRobotCommands()
        //{
        //    // Act
        //    var json = JsonConvert.SerializeObject(inputCmd1.inputCommand);
        //    var httpContent = new StringContent(json);
        //    //HttpResponseMessage response = await Client.PostAsync("/Robot/ExecuteCommands", new StringContent(json, Encoding.UTF8, "application/json"));
        //    HttpResponseMessage response = await Client.PostAsync("/Robot/ExecuteCommands", httpContent);
        //    var value = await response.Content.ReadAsStringAsync();

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //}

        [Fact]
        public void Robot_CommandIgnored_WhenNotPlacedYet()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("REPORT");
            //assert
            Assert.Equal(Robot.OUT_OF_BOUNDS_MESSAGE, result);
        }

        [Fact]
        public void Robot_CommandReturnEmptyString_AfterBeingPlaced()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 0,0");
            //assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Robot_Report_AfterBeingPlaced()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 0,0");
            result = robot.performAction("REPORT");
            //assert
            Assert.Equal("0,0,EMPTY", result);
        }

        // ************** Test cardinal directions ********************************************
        [Fact]
        public void Robot_Report_0_1_AfterPlaced_0_0_AndSingleMoveN()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 0,0");
            result = robot.performAction("MOVE N");
            result = robot.performAction("REPORT");
            //assert
            Assert.Equal("0,1,EMPTY", result);
        }

        [Fact]
        public void Robot_Report_1_0_AfterPlaced_0_0_AndSingleMoveE()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 0,0");
            result = robot.performAction("MOVE E");
            result = robot.performAction("REPORT");
            //assert
            Assert.Equal("1,0,EMPTY", result);
        }

        [Fact]
        public void Robot_Report_1_0_AfterPlaced_1_1_AndSingleMoveS()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 1,1");
            result = robot.performAction("MOVE S");
            result = robot.performAction("REPORT");
            //assert
            Assert.Equal("1,0,EMPTY", result);
        }

        [Fact]
        public void Robot_Report_0_1_AfterPlaced_1_1_AndSingleMoveW()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 1,1");
            result = robot.performAction("MOVE W");
            result = robot.performAction("REPORT");
            //assert
            Assert.Equal("0,1,EMPTY", result);
        }

        // ************** Test robot environment boundaries on placement ******************************
        [Fact]
        public void Robot_IgnoreCommand_AfterPlace_NegativeXCoordinate()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE -1,0");
            result = robot.performAction("MOVE W");
            result = robot.performAction("REPORT");
            //assert
            Assert.Equal(Robot.OUT_OF_BOUNDS_MESSAGE, result);
        }
        [Fact]
        public void Robot_IgnoreCommand_AfterPlace_NegativeYCoordinate()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 0,-1");
            result = robot.performAction("MOVE W");
            result = robot.performAction("REPORT");
            //assert
            Assert.Equal(Robot.OUT_OF_BOUNDS_MESSAGE, result);
        }
        [Fact]
        public void Robot_IgnoreCommand_AfterPlace_UpperBoundX()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 6,5");
            //assert
            Assert.Equal(Robot.OUT_OF_BOUNDS_MESSAGE, result);
        }
        [Fact]
        public void Robot_IgnoreCommand_AfterPlace_UpperBoundY()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 5,6");
            //assert
            Assert.Equal(Robot.OUT_OF_BOUNDS_MESSAGE, result);
        }

        // ************** Test robot environment boundaries on movement ************************
        [Fact]
        public void Robot_IgnoreCommand_AfterMove_NegativeXCoordinate()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 0,0");
            result = robot.performAction("MOVE W");
            //assert
            Assert.Equal(Robot.OUT_OF_BOUNDS_MESSAGE, result);
        }
        [Fact]
        public void Robot_IgnoreCommand_AfterMove_NegativeYCoordinate()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 0,0");
            result = robot.performAction("MOVE S");
            //assert
            Assert.Equal(Robot.OUT_OF_BOUNDS_MESSAGE, result);
        }
        [Fact]
        public void Robot_IgnoreCommand_AfterMove_UpperBoundXCoordinate()
        {
            //arrange
            Robot robot = new Robot(1);
            //act
            string result = robot.performAction("PLACE 0,0");
            result = robot.performAction("MOVE E");
            result = robot.performAction("MOVE E");
            //assert
            Assert.Equal(Robot.OUT_OF_BOUNDS_MESSAGE, result);
        }
        [Fact]
        public void Robot_IgnoreCommand_AfterMove_UpperBoundYCoordinate()
        {
            //arrange
            Robot robot = new Robot(1);
            //act
            string result = robot.performAction("PLACE 0,0");
            result = robot.performAction("MOVE N");
            result = robot.performAction("MOVE N");
            //assert
            Assert.Equal(Robot.OUT_OF_BOUNDS_MESSAGE, result);
        }

        // ************** Sample tests provided ************************
        [Fact]
        public void ProvidedTest_A()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 0,0");
            result = robot.performAction("MOVE N");
            result = robot.performAction("REPORT");
            //assert
            Assert.Equal("1,0,EMPTY", result);
        }
        [Fact]
        public void ProvidedTest_B()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 0,0");
            result = robot.performAction("MOVE E");
            result = robot.performAction("REPORT");
            //assert
            Assert.Equal("0,1,FULL", result);
        }
        [Fact]
        public void ProvidedTest_C()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 1,2");
            result = robot.performAction("MOVE N");
            result = robot.performAction("MOVE E");
            result = robot.performAction("REPORT");
            //assert
            Assert.Equal("2,3,EMPTY", result);
        }

        // ************** Test garbage input ************************
        [Fact]
        public void Robot_ReturnsErrorMessage_AfterPlacement_WhenGarbageCommandSent()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE 1,2");
            result = robot.performAction("BANANAS");
            //assert
            Assert.Equal(Robot.COMMAND_NOT_RECOGNISED_MESSAGE, result);
        }

        // ************** Test garbage input ************************
        [Fact]
        public void Robot_ReturnsValidCommandsMessage_WhenGarbageCommandSent()
        {
            //arrange
            Robot robot = new Robot(5);
            //act
            string result = robot.performAction("PLACE %,2");
            //assert
            Assert.Equal(Robot.VALID_COMMANDS_MESSAGE, result);
        }
    }
}
