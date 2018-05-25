using UnityEngine;
using ROSBridgeLib;

//test services class, where to have all the related services call made into a class member functions

public class TestServices : MonoBehaviour
{
    private ROSBridgeWebSocketConnection rosbridge = null;
    private string service, args;
    
    public void ServInit(ROSBridgeWebSocketConnection ros)
    {
        rosbridge = ros;
    }

    public struct serv
    {
        public string service;
        public string args;
    }

    public void AddtwoInt(int[] num) //sample adding twon ints together
    {
        service = "/add_two_ints";
        args = "{\"a\": " + num[0] + ", \"b\": " + num[1] + "}";
        rosbridge.CallService(service, args);
    }
}
