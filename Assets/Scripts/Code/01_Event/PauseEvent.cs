using UnityEngine;
using Watona.Events;
using RotaryPong.UICursor;

namespace RotaryPong.Events
{
    [CreateAssetMenu(menuName = "Event/Pause")]
    public class PauseEvent : BaseGameEvent<PauseParameters>{}
    public struct PauseParameters
    {
        public InputManager.Player sourcePlayerInput;
        public CursorController sourceGamepadCursor;
    }
}