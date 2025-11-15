using UnityEngine;

namespace Visuals.CameraScripts
{
    public class CameraFollow
    {
        private Vector3 _velocity;
        
        private readonly CameraManager _cameraManager;
        private CameraSettings CameraSettings => _cameraManager.cameraSettings;

        public CameraFollow(CameraManager cameraManager)
        {
            _cameraManager = cameraManager;
            _cameraManager.transform.position = CameraSettings.offset + _cameraManager.Target.Position.ToVector3();

        }
        public void FollowEntity()
        {
            var target = _cameraManager.Target;
            if (target == null) return;

            var deadZone = CameraSettings.deadZone;
            var offset = CameraSettings.offset;
            var smoothTime = CameraSettings.smoothTime;

            Vector3 targetPosition = target.Position.ToVector3();
            Vector3 delta = targetPosition - _cameraManager.transform.position + offset;

            if (Mathf.Abs(delta.x) > deadZone.x || Mathf.Abs(delta.y) > deadZone.y)
            {
                Vector3 desired = new Vector3(targetPosition.x, targetPosition.y, _cameraManager.transform.position.z);
                _cameraManager.transform.position = Vector3.SmoothDamp(_cameraManager.transform.position, desired, ref _velocity, smoothTime);
            }
        }
    }
}