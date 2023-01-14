using UnityEngine;
using Watona.Events;

namespace RotaryPong.Events
{
    [CreateAssetMenu(menuName = "Event/Goal")]
    public class GoalEvent : BaseGameEvent<Paint>{}
}