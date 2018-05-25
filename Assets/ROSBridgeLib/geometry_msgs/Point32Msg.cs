using SimpleJSON;

/* Created a point32msg version to use in pointcloud from the normal RosBridgeLib
 * modified by Cole Shing, 2017 as point32msg
 */

namespace ROSBridgeLib
{
    namespace geometry_msgs
    {
        public class Point32Msg : ROSBridgeMsg
        {
            public float _x, _y, _z;

            public Point32Msg(JSONNode msg)
            {
                _x = float.Parse(msg["x"]);
                _y = float.Parse(msg["y"]);
                _z = float.Parse(msg["z"]);
            }

            public Point32Msg(float x, float y, float z)
            {
                _x = x;
                _y = y;
                _z = z;
            }

            public static string getMessageType()
            {
                return "geometry_msgs/Point32";
            }

            public float GetX()
            {
                return _x;
            }

            public float GetY()
            {
                return _y;
            }

            public float GetZ()
            {
                return _z;
            }

            public override string ToString()
            {
                return "geometry_msgs/Point32 [x=" + _x + ",  y=" + _y + ", z=" + _z + "]";
            }

            public override string ToYAMLString()
            {
                return "{\"x\": " + _x + ", \"y\": " + _y + ", \"z\": " + _z + "}";
            }
        }
    }
}
