using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Watona.Events;
using Watona.Variables;

namespace RotaryPong.UICursor
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private RectTransform _cursorTransform;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CodedEventListener _pauseListener;
        [SerializeField] private CodedGameEventListener<int> _playerPausedListener;
        [SerializeField] private BooleanVariable _pause;
        private GamepadCursor _gamepad;
        private Vector2 _lastPosition;
        private Camera _mainCamera;

        private void OnEnable()
        {
            _gamepad = new();
            _mainCamera = Camera.main;
            SetCursorConfiguration();
            SceneManager.sceneLoaded += OnSceneChanged;
            _pauseListener.OnEnable(() => SetCanvasActive(_pause.Value));
            _playerPausedListener.OnEnable((x) => _gamepad.PlayerIndex = x);
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneChanged;
            _pauseListener.OnDisable();
            _playerPausedListener.OnDisable();
        }

        private void Update()
        {
            if(!_canvas.enabled) return;
            _gamepad.CheckForInput();
            UpdateCursorPosition();
        }

        private void SetCursorConfiguration()
        {
        #if UNITY_EDITOR
            Cursor.visible = true;
        #elif UNITY_STANDALONE
            Cursor.visible = false;
        #endif
            Cursor.lockState = CursorLockMode.Confined;
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
        private void OnSceneChanged(Scene scene, LoadSceneMode mode)
        {
            switch(scene.name)
            {
                case "MainMenu":
                SetCanvasActive(true);
                break;

                case "GameScene":
                SetCanvasActive(false);
                break;
            }
        }
        private void SetCanvasActive(bool value)
        {
            _canvas.enabled = value;
        }
    }
}
