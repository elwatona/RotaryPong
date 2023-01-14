using UnityEngine.Events;
using Watona.Events;

namespace RotaryPong.Events
{
    public class BallEffectListener : BaseGameEventListener<BallEffectParameters, BallEffectEvent, UnityEvent<BallEffectParameters>> {}
}