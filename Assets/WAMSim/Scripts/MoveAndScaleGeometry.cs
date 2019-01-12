using HoloToolkit.Unity.InputModule;
using UnityEngine;

/// <summary>
/// GestureAction performs custom actions based on
/// which gesture is being performed.
/// </summary>
public class GestureAction : MonoBehaviour, INavigationHandler, IManipulationHandler, ISpeechHandler
{
    [Tooltip("Rotation max speed controls amount of rotation.")]
    [SerializeField]
    private float RotationSensitivity = 10.0f;
    private float scalingBarrierBigger = 5.0f;
    private float scalingBarrierSmaller = 5.0f;

    private bool isNavigationEnabled = true;
    /*
    right now it is false; maybe we can create a variable which is passed
    into these variables when say through voice detection we enable the 
    scaling gesture. 
    */
    private bool scalingBarrierBigger = false; 
    private bool scalingBarrierSmaller = false;
    public bool IsNavigationEnabled
    {
        get { return isNavigationEnabled; }
        set { isNavigationEnabled = value; }
    }

    private Vector3 manipulationOriginalPosition = Vector3.zero;

    void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
    {
        if (isNavigationEnabled)
        {
            /* TODO: DEVELOPER CODING EXERCISE 2.c */

            // 2.c: Calculate a float rotationFactor based on eventData's NormalizedOffset.x multiplied by RotationSensitivity.
            // This will help control the amount of rotation.
            float rotationFactor = eventData.NormalizedOffset.x * RotationSensitivity;

            // 2.c: transform.Rotate around the Y axis using rotationFactor.
            transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));

            /*
            My thinking here is that a certain gesture movement can make 
            the barrier bigger, say the gesture where we take our thumb 
            and index finger and move them away from each other to expand
            which in turn will make the barrier bigger. The only question is 
            how can the hololens detect that specific behaviour? I will have to look
            into it. These two lines are the only two lines of code I added btw; 
            everything else is from the hololens documentation website. 
            (https://docs.microsoft.com/en-us/windows/mixed-reality/holograms-211)
            */
            if (scalingBarrierBigger)
            {
                transform.localScale(barrier.x * scalingBiggerFactor,
                barrier.y * scalingBiggerFactor, barrier.z * scalingBiggerFactor);
            }

            if (scalingBarrierSmaller)
            {
                transform.localScale(barrier.x * scalingsSmallerFactor,
                barrier.y * scalingsSmallerFactor, barrier.z * scalingsSmallerFactor);
            }
        }
    }

    void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
    {
            }
        
        InputManager.Instance.PopModalInputHandler();
    }

    void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        if (!isNavigationEnabled)
        {
            InputManager.Instance.PushModalInputHandler(gameObject);

            manipulationOriginalPosition = transform.position;
        }
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (!isNavigationEnabled)
        {
            /* TODO: DEVELOPER CODING EXERCISE 4.a */

            // 4.a: Make this transform's position be the manipulationOriginalPosition + eventData.CumulativeDelta
            manipulationOriginalPosition = manipulationOriginalPosition + eventData.CumulativeDelta
        }
    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void ISpeechHandler.OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        if (eventData.RecognizedText.Equals("Move Astronaut"))
        {
            isNavigationEnabled = false;
        }
        else if (eventData.RecognizedText.Equals("Rotate Astronaut"))
        {
            isNavigationEnabled = true;
        }
        else
        {
            return;
        }

        eventData.Use();
    }
}