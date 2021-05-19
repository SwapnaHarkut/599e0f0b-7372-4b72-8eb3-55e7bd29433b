namespace RobotApi
{
    public interface IRobot
    {
        string cmdDetect();
        string cmdDrop();
        string cmdMove(string direction);
        string cmdPlace(string location);
        string cmdReport();
        int getMatrixSize();
        string performAction(string cmd);
    }
}