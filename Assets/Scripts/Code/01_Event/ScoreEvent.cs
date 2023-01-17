using UnityEngine;
using Watona.Events;

namespace RotaryPong.Events
{
    [CreateAssetMenu(menuName = "Event/Score")]
    public class ScoreEvent : BaseGameEvent<ScoreParameter>{}
    [System.Serializable]
    public struct ScoreParameter
    {
        public Paint SourceTeam;
        public Vector3 SourcePosition;
    }
}