/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

using RosSharp.RosBridgeClient.MessageTypes.Geometry;

namespace RosSharp.RosBridgeClient.MessageTypes.Moveit
{
    public class OrientedBoundingBox : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "moveit_msgs/OrientedBoundingBox";

        //  the pose of the box
        public Pose pose;
        //  the extents of the box, assuming the center is at the origin
        public Point32 extents;

        public OrientedBoundingBox()
        {
            this.pose = new Pose();
            this.extents = new Point32();
        }

        public OrientedBoundingBox(Pose pose, Point32 extents)
        {
            this.pose = pose;
            this.extents = extents;
        }
    }
}
