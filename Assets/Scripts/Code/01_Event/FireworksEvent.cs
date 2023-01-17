using UnityEngine;
using Watona.Events;

namespace RotaryPong.Events
{
    [CreateAssetMenu(menuName = "Event/Fireworks")]
    public class FireworksEvent : BaseGameEvent<FireworksParameter>{}
    [System.Serializable]
    public struct FireworksParameter
    {
        public Vector3 SourcePosition;
        public bool RandomColor;
    }
}
