using UnityEngine;
using Watona.Variables;

namespace RotaryPong
{
    [CreateAssetMenu(menuName = "Variable/GameState")]
    public class GameStateVariable : Variable<GameState>{}
    [System.Serializable]
    public enum GameState
    {
        Versus,
        SuddenDeath
    }
}
