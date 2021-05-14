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

namespace RosSharp.RosBridgeClient.MessageTypes.Moveit
{
    public class ObjectColor : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "moveit_msgs/ObjectColor";

        //  The object id for which we specify color
        public string id;
        //  The value of the color
        public ColorRGBA color;

        public ObjectColor()
        {
            this.id = "";
            this.color = new ColorRGBA();
        }

        public ObjectColor(string id, ColorRGBA color)
        {
            this.id = id;
            this.color = color;
        }
    }
}
