// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos.EyeTracking
{
    /// <summary>
    /// This script allows for dynamically changing the size of a GameObject when it is looked at. 
    /// This is for example useful for better legibility of small text.
    /// </summary>
    public class ChangeSizeCustom : MonoBehaviour
    {
        #region Serialized variables
        [Tooltip("Final size when the target is looked at.")]
        [SerializeField]
        private Vector3 Size_OnLookAt = new Vector3(1.2f, 1.2f, 1.2f);

        [Tooltip("Speed factor that will be multiplied with the delta time to adapt speed of change when engaged.")]
        [SerializeField]
        private float SizeChangeSpeedEngaged = 1.0f;

        [Tooltip("Speed factor that will be multiplied with the delta time to adapt speed of change when disengaged.")]
        [SerializeField]
        private float SizeChangeSpeedDisengaged = 1.0f;

        [Tooltip("If the delta between the current scale and the final scale is lower than this threshold, stop resizing. This helps keeping performance high.")]
        [SerializeField]
        private float SizeDeltaThresh = 0.1f;
        #endregion

        private GameObject target = null;
        private GameObject objectWithCollider = null;
        private Vector3 originalLocalScale;
        private bool inTransition = false;
        private bool isLookedAt = false;

        private void Start()
        {
            InitialSetup();
        }

        public void Engage()
        {
            isLookedAt = true;
            inTransition = true;
        }

        public void Disengage()
        {
            isLookedAt = false;
            inTransition = true;
        }

        /// <summary>
        /// Getting things set up. This includes making sure that relevant objects are defined and parameters are correctly set to start.
        /// </summary>
        private void InitialSetup()
        {
            if (target == null)
            {
                target = gameObject;
            }

            if (objectWithCollider == null)
            {
                Collider coll;
                coll = GetComponent<Collider>();
                if (coll == null)
                {
                    coll = GetComponentInChildren<Collider>();
                }

                if (coll != null)
                {
                    objectWithCollider = GetComponentInChildren<Collider>().gameObject;
                }
            }

            originalLocalScale = target.transform.localScale;
        }


        private void Update()
        {
            if (inTransition)
            {
                if (isLookedAt)
                {
                    OnLookAt_IncreaseTargetSize();
                }
                else
                {
                    OnLookAway_ReturnToOriginalTargetSize();
                }
            }
        }

        private void OnLookAt_IncreaseTargetSize()
        {
            // Check whether scale is close enough to desired final value
            if (AreWeThereYet(Size_OnLookAt))
            {
                return;
            }

            // Otherwise, let's keep continuously changing the scale
            target.transform.localScale = target.transform.localScale + (Size_OnLookAt - target.transform.localScale).normalized * SizeChangeSpeedEngaged * Time.deltaTime;
        }

        private void OnLookAway_ReturnToOriginalTargetSize()
        {
            // Check whether scale is close enough to desired final value
            if (AreWeThereYet(originalLocalScale))
            {
                target.transform.localScale = originalLocalScale;
                return;
            }

            // Otherwise, let's keep continuously changing the scale
            target.transform.localScale = target.transform.localScale + (originalLocalScale - target.transform.localScale).normalized * SizeChangeSpeedDisengaged * Time.deltaTime;
        }

        private bool AreWeThereYet(Vector3 targetValue)
        {
            if (Mathf.Abs(Vector3.Distance(target.transform.localScale, targetValue)) < SizeDeltaThresh)
            {
                inTransition = false;
                return true;
            }
            return false;
        }
    }
}