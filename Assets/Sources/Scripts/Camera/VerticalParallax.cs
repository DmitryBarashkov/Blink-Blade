using UnityEngine;

namespace BlinkBlade.Camera
{
    public class VerticalParallax : MonoBehaviour
    {
        [SerializeField][Range(0f, 1f)] private float parallaxEffectY;

        private float _lastCameraY;
        private float _textureUnitSizeY;
        private Transform _cameraTransform;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;

            if (_cameraTransform == null)
                _cameraTransform = UnityEngine.Camera.main.transform;

            _lastCameraY = _cameraTransform.position.y;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                Sprite sprite = spriteRenderer.sprite;
                Texture2D texture = sprite.texture;
                _textureUnitSizeY = sprite.bounds.size.y;
            }
        }

        private void LateUpdate()
        {
            if (_cameraTransform == null)
                _cameraTransform = UnityEngine.Camera.main.transform;

            float deltaY = _cameraTransform.position.y - _lastCameraY;

            transform.position += new Vector3(0f, deltaY * parallaxEffectY, 0f);
            _lastCameraY = _cameraTransform.position.y;

            if (_textureUnitSizeY > 0f)
            {
                if (Mathf.Abs(_cameraTransform.position.y - _transform.position.y) >= _textureUnitSizeY)
                {
                    float offsetPositionBackgroundY = (_cameraTransform.position.y - _transform.position.y) % _textureUnitSizeY;

                    _transform.position = new Vector3(_transform.position.x, _cameraTransform.position.y + offsetPositionBackgroundY, _transform.position.z);
                }
            }
        }
    }
}
