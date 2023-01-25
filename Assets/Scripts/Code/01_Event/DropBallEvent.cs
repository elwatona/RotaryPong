using UnityEngine;
using Watona.Events;

namespace RotaryPong.Events
{
    [CreateAssetMenu(menuName = "Event/Drop Ball")]
    public class DropBallEvent : BaseGameEvent<DropBallParameters>{}
    [System.Serializable]
    public struct DropBallParameters
    {
        public Vector3 SourceDirection;
        public GameObject SourceGrabber;
    }
}