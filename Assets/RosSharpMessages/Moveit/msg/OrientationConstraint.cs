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
    public class OrientationConstraint : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "moveit_msgs/OrientationConstraint";

        //  This message contains the definition of an orientation constraint.
        public Header header;
        //  The desired orientation of the robot link specified as a quaternion
        public Quaternion orientation;
        //  The robot link this constraint refers to
        public string link_name;
        //  optional axis-angle error tolerances specified
        public double absolute_x_axis_tolerance;
        public double absolute_y_axis_tolerance;
        public double absolute_z_axis_tolerance;
        //  A weighting factor for this constraint (denotes relative importance to other constraints. Closer to zero means less important)
        public double weight;

        public OrientationConstraint()
        {
            this.header = new Header();
            this.orientation = new Quaternion();
            this.link_name = "";
            this.absolute_x_axis_tolerance = 0.0;
            this.absolute_y_axis_tolerance = 0.0;
            this.absolute_z_axis_tolerance = 0.0;
            this.weight = 0.0;
        }

        public OrientationConstraint(Header header, Quaternion orientation, string link_name, double absolute_x_axis_tolerance, double absolute_y_axis_tolerance, double absolute_z_axis_tolerance, double weight)
        {
            this.header = header;
            this.orientation = orientation;
            this.link_name = link_name;
            this.absolute_x_axis_tolerance = absolute_x_axis_tolerance;
            this.absolute_y_axis_tolerance = absolute_y_axis_tolerance;
            this.absolute_z_axis_tolerance = absolute_z_axis_tolerance;
            this.weight = weight;
        }
    }
}