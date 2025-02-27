using System;
using UnityEngine;
using UnityEngine.Events;
namespace RotaryPong
{
    public class InputManager : MonoBehaviour
    {
        public static readonly string[] ACTIONS = { "RotateLeft", "RotateRight", "BallEffect", "RotateMap", "Drop", "Pause"};
        [Serializable]
        public class Player
        {
            [Serializable]
            private struct InputEvent
            {
                public enum ActionType { Button, Axis, Analog };
                [SerializeField] private ActionType _type;
                [SerializeField] private bool _continuousCallback;
                [SerializeField] private UnityEvent<bool> _buttonEvent;
                [SerializeField] private UnityEvent<float> _axisEvent;
                [SerializeField] private UnityEvent<Vector2> _analogEvent;
                public ActionType Type => _type;
                public bool ContinuousCallback => _continuousCallback;
                public UnityEvent<bool> ButtonEvent => _buttonEvent;
                public UnityEvent<float> AxisEvent => _axisEvent;
                public UnityEvent<Vector2> AnalogEvent => _analogEvent;
            }
            [SerializeField] private InputEvent[] _inputEvents;
            [SerializeField] private UnityEvent<Vector2> _joystickEvent;
            private string _playerPrefix;
            private Vector2 _joystick => new Vector2(Input.GetAxis(_playerPrefix + "Horizontal"), Input.GetAxis(_playerPrefix + "Vertical"));

            public void SetPrefix(string value)
            {
                _playerPrefix = value;
            }

            private bool GetInput(int index, bool continuous)
            {
                string action = _playerPrefix + ACTIONS[index];
                Debug.Log(action + " " + index);
                return continuous ? Input.GetButton(action) : Input.GetButtonDown(action);
            }
            private float GetAxis(int index)
            {
                string action = _playerPrefix + ACTIONS[index];
                Debug.Log(action + " " + index);
                return Input.GetAxis(action);
            }
            private Vector2 GetAnalog(int index)
            {
                string action = _playerPrefix + ACTIONS[index];
                Debug.Log(action + " " + index);
                Vector2 input = new Vector2(Input.GetAxis(action + "_Horizontal"), Input.GetAxis(action + "_Vertical"));
                if(input != Vector2.zero) Debug.Log(input);
                return input;
            }
            public void CheckInputs()
            {
                for (int i = 0; i < _inputEvents.Length; i++)
                {
                    if(i >= ACTIONS.Length)
                    {
                        Debug.LogAssertion("El input ingresado no está registrado.");
                        break;
                    }
                    InputEvent inputEvent = _inputEvents[i];
                    switch(inputEvent.Type)
                    {
                        case InputEvent.ActionType.Axis:
                            inputEvent.AxisEvent?.Invoke(GetAxis(i));
                            continue;
                        case InputEvent.ActionType.Button:
                            inputEvent.ButtonEvent?.Invoke(GetInput(i, inputEvent.ContinuousCallback));
                            continue;
                        case InputEvent.ActionType.Analog:
                            inputEvent.AnalogEvent?.Invoke(GetAnalog(i));
                            continue;
                    }
                }
            }

            public void CheckJoystick()
            {
                _joystickEvent?.Invoke(_joystick);
            }
        }
        [SerializeField] private bool _canDebug;
        [SerializeField] private Player[] _players;
        private void OnEnable()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].SetPrefix($"Player{i+1}_");
            }
        }
        private void Update()
        {
            foreach(Player player in _players)
            {
                player.CheckJoystick();
                player.CheckInputs();
            }
        }
        public void TestInput(Vector2 input)
        {
            if(_canDebug) Debug.Log(input);
        }
        public void TestInput(bool input)
        {
            if(_canDebug) Debug.Log(input);
        }
    }
}