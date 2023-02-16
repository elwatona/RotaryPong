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
        [SerializeField] CodedGameEventListener<Paint> _scoreListener;
        [SerializeField] GameEvent _timeOutEvent;

        [SerializeField] BooleanVariable _didGameEnded;
        [SerializeField] FloatVariable _matchDuration;
        [SerializeField] StringVariable _timer;
        [SerializeField] FloatVariable _timerSeconds;
        [SerializeField] GameStateVariable _currentGameState;
        [SerializeField] bool _canCount;

        private float _maxMatchTimer;

        private void Awake()
        {
            SetTimer();
            _canCount = true;
        }
        private void OnEnable()
        {
            _scoreListener?.OnEnable(ScoreAnnounce);
        }
        private void OnDisable()
        {
            _scoreListener?.OnDisable();
        }
        private void Update()
        {
            switch(_currentGameState.Value)
            {
                case GameState.Versus:
                    if(!_didGameEnded.Value && _canCount) 
                    {
                        UpdateTimer();
                        return;
                    }

                break;
                case GameState.SuddenDeath:
                    _timer.SetValue("SUDDEN DEATH!");
                break;
                case GameState.Pause:
                    _timer.SetValue("Pause");
                break;
            }
        }

        private void ScoreAnnounce(Paint team)
        {
            _timer.SetValue("Point");
            _canCount = false;
            Invoke("EnableCount", 2f);
        }
        private void EnableCount()
        {
            _canCount = true;
        }
        ///<summary> Define el valor de <paramref name="_maxMatchTimer"/> </summary>
        private void SetTimer()
        {
            _didGameEnded.SetValue(false);
            _maxMatchTimer = _matchDuration.Value;
        }
        ///<summary> Actualiza la variable <paramref name="_timer"> al tiempo restante de partida </summary>
        private void UpdateTimer()
        {
            float timer =  _maxMatchTimer -= Time.deltaTime;
            float timerWholeNumbers = Mathf.Floor(timer);
            string textTimer = timer.ToString("0.00");
            bool didGameEnded = _didGameEnded.Value;

            if (timer <= 0 && !didGameEnded)
            {
                _didGameEnded.SetValue(true);
                _timeOutEvent.Raise();
                return;
            }

            _timerSeconds.SetValue(timerWholeNumbers);
            _timer.SetValue(textTimer);            
        }
    }
}
