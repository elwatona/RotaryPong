using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Watona.Events;
using Watona.Variables;
using RotaryPong.Events;

namespace RotaryPong
{
    public class ScoreHandler : MonoBehaviour
    {
        [SerializeField] CodedGameEventListener<ScoreParameter> _scoreListener;
        [SerializeField, Header("Events")] PaintEvent _updateScores;
        [SerializeField] FireworksEvent _fireworks;
        [SerializeField] GameEvent _spinMap;
        [SerializeField, Header("Variables")] IntVariable _pinkScore;
        [SerializeField] IntVariable _blueScore;

        ///<summary> Agrega un punto a la puntuacion de <paramref name="team"/> </summary>
        ///<param name="team"> El color de equipo quien hizo punto </param> 
        private void Point(ScoreParameter parameter)
        {
            FireworksParameter fireworksParameter = new FireworksParameter {SourcePosition = parameter.SourcePosition, RandomColor = false};

            switch((int)parameter.SourceTeam)
            {
                case 1:
                    _pinkScore.ApplyChange(1);
                break;
                case 2:
                    _blueScore.ApplyChange(1);
                break;
            }

            _updateScores?.Raise(parameter.SourceTeam);
            _spinMap?.Raise();
            _fireworks?.Raise(fireworksParameter);
        }
        ///<summary> Reinicia los valores de puntaje a 0 </summary>
        private void ResetPoints()
        {
            _blueScore.SetValue(0);
            _pinkScore.SetValue(0);
        }

        private void OnEnable()
        {
            _scoreListener?.OnEnable(Point);
        }
        private void OnDisable()
        {
            _scoreListener?.OnDisable();
        }
        private void Start()
        {
            ResetPoints();
        }
    }
}
