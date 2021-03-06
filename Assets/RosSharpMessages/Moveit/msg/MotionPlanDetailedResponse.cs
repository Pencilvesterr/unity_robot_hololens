/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

namespace RosSharp.RosBridgeClient.MessageTypes.Moveit
{
    public class MotionPlanDetailedResponse : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "moveit_msgs/MotionPlanDetailedResponse";

        //  The representation of a solution to a planning problem, including intermediate data
        //  The starting state considered for the robot solution path
        public RobotState trajectory_start;
        //  The group used for planning (usually the same as in the request)
        public string group_name;
        //  Multiple solution paths are reported, each reflecting intermediate steps in the trajectory processing
        //  The list of reported trajectories
        public RobotTrajectory[] trajectory;
        //  Description of the reported trajectories (name of processing step)
        public string[] description;
        //  The amount of time spent computing a particular step in motion plan computation 
        public double[] processing_time;
        //  Status at the end of this plan
        public MoveItErrorCodes error_code;

        public MotionPlanDetailedResponse()
        {
            this.trajectory_start = new RobotState();
            this.group_name = "";
            this.trajectory = new RobotTrajectory[0];
            this.description = new string[0];
            this.processing_time = new double[0];
            this.error_code = new MoveItErrorCodes();
        }

        public MotionPlanDetailedResponse(RobotState trajectory_start, string group_name, RobotTrajectory[] trajectory, string[] description, double[] processing_time, MoveItErrorCodes error_code)
        {
            this.trajectory_start = trajectory_start;
            this.group_name = group_name;
            this.trajectory = trajectory;
            this.description = description;
            this.processing_time = processing_time;
            this.error_code = error_code;
        }
    }
}
