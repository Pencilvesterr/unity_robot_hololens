using UnityEngine;

/* Service Response for WAM, currently just used to denote that a callback has been responded to. Should always only have one service reponse active.
 * 
 */

public class WAMServiceResponse
{

    public static void ServiceCallBack(string service, string response)
    {
        if (response == null)
            Debug.Log("ServiceCallback for service " + service);
        else
            Debug.Log("ServiceCallback for service " + service + " response " + response);
    }
}
