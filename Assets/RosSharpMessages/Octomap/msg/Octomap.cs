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

namespace RosSharp.RosBridgeClient.MessageTypes.Octomap
{
    public class Octomap : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "octomap_msgs/Octomap";

        //  A 3D map in binary format, as Octree
        public Header header;
        //  Flag to denote a binary (only free/occupied) or full occupancy octree (.bt/.ot file)
        public bool binary;
        //  Class id of the contained octree 
        public string id;
        //  Resolution (in m) of the smallest octree nodes
        public double resolution;
        //  binary serialization of octree, use conversions.h to read and write octrees
        public sbyte[] data;

        public Octomap()
        {
            this.header = new Header();
            this.binary = false;
            this.id = "";
            this.resolution = 0.0;
            this.data = new sbyte[0];
        }

        public Octomap(Header header, bool binary, string id, double resolution, sbyte[] data)
        {
            this.header = header;
            this.binary = binary;
            this.id = id;
            this.resolution = resolution;
            this.data = data;
        }
    }
}
