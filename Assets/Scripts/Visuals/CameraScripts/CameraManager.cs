using Core;
using Core.Context;
using Core.Events;
using Interfaces;
using Systems.EntitySystem.Interfaces;
using UnityEngine;
using Utils;
using Visuals.Interfaces;

namespace Visuals.CameraScripts
{
    public class CameraManager : 
        MonoBehaviour, 
        IClientLateTickable,
        IInitializable<ClientContext>
    {
        private CameraFollow _cameraFollow;
        
        [SerializeField] private Camera mainCamera;
        public CameraSettings cameraSettings;
        public IPhysicalEntity Target { get; private set; }
        public Camera MainCamera => mainCamera;
        private Camera _screenshotCamera;
        public CameraState State { get; private set; } = CameraState.NotInitialized;
        private Vector3 _lastPosition;
        public Vector3 CameraVelocity { get; private set; } = Vector3.zero;
        public void Initialize(ClientContext data)
        {
            if (State != CameraState.NotInitialized)
                return;
            
            Target = data.Player;
            State = Target == null ? 
                CameraState.Idle : 
                CameraState.Following;
            _cameraFollow = new CameraFollow(this);
            mainCamera.enabled = true;
            CreateScreenshotCamera();
            _lastPosition = transform.position;
        }
        
        public void ClientLateTick(float deltaTime)
        {
            if (State != CameraState.Following || Target == null)
                return;
            
            _cameraFollow.FollowEntity();
            CameraVelocity = (transform.position - _lastPosition) / deltaTime;
            _lastPosition = transform.position;
        }
        
        public void CaptureScreenshot(string path)
        {
            _screenshotCamera.Render();

            RenderTexture.active = _screenshotCamera.targetTexture;
            Texture2D tex = new Texture2D(cameraSettings.screenshotWidth, cameraSettings.screenshotHeight, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, cameraSettings.screenshotWidth, cameraSettings.screenshotHeight), 0, 0);
            tex.Apply();

            FileUtils.WriteAllBytes(path, tex.EncodeToPNG());

            RenderTexture.active = null;
            Destroy(tex);
        }


        private void OnEnable()
        {
            ClientGameLoop.Instance.Register(this);
            GameEventBus.Subscribe<PlayerSpawnEvent>(OnPlayerSpawn);
            GameEventBus.Subscribe<ScreenshotRequest>(OnScreenshotRequest);
            State = CameraState.Following;
        }
        
        private void OnDisable()
        {
            ClientGameLoop.Instance?.Unregister(this);
            GameEventBus.Unsubscribe<PlayerSpawnEvent>(OnPlayerSpawn);
            GameEventBus.Unsubscribe<ScreenshotRequest>(OnScreenshotRequest);
            State = CameraState.Idle;
        }
        
        private void OnScreenshotRequest(ScreenshotRequest e)
        {
            CaptureScreenshot(e.Path);
        }
        
        private void OnPlayerSpawn(PlayerSpawnEvent evt)
        {
            Target = evt.Player;
            transform.position = evt.Player.Position.ToVector3() + cameraSettings.offset;
        }
        
        private void CreateScreenshotCamera()
        {
            GameObject camObj = new GameObject("ScreenshotCamera");
            camObj.transform.SetParent(transform);
            camObj.transform.localPosition = Vector3.zero;
            _screenshotCamera = camObj.AddComponent<Camera>();
            _screenshotCamera.enabled = false;
            
            _screenshotCamera.clearFlags = MainCamera.clearFlags;
            _screenshotCamera.backgroundColor = MainCamera.backgroundColor;
            _screenshotCamera.orthographic = MainCamera.orthographic;
            _screenshotCamera.fieldOfView = MainCamera.fieldOfView;
            _screenshotCamera.nearClipPlane = MainCamera.nearClipPlane;
            _screenshotCamera.farClipPlane = MainCamera.farClipPlane;
            
            _screenshotCamera.cullingMask  = -1;
            _screenshotCamera.targetTexture = new RenderTexture(cameraSettings.screenshotWidth, cameraSettings.screenshotHeight, 24);
        }
        
        
    }
    
    public enum CameraState : byte
    {
        NotInitialized,
        Idle,
        Following
    }

}