/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

using RosSharp.RosBridgeClient.MessageTypes.Octomap;

namespace RosSharp.RosBridgeClient.MessageTypes.Moveit
{
    public class PlanningSceneWorld : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "moveit_msgs/PlanningSceneWorld";

        //  collision objects
        public CollisionObject[] collision_objects;
        //  The octomap that represents additional collision data
        public OctomapWithPose octomap;

        public PlanningSceneWorld()
        {
            this.collision_objects = new CollisionObject[0];
            this.octomap = new OctomapWithPose();
        }

        public PlanningSceneWorld(CollisionObject[] collision_objects, OctomapWithPose octomap)
        {
            this.collision_objects = collision_objects;
            this.octomap = octomap;
        }
    }
}
