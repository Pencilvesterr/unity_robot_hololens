using UnityEngine;

//test service response, again should always have only one serivce responses added to ROS

public class TestServiceResponse
{

    public static void ServiceCallBack(string service, string response)
    {
        if (response == null)
            Debug.Log("ServiceCallback for service " + service);
        else
            Debug.Log("ServiceCallback for service " + service + " response " + response);
    }
}

