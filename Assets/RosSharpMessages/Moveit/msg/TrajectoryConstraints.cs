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
    public class TrajectoryConstraints : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "moveit_msgs/TrajectoryConstraints";

        //  The array of constraints to consider along the trajectory
        public Constraints[] constraints;

        public TrajectoryConstraints()
        {
            this.constraints = new Constraints[0];
        }

        public TrajectoryConstraints(Constraints[] constraints)
        {
            this.constraints = constraints;
        }
    }
}
