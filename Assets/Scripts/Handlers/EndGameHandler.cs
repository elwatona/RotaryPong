using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Watona.Events;
using Watona.Variables;
using RotaryPong.Events;

namespace RotaryPong
{
    public class EndGameHandler : MonoBehaviour
    {
        [SerializeField] CodedEventListener _timeOutListener;
        [SerializeField] CodedEventListener _afterScoreListener;

        [SerializeField, Space] IntVariable _pinkScore;
        [SerializeField] IntVariable _blueScore;
        [SerializeField] BooleanVariable _didGameEnded;
        [SerializeField, Space] PaintEvent _updateWinnerUI;
        [SerializeField] GameEvent _endGame;
        [SerializeField] GameStateVariable _currentGameState;
        private void CheckWinner()
        {
            int pinkScore = _pinkScore.Value;
            int blueScore = _blueScore.Value;
            Paint winner = pinkScore == blueScore ? Paint.White : pinkScore > blueScore ? Paint.Pink : Paint.Blue;

            if(winner != Paint.White)
            {
                _updateWinnerUI?.Raise(winner);
                _endGame?.Raise();
                _didGameEnded.SetValue(true);
                return;
            }
            _currentGameState.SetValue(GameState.SuddenDeath);
        }
        private void AfterScore()
        {
            if(_currentGameState.Value != GameState.Versus) CheckWinner();
        }

        private void OnEnable()
        {
            _timeOutListener.OnEnable(CheckWinner);
            _afterScoreListener.OnEnable(AfterScore);
            
            _currentGameState?.SetValue(GameState.Versus);
        }
        private void OnDisable()
        {
            _timeOutListener.OnDisable();
            _afterScoreListener.OnDisable();
        }
    }
}
