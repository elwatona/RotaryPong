using UnityEngine;
using Watona.Events;

namespace RotaryPong.Events
{
    [CreateAssetMenu(menuName = "Event/BallEffect")]
    public class BallEffectEvent : BaseGameEvent<BallEffectParameters>{}
    [System.Serializable]
    public struct BallEffectParameters
    {
        public Paint SourceTeam;
        public Vector2 SourceInput;
    }
}