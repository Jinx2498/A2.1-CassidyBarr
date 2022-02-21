using System.Collections.Generic;
using GameBrains.Weebles;
using UnityEngine;

namespace GameBrains.Cameras
{
    public abstract class TargetedCamera : SelectableCamera
    {
        // The camera's target.
        public Transform Target { get => target; set => target = value; }
        [SerializeField] protected Transform target;
        [SerializeField] bool warnIfNoTarget;

        static List<TargetedCamera> targetedCameras;

        public override void Awake()
        {
            if (targetedCameras == null)
            {
                targetedCameras = new List<TargetedCamera>();
                var cameras = FindObjectsOfType<Camera>();

                foreach (Camera cam in cameras)
                {
                    targetedCameras.AddRange(cam.GetComponents<TargetedCamera>());
                }
            }

            if (warnIfNoTarget && !Target) { Log.Debug("Please assign a target to the camera."); }

            base.Awake();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            if (!Target)
            {
                var weeble = FindObjectOfType<Weeble>();
                if (weeble) { Target = weeble.transform; }
            }
        }
    }
}