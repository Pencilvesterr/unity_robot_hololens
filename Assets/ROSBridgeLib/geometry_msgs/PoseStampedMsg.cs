using SimpleJSON;
using ROSBridgeLib.std_msgs;

/*  PoseStamped Msg   
 *  added to ROSBridgeLib and modified by Cole Shing, 2017
 *  std_msgs/Header header
 *  Geometry_msgs/Pose Pose
 *      geometry_msgs/Point position
 *      geometry_msgs/Quaternion orientation
 */

namespace ROSBridgeLib
{
    namespace geometry_msgs
    {
        public class PoseStampedMsg : ROSBridgeMsg {
			public HeaderMsg _header;
			public PoseMsg _pose;
			
			public PoseStampedMsg(JSONNode msg) {
				_header = new HeaderMsg(msg["header"]);
				_pose = new PoseMsg(msg["pose"]);
			}
 			
            public PoseStampedMsg(HeaderMsg header, PoseMsg pose)
            {
                _header = header;
                _pose = pose;
            }
			public static string GetMessageType() {
				return "geometry_msgs/PoseStamped";
			}
			
			public HeaderMsg GetHeader() {
				return _header;
			}

			public PoseMsg GetPose() {
				return _pose;
			}

            public PointMsg GetPostion(){
                return _pose._position;
            }
			
            public QuaternionMsg GetOrientation(){
                return _pose._orientation;
            }

			public override string ToString() {
				return "PoseStamped [header=" + _header.ToString() + ",  pose=" + _pose.ToString() + "]";
			}
			
			public override string ToYAMLString() {
				return "{\"header\" : " + _header.ToYAMLString() + ", \"pose\" : " + _pose.ToYAMLString() + "}";
			}
		}
	}
}