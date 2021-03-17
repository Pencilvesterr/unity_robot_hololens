/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

using RosSharp.RosBridgeClient.MessageTypes.Shape;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;

namespace RosSharp.RosBridgeClient.MessageTypes.Moveit
{
    public class BoundingVolume : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "moveit_msgs/BoundingVolume";

        //  Define a volume in 3D
        //  A set of solid geometric primitives that make up the volume to define (as a union)
        public SolidPrimitive[] primitives;
        //  The poses at which the primitives are located
        public Pose[] primitive_poses;
        //  In addition to primitives, meshes can be specified to add to the bounding volume (again, as union)
        public Mesh[] meshes;
        //  The poses at which the meshes are located
        public Pose[] mesh_poses;

        public BoundingVolume()
        {
            this.primitives = new SolidPrimitive[0];
            this.primitive_poses = new Pose[0];
            this.meshes = new Mesh[0];
            this.mesh_poses = new Pose[0];
        }

        public BoundingVolume(SolidPrimitive[] primitives, Pose[] primitive_poses, Mesh[] meshes, Pose[] mesh_poses)
        {
            this.primitives = primitives;
            this.primitive_poses = primitive_poses;
            this.meshes = meshes;
            this.mesh_poses = mesh_poses;
        }
    }
}
