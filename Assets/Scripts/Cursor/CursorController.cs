using UnityEngine;

namespace RotaryPong.UICursor
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private RectTransform _cursorTransform;
        [SerializeField] private Canvas _canvas;
        private GamepadCursor _gamepad;
        private Vector2 _lastPosition;
        private Camera _mainCamera;

        private void OnEnable()
        {
            _gamepad = new();

            _mainCamera = Camera.main;
        #if UNITY_EDITOR
            Cursor.visible = true;
        #elif UNITY_STANDALONE
            Cursor.visible = false;
        #endif
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update() 
        { 
            UpdateCursorPosition();
            _gamepad.CheckForInput();
        }

        private void UpdateCursorPosition()
        {
            Vector2 desiredMovement = _gamepad.IsMoving() ? _gamepad.Position(_lastPosition) : Input.mousePosition;
            AnchorCursor(desiredMovement);
        }

        private void AnchorCursor(Vector2 position)
        {
            _lastPosition = position;

            Vector2 anchoredPosition;
            Camera camera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _mainCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, position, camera, out anchoredPosition);

            _cursorTransform.anchoredPosition = anchoredPosition;
        }
    }
}
