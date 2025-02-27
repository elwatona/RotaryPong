using UnityEngine;
using Watona.Events;
using RotaryPong.UICursor;

namespace RotaryPong.Events
{
    [CreateAssetMenu(menuName = "Event/Pause")]
    public class PauseEvent : BaseGameEvent<int>{}
}