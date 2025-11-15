using System;
using UnityEngine;

namespace Visuals.CameraScripts
{
    [Serializable]
    public class CameraSettings
    {
        [Header("Camera follow")]
        public Vector3 offset;
        public Vector2 deadZone;
        public float smoothTime;
        [Header("Screenshot")] 
        public int screenshotWidth;
        public int screenshotHeight;
    }
}