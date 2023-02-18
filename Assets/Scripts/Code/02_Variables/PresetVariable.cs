using System.ComponentModel;
using UnityEngine;
using Watona.Variables;

namespace RotaryPong
{
    [CreateAssetMenu(menuName = "Variable/Preset")]
    public class PresetVariable : Variable<Preset> {}
    [System.Serializable]
    public enum Preset
    {
        Rotary = 0,
        Classic = 1,
        CShape = 2,
        Custom = 3
    }
}
