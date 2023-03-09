using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UIElements;
using RotaryPong.UI;

namespace RotaryPong.UICursor
{
    [RequireComponent(typeof(PlayerInput))]
    public class GamepadCursor : MonoBehaviour
    {
        [SerializeField] PlayerInput _playerInput;
        [SerializeField] RectTransform _cursorTranform;
        [SerializeField] Canvas _canvas;
        [SerializeField] RectTransform _canvasRectTransform;
        [SerializeField] float _cursorSpeed = 1000f;
        [SerializeField] float _padding = 35f;
        
        private bool _previousMouseState;
        [SerializeField] private Vector2 _lastPosition;
        private Mouse _virtualMouse;
        private Mouse _currentMouse;
        private Camera _mainCamera;

        private string _previousControlScheme = "";
        private const string GAMEPAD_SCHEME = "Gamepad";
        private const string MOUSE_SCHEME = "Keyboard&Mouse";

        private void OnEnable()
        {
            _mainCamera = Camera.main;
            _currentMouse = Mouse.current;
            _playerInput = GetComponent<PlayerInput>();

            AddVirtualMouse();

            //Pair the device to the user to use PlayerInput component with the Event System & the Virtual Mouse
            InputUser.PerformPairingWithDevice(_virtualMouse, _playerInput.user);
            
            if(_cursorTranform != null)
            {
                Vector2 position = _cursorTranform.anchoredPosition;
                InputState.Change(_virtualMouse.position, position);
            }

            AnchorCursorLastPosition();
            ShowCursor(true);

            InputSystem.onAfterUpdate += UpdateMotion;
            _playerInput.onControlsChanged += OnControlsChanged;
        }
    
        private void OnDisable()
        {
            if(_virtualMouse != null && _virtualMouse.added) 
            {
                _lastPosition = _virtualMouse.position.ReadValue();

                InputSystem.RemoveDevice(_virtualMouse);
                print(string.Format("Se eliminó {0}", _virtualMouse));
            }
            InputSystem.onAfterUpdate -= UpdateMotion;
            _playerInput.onControlsChanged -= OnControlsChanged;

            ShowCursor(false);
        }

        private void AddVirtualMouse()
        {
            if (_virtualMouse == null)
            {
                _virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            }
            else if (!_virtualMouse.added)
            {
                InputSystem.AddDevice(_virtualMouse);
            }
        }
        private void AnchorCursorLastPosition()
        {
            InputState.Change(_virtualMouse.position, _lastPosition);
            AnchorCursor(_lastPosition);
        }
        private void ShowCursor(bool value)
        {
            if(_cursorTranform) _cursorTranform.gameObject.SetActive(value);
        }
        private void UpdateMotion()
        {
            Gamepad gamepad = _playerInput.GetDevice<Gamepad>();
            
            if(_virtualMouse == null || gamepad == null) 
            {
                AnchorCursor(_currentMouse.position.ReadValue());
                return;
            }
            Vector2 deltaValue = gamepad.leftStick.ReadValue();
            deltaValue *= _cursorSpeed * Time.unscaledDeltaTime;

            Vector2 currentPosition = _virtualMouse.position.ReadValue();
            Vector2 newPosition = currentPosition + deltaValue;

            newPosition.x = Mathf.Clamp(newPosition.x, _padding, Screen.width - _padding);
            newPosition.y = Mathf.Clamp(newPosition.y, _padding, Screen.height - _padding);

            InputState.Change(_virtualMouse.position, newPosition);
            InputState.Change(_virtualMouse.delta, deltaValue);

            bool acceptButtonIsPressed = gamepad.buttonSouth.IsPressed();
            if(_previousMouseState != acceptButtonIsPressed)
            {
                _virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(UnityEngine.InputSystem.LowLevel.MouseButton.Left, acceptButtonIsPressed);
                InputState.Change(_virtualMouse, mouseState);
                _previousMouseState = acceptButtonIsPressed;
            }

            AnchorCursor(newPosition);
        }

        private void AnchorCursor(Vector2 position)
        {
            Vector2 anchoredPosition;
            Camera camera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _mainCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, position, camera, out anchoredPosition);

            _cursorTranform.anchoredPosition = anchoredPosition;
        }
        private void OnControlsChanged(PlayerInput input)
        {
            if(_playerInput.currentControlScheme == MOUSE_SCHEME && _previousControlScheme != MOUSE_SCHEME)
            {
                _cursorTranform.gameObject.SetActive(false);
                _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
                _previousControlScheme = MOUSE_SCHEME;
                print(_previousControlScheme);
            }
            else if(_playerInput.currentControlScheme == GAMEPAD_SCHEME && _previousControlScheme != GAMEPAD_SCHEME)
            {
                _cursorTranform.gameObject.SetActive(true);
                InputState.Change(_virtualMouse.position, _currentMouse.position.ReadValue());
                AnchorCursor(_currentMouse.position.ReadValue());
                _previousControlScheme = GAMEPAD_SCHEME;
                print(_previousControlScheme);
            }
        }
    }
}
