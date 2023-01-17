using UnityEngine;
using Watona.Variables;

namespace RotaryPong
{
    [CreateAssetMenu(menuName = "Variable/ColorArray")]
    public class TeamColorVariable : Variable<TeamColors>{}
    [System.Serializable]
    public struct TeamColors
    {
        public Color[] colors;
    }
}
