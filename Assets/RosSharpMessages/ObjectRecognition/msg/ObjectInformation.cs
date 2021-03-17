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
using RosSharp.RosBridgeClient.MessageTypes.Sensor;

namespace RosSharp.RosBridgeClient.MessageTypes.ObjectRecognition
{
    public class ObjectInformation : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "object_recognition_msgs/ObjectInformation";

        // ############################################# VISUALIZATION INFO ######################################################
        // ################## THIS INFO SHOULD BE OBTAINED INDEPENDENTLY FROM THE CORE, LIKE IN AN RVIZ PLUGIN ###################
        //  The human readable name of the object
        public string name;
        //  The full mesh of the object: this can be useful for display purposes, augmented reality ... but it can be big
        //  Make sure the type is MESH
        public Mesh ground_truth_mesh;
        //  Sometimes, you only have a cloud in the DB
        //  Make sure the type is POINTS
        public PointCloud2 ground_truth_point_cloud;

        public ObjectInformation()
        {
            this.name = "";
            this.ground_truth_mesh = new Mesh();
            this.ground_truth_point_cloud = new PointCloud2();
        }

        public ObjectInformation(string name, Mesh ground_truth_mesh, PointCloud2 ground_truth_point_cloud)
        {
            this.name = name;
            this.ground_truth_mesh = ground_truth_mesh;
            this.ground_truth_point_cloud = ground_truth_point_cloud;
        }
    }
}
