using UnityEngine;
using Watona.Events;

namespace RotaryPong.Events
{
    [CreateAssetMenu(menuName = "Event/SpinMap")]
    public class SpinMapInputEvent : BaseGameEvent<SpinMapInputParameter>{}
    [System.Serializable]
    public struct SpinMapInputParameter
    {
        public Paint SourceTeam;
        public float SourceDirection;
    }
}