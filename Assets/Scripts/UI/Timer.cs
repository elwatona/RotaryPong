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
            EnableCount();
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
                case GameState.Versus: if(!_didGameEnded.Value && _canCount) UpdateTimer(); break;
                case GameState.SuddenDeath: _timer.SetValue( _didGameEnded.Value ? "Winner!" : "Sudden Death!"); break;
                case GameState.Pause: _timer.SetValue("Pause"); break;
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
        private void SetTimer()
        {
            _maxMatchTimer = _matchDuration.Value;
        }
        private void UpdateTimer()
        {
            float timer =  _maxMatchTimer -= Time.deltaTime;
            float timerWholeNumbers = Mathf.Floor(timer);
            string textTimer = timer.ToString("0.00");

            if (timer <= 0)
            {
                _timeOutEvent.Raise();
                return;
            }

            _timerSeconds.SetValue(timerWholeNumbers);
            _timer.SetValue(textTimer);            
        }
    }
}
