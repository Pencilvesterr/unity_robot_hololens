using System.Collections.Generic;
using SimpleJSON;

/* Polygon message under geometry_msgs
 * by Cole Shing, 2017
 */

namespace ROSBridgeLib
{
    namespace geometry_msgs
    {     
        public class PolygonMsg : ROSBridgeMsg
        {
            ConvertingArraytoString convert = new ConvertingArraytoString(); //convert lists to different arrays
            private List<Point32Msg> _points = new List<Point32Msg>();

            public PolygonMsg(JSONNode msg)
            {
                for (int i = 0; i < msg["points"].Count; i++)
                {
                    _points.Add(new Point32Msg(msg["points"][i]));
                }
            }

            public PolygonMsg(List<Point32Msg> points) //points should all be initialized
            {
                _points = points;
            }

            public static string getMessageType()
            {
                return "geometry_msgs/Polygon";
            }

            public Point32Msg GetPoint(int idx = 0) //get point indexed at idx
            {
                if (idx < _points.Count) 
                    return _points[idx];
                else    
                    return null;
            }

            public override string ToString()
            {
                //converting accelerations array to yaml string
                string pointarray = convert.Listtoarray(_points);

                return "PointTrajectory [points=" + pointarray + "]";
            }

            public override string ToYAMLString()
            {
                //converting accelerations array to yaml string
                string pointarray = convert.Listtoarray(_points);

                return "{\"points\" : " + pointarray + "}";
            }


        }
    }
}
