using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watona.Variables;
using Watona.Events;
using RotaryPong.Events;

namespace RotaryPong
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] GameEvent _timeOutEvent;

        [SerializeField] BooleanVariable _didGameEnded;
        [SerializeField] FloatVariable _matchDuration;
        [SerializeField] StringVariable _timer;
        [SerializeField] FloatVariable _timerSeconds;
        [SerializeField] GameStateVariable _currentGameState;

        private float _maxMatchTimer;

        ///<summary> Define el valor de <paramref name="_maxMatchTimer"/> </summary>
        private void SetTimer()
        {
            _didGameEnded.SetValue(false);
            _maxMatchTimer = _matchDuration.Value;
        }
        ///<summary> Actualiza la variable <paramref name="_timer"> al tiempo restante de partida </summary>
        private void UpdateTimer()
        {
            float timer = _maxMatchTimer -= Time.deltaTime;
            bool didGameEnded = _didGameEnded.Value;

            if (timer <= 0 && !didGameEnded)
            {
                _didGameEnded.SetValue(true);
                _timeOutEvent.Raise();
                return;
            }

            float timerWholeNumbers = Mathf.Floor(timer);
            float gameTimer = timer;

            gameTimer *= 100;
            gameTimer = Mathf.Floor(gameTimer);

            float timerDecimals = gameTimer - (timerWholeNumbers * 100);
            string extraNum = "";

            CheckTimer(timerDecimals, extraNum);

            string textTimer = timer.ToString("0.00");
            // timerWholeNumbers + "." + extraNum + timerDecimals;

            _timerSeconds.SetValue(timerWholeNumbers);
            _timer.SetValue(textTimer);            
        }
        ///<summary> Define el valor de <paramref name="extra"/> dependiendo <paramref name="decimals"/> </summary>
        private void CheckTimer(float decimals, string extra)
        {
            if (decimals < 10)
            {
                extra = "0";
                return;
            }
            extra = "";
        }

        private void Awake()
        {
            SetTimer();
        }
        private void Update()
        {
            if(!_didGameEnded.Value && _currentGameState.Value != GameState.SuddenDeath) 
            {
                UpdateTimer();
                return;
            }
            _timer.SetValue("SUDDEN DEATH!");
        }
    }
}
