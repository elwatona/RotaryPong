using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watona.Variables;

namespace RotaryPong
{
    public class MainMenuBackground : MonoBehaviour
    {
        [SerializeField] FloatVariable _spinSpeed;
        private void FixedUpdate()
        {
            float speedRot = _spinSpeed.Value * Time.deltaTime * -1;
            gameObject.transform.Rotate(0,0,speedRot);
        }
    }
}
