using UnityEngine;
using ROSBridgeLib;

/// <summary>
// services class for the Barrett Hand, written by Cole Shing 2017
// to use this class, in the main ros bridge code, add a BHandServices BhandServ = new BHandServices(); as a global variable
// then in start(), call the init function BHandserv.ServInit(ROSBridgeWebSocketConnection ROS)
/// </summary>

public class BHandServices : MonoBehaviour {

    private ROSBridgeWebSocketConnection rosbridge = null; //local copy of the rosbridge
    private string service, args;

    public void ServInit(ROSBridgeWebSocketConnection ros) //initialization of the class
    {
        rosbridge = ros;
    }

    public void CloseGrasp() //close the grasp of the hand
    {
        service = "/bhand/close_grasp";
        args = "";
        rosbridge.CallService(service, args);
    }

    public void CloseSpread() //close the spread of the hand
    {
        service = "/bhand/close_spread";
        args = "";
        rosbridge.CallService(service, args);
    }

    public void FingerPos(float[] Position) //set the positions of the fingers
    {
        args = "{\"radians\" : [" + Position[0] + ", " + Position[1] +
            ", " + Position[2] + "] }";
        service = "/bhand/finger_pos";
        rosbridge.CallService(service, args);
    }

    public void FingerVel(float[] velocities) //set the velocities of the fingers
    {
        args = "{\"velocity\" : [" + velocities[0] + ", " + velocities[1] +
            ", " + velocities[2] + "] }";
        service = "/bhand/finger_vel";
        rosbridge.CallService(service, args);
    }

    public void GraspPos(float radians) //set the grasp position
    {
        args = "{\"radians\" : " + radians + "}";
        service = "/bhand/grasp_pos";
        rosbridge.CallService(service, args);
    }

    public void GraspVel(float velocity) //set the grasp velocities
    {
        args = "{\"velocity\" : " + velocity + "}";
        service = "/bhand/grasp_vel";
        rosbridge.CallService(service, args);
    }

    public void OpenGrasp() //open the grasp of the hand
    {
        service = "/bhand/open_grasp";
        args = "";
        rosbridge.CallService(service, args);
    }

    public void OpenSpread() //open the spread of the hand
    {
        service = "/bhand/open_spread";
        args = "";
        rosbridge.CallService(service, args);
    }

    public void SpreadPos(float radians) //set the spread position
    {
        args = "{\"radians\" : " + radians + "}";
        service = "/bhand/spread_pos";
        rosbridge.CallService(service, args);
    }

    public void SpreadVel(float velocity) //set the spread velocity
    {
        args = "{\"radians\" : " + velocity + "}";
        service = "/bhand/grasp_pos";
        rosbridge.CallService(service, args);
    }

}
