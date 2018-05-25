using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;

/* Point Trajectory publisher for gemeotry msgs polygon
 * written by Cole Shing, 2017
 */
public class PointTrajectoryPub : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/point_trajectory";
    }

    public new static string GetMessageType()
    {
        return "geometry_msgs/Polygon";
    }

    public static string ToYAMLString(PolygonMsg msg)
    {
        return msg.ToYAMLString();
    }
}
