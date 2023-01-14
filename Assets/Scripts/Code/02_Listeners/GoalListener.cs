using UnityEngine.Events;
using Watona.Events;

namespace RotaryPong.Events
{
    public class GoalListener : BaseGameEventListener<Paint, GoalEvent, UnityEvent<Paint>> {}
}