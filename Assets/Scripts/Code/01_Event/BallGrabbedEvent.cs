using UnityEngine;
using Watona.Events;

namespace RotaryPong.Events
{
    [CreateAssetMenu(menuName = "Event/GameObject")]
    public class BallGrabbedEvent : BaseGameEvent<BallGrabbedParameters>{}
    [System.Serializable]
    public struct BallGrabbedParameters
    {
        public GameObject SourceGrabber;
        public GameObject Ball;
    }
}