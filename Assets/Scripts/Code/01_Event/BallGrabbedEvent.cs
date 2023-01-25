using UnityEngine;
using Watona.Events;

namespace RotaryPong.Events
{
    [CreateAssetMenu(menuName = "Event/Ball Grabbed")]
    public class BallGrabbedEvent : BaseGameEvent<BallGrabbedParameters>{}
    [System.Serializable]
    public struct BallGrabbedParameters
    {
        public GameObject SourceGrabber;
    }
}