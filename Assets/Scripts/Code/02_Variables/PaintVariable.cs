using UnityEngine;
using Watona.Variables;

namespace RotaryPong
{
    [CreateAssetMenu(menuName = "Variable/Paint")]
    public class PaintVariable : Variable<Paint>{}
    [System.Serializable]
    public enum Paint
    {
        White,
        Pink,
        Blue
    }
}
