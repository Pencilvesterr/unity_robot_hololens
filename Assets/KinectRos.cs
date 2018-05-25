using UnityEngine;
using SimpleJSON;
using ROSBridgeLib;

/* Kinect ros subsrciber, it is currently too slow and not usable, need more work, left as archived
 * 
 */

public class KinectRos : ROSBridgeSubscriber
{
    static GameObject kinectimage;
    static MeshRenderer image_rect;
    static Texture2D pic;

    public new static string GetMessageTopic()
    {
        return "/kinect2/sd/image_color_rect";
        //return "/image_converter/output_video";
    }

    public new static string GetMessageType()
    {
        return "sensor_msgs/Image";
    }

    public new static int GetQueueLength()
    {
        return 1;
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ROSBridgeLib.sensor_msgs.ImageMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        kinectimage = GameObject.Find("Picture");
        image_rect = kinectimage.GetComponent<MeshRenderer>();
        pic = new Texture2D(512, 424, TextureFormat.RGB24, false);
        //ROSBridgeLib.sensor_msgs.CompressedImageMsg kinectimg = (ROSBridgeLib.sensor_msgs.CompressedImageMsg)msg;
        ROSBridgeLib.sensor_msgs.ImageMsg kinectimg = (ROSBridgeLib.sensor_msgs.ImageMsg)msg;
        byte[] image = kinectimg.GetImage();
        pic.LoadRawTextureData(image);
        pic.Apply();
        image_rect.material.mainTexture = pic;       
    }
}
