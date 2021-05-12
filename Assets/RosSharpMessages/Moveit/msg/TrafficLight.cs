/* 
    Custom message created by Morgan Crouch for my FYP.
 */

using Newtonsoft.Json;

using RosSharp.RosBridgeClient.MessageTypes.Std;

namespace RosSharp.RosBridgeClient.MessageTypes.Moveit
{
    public class TrafficLight : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "TrafficLight";

        //  Message contents
        public int block_selected;
        public int block_status;
        public int zone_selected;
        public int zone_status;

        //private const int YELLOW = 2;
        //private const int RED = 1;
        //private const int UNSELECTED = 0;

        public TrafficLight()
        {
            block_selected = 0;
            block_status = 0;
            zone_selected = 0;
            zone_status = 0;
        }

        public TrafficLight(int block_selected, int block_status, int zone_selected, int zone_status)
        {
            this.block_selected = block_selected;
            this.block_status = block_status;
            this.zone_selected = zone_selected;
            this.zone_status = zone_status;
        }
    }
}
