using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RotaryPong
{
    [RequireComponent(typeof(EventSystem))]
    public class EventSystemHandler : MonoBehaviour
    {
        EventSystem _eventSystem;
        private void OnEnable()
        {
            _eventSystem = GetComponent<EventSystem>();
        }
        public void SetSelectedGameObject()
        {
            // _eventSystem.SetSelectedGameObject(option1.activeSelf ? option1 : option2);
        }
    }
}
