using UnityEngine;
using System.Runtime.InteropServices;

namespace RotaryPong.UICursor
{
    public class GamepadCursor
    {
        public int PlayerIndex = 0;
        private const string CURSOR_SUBMIT_BUTTON = "Submit";
        private float _cursorSpeed = 1000f;
        private float _padding = 35f;
    #region  dll magic
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public static implicit operator Vector2(POINT p)
            {
                return new Vector2(p.x, p.y);
            }
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);
        const uint MOUSEEVENTF_LEFTDOWN = 0x02, MOUSEEVENTF_LEFTUP = 0x04, MOUSEEVENTF_MOVE = 0x0001;
    #endregion 
        public GamepadCursor()
        {
            PlayerIndex = 0;
        }
        public Vector2 Position(Vector2 lastPosition)
        {
            float moveX = Input.GetAxisRaw(Horizontal());
            float moveY = Input.GetAxisRaw(Vertical());

            lastPosition.x += moveX * _cursorSpeed * Time.unscaledDeltaTime; 
            lastPosition.y += moveY * _cursorSpeed * Time.unscaledDeltaTime; 

            lastPosition.x = Mathf.Clamp(lastPosition.x, _padding, Screen.width - _padding);
            lastPosition.y = Mathf.Clamp(lastPosition.y, _padding, Screen.height - _padding);

            SetCursorPos((int)lastPosition.x, (int)(Screen.height - lastPosition.y));
            
            return lastPosition;
        }
        public void CheckForInput()
        {
            if (Input.GetButtonDown(CURSOR_SUBMIT_BUTTON))
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                Debug.Log("Gamepad Click Pressed");
            }

            if (Input.GetButtonUp(CURSOR_SUBMIT_BUTTON))
            {
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                Debug.Log("Gamepad Click Released");
            }
        }
        private string Horizontal()
        {
            return PlayerIndex != 0 ? $"Player{PlayerIndex}_Horizontal" : "Horizontal";
        }
        private string Vertical()
        {
            return PlayerIndex != 0 ? $"Player{PlayerIndex}_Vertical" : "Vertical";
        }
        public bool IsMoving()
        {
            return Input.GetAxisRaw(Vertical()) != 0 || Input.GetAxisRaw(Horizontal()) != 0;
        }
    }
}