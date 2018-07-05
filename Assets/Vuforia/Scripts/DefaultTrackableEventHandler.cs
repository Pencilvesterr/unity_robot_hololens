/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System.Linq;
using UnityEngine;
using Vuforia;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    public bool recalibrate;

    #region PRIVATE_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    int count = 0;
    TrackableSettings forData = new TrackableSettings();
    
    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        //recalibrate = false;
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        Debug.Log("In start" + recalibrate);
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

  

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    /// 
    public void reActivate()
    {
        if (recalibrate)
        {
            //Request an ObjectTracker instance, this is the instance that tracks the armarkers
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            //grab all the datasets inside the database
            List<DataSet> dataSet = tracker.GetDataSets().ToList();
            //for loop activates all the datasets, activation can only occur when the objectTracker is off, so once activation is complete start up the tracker again
            foreach (DataSet activate in dataSet)
            {
                tracker.Stop();
                tracker.ActivateDataSet(activate);
                tracker.Start();
                Debug.Log("datasets reactivated " + recalibrate);
            }



        }


    }

    
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)

    {

        Debug.Log("Inside OnTrackableSateChanged" + recalibrate);
        
        
               
         
        //we enter this if statement if we have found the marker
        if ((newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED))
        {
            //Request an ObjectTracker instance, this is the instance that tracks the armarkers
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            Debug.Log("Trackable detected,tracked, extended " + recalibrate);
            //Since marker is found, display the WAM
            OnTrackingFound();
            //Once the WAM has been placed, we want to avoid automatic jumping, hence we deactivate the datasets.
            //First, grab all active datasets 
            List<DataSet> dataSetActive = tracker.GetActiveDataSets().ToList();
            //this for loop deactivates all active datasets; can be done only when the objectTracker is stopped, so make sure to start it up again.
            foreach (DataSet activate in dataSetActive)
            {
                tracker.Stop();
                tracker.DeactivateDataSet(activate);
                tracker.Start();
                Debug.Log("dataset deactivated " + activate);
            }
          

        }
      
        //this else/if part is something we don't need anymore, keeping it just in case though if we need it for reference:
        //else if part would originally remove the AR Object on the Ar Marker (for us, the WAM Robot) when the marker moves places. 
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable lost" + recalibrate);
            //OnTrackingLost();
        }

        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
            Debug.Log("Trackable unknown " +recalibrate);
            recalibrate = false;
        }
    }

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;

        
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    #endregion // PRIVATE_METHODS
}
