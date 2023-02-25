using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UIElements;

using RotaryPong.UI;

namespace RotaryPong
{
    public class GamepadCursor : MonoBehaviour
    {
        [SerializeField] PlayerInput playerInput;
        [SerializeField] RectTransform cursorTranform;
        [SerializeField] Canvas canvas;
        [SerializeField] RectTransform canvasRectTransform;
        // [SerializeField] UIDocument uIDocument;
        // [SerializeField] VisualElement cursorUI;
        [SerializeField] float cursorSpeed = 1000f;
        [SerializeField] float padding = 35f;
        
        private bool previousMouseState;
        private Mouse virtualMouse;
        private Mouse currentMouse;
        private Camera mainCamera;

        private string previousControlScheme = "";
        private const string gamepadScheme = "Gamepad";
        private const string mouseScheme = "Keyboard&Mouse";
        private void OnEnable()
        {
            mainCamera = Camera.main;
            currentMouse = Mouse.current;
            // cursorUI = new CursorUI();
            // uIDocument.rootVisualElement.Add(cursorUI);
            // cursorUI.SendToBack();

            AddVirtualMouse();

            //Pair the device to the user to use PlayerInput component with the Event System & the Virtual Mouse
            InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

            if(cursorTranform != null)
            {
                Vector2 position = cursorTranform.anchoredPosition;
                InputState.Change(virtualMouse.position, position);
            }

            InputSystem.onAfterUpdate += UpdateMotion;
            playerInput.onControlsChanged += OnControlsChanged;
        }
        private void OnDisable()
        {
            if(virtualMouse != null && virtualMouse.added) InputSystem.RemoveDevice(virtualMouse);
            InputSystem.onAfterUpdate -= UpdateMotion;
            playerInput.onControlsChanged -= OnControlsChanged;
        }

        private void AddVirtualMouse()
        {
            if (virtualMouse == null)
            {
                virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            }
            else if (!virtualMouse.added)
            {
                InputSystem.AddDevice(virtualMouse);
            }
        }
        private void UpdateMotion()
        {
            if(virtualMouse == null || Gamepad.current == null) return;

            Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
            deltaValue *= cursorSpeed * Time.deltaTime;

            Vector2 currentPosition = virtualMouse.position.ReadValue();
            Vector2 newPosition = currentPosition + deltaValue;

            newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
            newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

            InputState.Change(virtualMouse.position, newPosition);
            InputState.Change(virtualMouse.delta, deltaValue);

            bool acceptButtonIsPressed = Gamepad.current.buttonSouth.IsPressed();
            if(previousMouseState != acceptButtonIsPressed)
            {
                virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(UnityEngine.InputSystem.LowLevel.MouseButton.Left, acceptButtonIsPressed);
                InputState.Change(virtualMouse, mouseState);
                previousMouseState = acceptButtonIsPressed;
            }

            AnchorCursor(newPosition);
        }

        private void AnchorCursor(Vector2 position)
        {
            Vector2 anchoredPosition;
            Camera camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, camera, out anchoredPosition);

            cursorTranform.anchoredPosition = anchoredPosition;

            // SetCursorUIPosition(cursorUI, position);
        }
        private void OnControlsChanged(PlayerInput input)
        {
            if(playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme)
            {
                cursorTranform.gameObject.SetActive(false);
                UnityEngine.Cursor.visible = true;
                currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
                previousControlScheme = mouseScheme;
            }
            else if(playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme)
            {
                cursorTranform.gameObject.SetActive(true);
                UnityEngine.Cursor.visible = false;
                InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
                AnchorCursor(currentMouse.position.ReadValue());
                previousControlScheme = gamepadScheme;
            }
        }
        private void SetCursorUIPosition(VisualElement cursor, Vector2 position)
        {
            Vector2 positionCorrected = new Vector2(position.x - 24, Screen.height - position.y - 24);
            Vector2 newPosition = RuntimePanelUtils.ScreenToPanel(cursor.panel, positionCorrected);
            cursor.transform.position = newPosition;
        }
    }
}
