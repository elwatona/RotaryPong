using UnityEngine;
using Watona.Events;
using UnityEngine.InputSystem;
using RotaryPong.UICursor;

namespace RotaryPong.Events
{
    [CreateAssetMenu(menuName = "Event/Pause")]
    public class PauseEvent : BaseGameEvent<PauseParameters>{}
    public struct PauseParameters
    {
        public PlayerInput sourcePlayerInput;
        public GamepadCursor sourceGamepadCursor;
    }
}