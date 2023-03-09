using UnityEngine;

namespace RotaryPong
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXVolumen : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<AudioSource>().volume = Resources.Load<Watona.Variables.FloatVariable>("SFX Volumen").Value / 100;
        }
    }
}
