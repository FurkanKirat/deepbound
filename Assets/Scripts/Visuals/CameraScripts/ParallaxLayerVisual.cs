using UnityEngine;

namespace Visuals.CameraScripts
{
    public class ParallaxLayerVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer first;
        [SerializeField] private SpriteRenderer second;

        private float _factor;
        private float _width;
        private float _height;
        private float _baseSpeed;
        private CameraManager _cameraManager;
        private float _speed;

        public void Initialize(
            string objectName,
            CameraManager cameraManager,
            Sprite sprite, 
            float factor, 
            float baseSpeed,
            int sortingOrder)
        {
            name = objectName;
            _cameraManager = cameraManager;
            var startPosition = _cameraManager.transform.position;
            first.sprite = sprite;
            second.sprite = sprite;
            first.sortingOrder = sortingOrder;
            second.sortingOrder = sortingOrder;
            Vector2 rawSize = new Vector2(
                first.sprite.rect.width / first.sprite.pixelsPerUnit,
                first.sprite.rect.height / first.sprite.pixelsPerUnit
            );
            
            float worldScreenHeight = _cameraManager.MainCamera.orthographicSize * 2f;
            
            float ratio = worldScreenHeight / rawSize.y;
            
            Vector3 ratioVec = new Vector3(ratio, ratio, 1f);
            first.transform.localScale = ratioVec;
            second.transform.localScale = ratioVec;
            
            _width = rawSize.x * ratio;
            _height = rawSize.y * ratio;
            
            _baseSpeed = baseSpeed;
            _factor = factor;
            
            first.transform.position = new Vector3(startPosition.x, startPosition.y, 0);
            second.transform.position = new Vector3(startPosition.x - _width , startPosition.y, 0);
            _speed = _baseSpeed * _factor;
        }

        public void UpdateMovement(float deltaTime)
        {
            var cameraPos = _cameraManager.transform.position;
            var cameraVelocity = _cameraManager.CameraVelocity;
            var mainCamera = _cameraManager.MainCamera;
            SpriteRenderer front;
            SpriteRenderer back;
            if (IsFirstInRight())
            {
                front = first;
                back = second;
            }
            else
            {
                front = second;
                back = first;
            }
            
            // var frontTransform = front.transform;
            // var backTransform = back.transform;

            // var xMovement = _cameraManager.CameraVelocity * _speed * deltaTime;
            //
            // frontTransform.position += xMovement;
            // backTransform.position += xMovement;
            // if (frontTransform.position.x + _width <= mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect)
            // {
            //     frontTransform.position = new Vector3(backTransform.position.x + _width, frontTransform.position.y, frontTransform.position.z);
            // }
            
            first.transform.position = new Vector3(cameraPos.x, cameraPos.y, 0);
            //first.transform.position += cameraVelocity * deltaTime * _speed;
        }

        private bool IsFirstInRight()
            => first.transform.localPosition.x > second.transform.localPosition.x;
    }
}