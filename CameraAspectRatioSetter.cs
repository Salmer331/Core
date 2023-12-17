using System;
using UnityEngine;

namespace Core
{
    [ExecuteAlways]
    public class CameraAspectRatioSetter : MonoBehaviour
    {
        [SerializeField] private Camera _camera = default;    
        [SerializeField] private float _defaultSize = 5f;    
        [SerializeField] private float _targetAspectRatio = 9f / 16;

        private float _cachedAspect = 0;
        private bool _cameraSet = false;
        public void Start()
        {
            if (_camera == null) return;                
            SetScaleByAspectRatio((float)Screen.width / Screen.height);
        }

        public void Update()
        {
            if (!_cameraSet) return;
            var newAspect = (float)Screen.width / Screen.height;
            if (Math.Abs(newAspect - _cachedAspect) < 0.0001f) return;
            
            SetScaleByAspectRatio(newAspect);
        }

        private void SetScaleByAspectRatio(float aspect)
        {
            _cachedAspect = aspect;
            
            float scaleHeight = _targetAspectRatio / aspect;  
            scaleHeight = scaleHeight < 1 ? 1 : scaleHeight;
            _camera.orthographicSize = _defaultSize * scaleHeight;
        }

        private void OnValidate()
        {
            _cameraSet = _camera != null;
        }
    }
}