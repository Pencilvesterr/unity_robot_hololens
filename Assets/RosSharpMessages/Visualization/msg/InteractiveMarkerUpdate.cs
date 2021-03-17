/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

namespace RosSharp.RosBridgeClient.MessageTypes.Visualization
{
    public class InteractiveMarkerUpdate : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "visualization_msgs/InteractiveMarkerUpdate";

        //  Identifying string. Must be unique in the topic namespace
        //  that this server works on.
        public string server_id;
        //  Sequence number.
        //  The client will use this to detect if it has missed an update.
        public ulong seq_num;
        //  Type holds the purpose of this message.  It must be one of UPDATE or KEEP_ALIVE.
        //  UPDATE: Incremental update to previous state. 
        //          The sequence number must be 1 higher than for
        //          the previous update.
        //  KEEP_ALIVE: Indicates the that the server is still living.
        //              The sequence number does not increase.
        //              No payload data should be filled out (markers, poses, or erases).
        public const byte KEEP_ALIVE = 0;
        public const byte UPDATE = 1;
        public byte type;
        // Note: No guarantees on the order of processing.
        //       Contents must be kept consistent by sender.
        // Markers to be added or updated
        public InteractiveMarker[] markers;
        // Poses of markers that should be moved
        public InteractiveMarkerPose[] poses;
        // Names of markers to be erased
        public string[] erases;

        public InteractiveMarkerUpdate()
        {
            this.server_id = "";
            this.seq_num = 0;
            this.type = 0;
            this.markers = new InteractiveMarker[0];
            this.poses = new InteractiveMarkerPose[0];
            this.erases = new string[0];
        }

        public InteractiveMarkerUpdate(string server_id, ulong seq_num, byte type, InteractiveMarker[] markers, InteractiveMarkerPose[] poses, string[] erases)
        {
            this.server_id = server_id;
            this.seq_num = seq_num;
            this.type = type;
            this.markers = markers;
            this.poses = poses;
            this.erases = erases;
        }
    }
}
