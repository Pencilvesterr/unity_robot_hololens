using ROSBridgeLib;
using ROSBridgeLib.wam_common;

/* WAM cartesian position publisher using RTCartPosMsg to publish to 
 * /wam/cart_pos_cmd
 * written by Cole Shing, 2017
 */
public class WAMRTCartPos : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/wam/cart_pos_cmd";
    }

    public new static  string GetMessageType()
    {
        return "wam_common/RTCartPos";
    }

    public static string ToYAMLString(RTCartPosMsg msg)
    {
        return msg.ToYAMLString();
    }
}
