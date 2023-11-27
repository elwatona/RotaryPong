using UnityEngine;
using Watona.Variables;

namespace RotaryPong
{
    [CreateAssetMenu(menuName = "Variable/Shape")]
    public class ShapeVariable : Variable<Shape>
    {
    }
    [System.Serializable]
    public enum Shape
    {
        Bar = 0,
        C = 1
    }
}
