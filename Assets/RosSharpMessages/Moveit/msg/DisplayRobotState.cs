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
    public class DisplayRobotState : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "moveit_msgs/DisplayRobotState";

        //  The robot state to display
        public RobotState state;
        //  Optionally, various links can be highlighted
        public ObjectColor[] highlight_links;
        //  If true, suppress the display in visualizations (like rviz)
        public bool hide;

        public DisplayRobotState()
        {
            this.state = new RobotState();
            this.highlight_links = new ObjectColor[0];
            this.hide = false;
        }

        public DisplayRobotState(RobotState state, ObjectColor[] highlight_links, bool hide)
        {
            this.state = state;
            this.highlight_links = highlight_links;
            this.hide = hide;
        }
    }
}
