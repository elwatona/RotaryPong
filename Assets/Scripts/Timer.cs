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
        [SerializeField] GameEvent EndGame;

        [SerializeField] BooleanVariable _didGameEnded;
        [SerializeField] FloatVariable _matchDuration;
        [SerializeField] StringVariable _timer;
        [SerializeField] FloatVariable _timerSeconds;

        private float _maxMatchTimer;

        ///<summary> Define el valor de <paramref name="_maxMatchTimer"/> </summary>
        private void SetTimer()
        {
            _maxMatchTimer = _matchDuration.Value;
        }
        ///<summary> Actualiza la variable <paramref name="_timer"> al tiempo restante de partida </summary>
        private void UpdateTimer()
        {
            float timer = _maxMatchTimer -= Time.deltaTime;
            bool didGameEnded = _didGameEnded.Value;

            if (timer <= 0 && !didGameEnded)
            {
                didGameEnded = true;
                EndGame.Raise();
                return;
            }

            float gameTimerWholeNums = Mathf.Floor(timer);
            float gameTimer = timer;

            gameTimer *= 100;
            gameTimer = Mathf.Floor(gameTimer);

            float gameTimerDecimals = gameTimer - (gameTimerWholeNums * 100);
            string extraNum = "";

            CheckTimer(gameTimerDecimals, extraNum);

            string textTimer = gameTimerWholeNums + "." + extraNum + gameTimerDecimals;

            _timerSeconds.SetValue(gameTimerWholeNums);
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
            UpdateTimer();
        }
    }
}
