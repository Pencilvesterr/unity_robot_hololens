/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;

namespace RosSharp.RosBridgeClient.MessageTypes.Moveit
{
    public class WorkspaceParameters : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "moveit_msgs/WorkspaceParameters";

        //  This message contains a set of parameters useful in
        //  setting up the volume (a box) in which the robot is allowed to move.
        //  This is useful only when planning for mobile parts of 
        //  the robot as well.
        //  Define the frame of reference for the box corners
        public Header header;
        //  The minumum corner of the box, with respect to the robot starting pose
        public Vector3 min_corner;
        //  The maximum corner of the box, with respect to the robot starting pose
        public Vector3 max_corner;

        public WorkspaceParameters()
        {
            this.header = new Header();
            this.min_corner = new Vector3();
            this.max_corner = new Vector3();
        }

        public WorkspaceParameters(Header header, Vector3 min_corner, Vector3 max_corner)
        {
            this.header = header;
            this.min_corner = min_corner;
            this.max_corner = max_corner;
        }
    }
}
