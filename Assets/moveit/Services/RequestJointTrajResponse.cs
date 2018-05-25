using UnityEngine;
using ROSBridgeLib.moveit_msgs;
using SimpleJSON;

/* Written by Cole Shing, 2017
 * Service Response for the custom class. Required to have atleast one subscribed in main code with addServiceResponse
 */

public class RequestJointTrajResponose
{
    public static RequestJointTrajSrv Jointsrv= new RequestJointTrajSrv(); //define the joint service class 
    static RobotTrajectoryMsg robotmsg; //define the robot trajectory message
    public static void ServiceCallBack(string service, string response)
    {

        if (response == null)
            Debug.Log("ServiceCallback for service " + service);
        else
            Debug.Log("ServiceCallback for service " + service + " response " + response);

        if (service == "/request_joint_path") //if the service was request_joint_path send the send joint trajectory service call
        {
            Debug.Log("gets response service request_joint_path");
            robotmsg = new RobotTrajectoryMsg(JSONNode.Parse(response));

            new WaitForSeconds(20); //should wait 20 seconds

            Debug.Log("20 secs pass");
            Jointsrv.SendJointTraj(robotmsg.GetJointTrajectories()); //send the request
        }
    }
}
