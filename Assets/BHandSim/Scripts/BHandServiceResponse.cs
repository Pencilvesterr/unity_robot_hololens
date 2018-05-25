using UnityEngine;
/*
 * Service Response for the Barrett Hand. Required to have atleast one subscribed in main code with addServiceResponse
 * Currently does nothing but just shows that the reponse has been recieved 
 */

public class BHandServiceResponse {

	public static void ServiceCallBack(string service, string response)
    {
        //add any function here to start once a service callback is polled back

        if (response == null)
            Debug.Log("BHand ServiceCallback for service " + service);
        else
            Debug.Log("BHand ServiceCallback for service " + service + " response " + response);
    }
}
