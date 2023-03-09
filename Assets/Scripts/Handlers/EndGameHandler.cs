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
        [SerializeField] CodedEventListener _scoreListener;
        [SerializeField] CodedEventListener _pauseHandler;

        [SerializeField, Space] IntVariable _pinkScore;
        [SerializeField] IntVariable _blueScore;
        [SerializeField] BooleanVariable _didGameEnded;
        [SerializeField, Space] PaintEvent _updateWinnerUI;
        [SerializeField] GameEvent _endGame;
        [SerializeField] GameStateVariable _currentGameState;
        [SerializeField] BooleanVariable _isPaused;
        private GameState _baseState;
        private void CheckWinner()
        {
            int pinkScore = _pinkScore.Value;
            int blueScore = _blueScore.Value;
            Paint winner = pinkScore == blueScore ? Paint.White : pinkScore > blueScore ? Paint.Pink : Paint.Blue;

            if(winner != Paint.White)
            {
                _didGameEnded.SetValue(true);
                _updateWinnerUI?.Raise(winner);
                _endGame?.Raise();
                return;
            }
            _baseState = GameState.SuddenDeath;
            _currentGameState.SetValue(GameState.SuddenDeath);
        }
        private void AfterScore()
        {
            if(_currentGameState.Value == GameState.SuddenDeath) CheckWinner();
        }
        private void PauseToggle(bool value)
        {
            _isPaused.SetValue(value);
            _currentGameState.SetValue(value ? GameState.Pause : _baseState);
        }

        private void OnEnable()
        {
            _timeOutListener.OnEnable(CheckWinner);
            _scoreListener.OnEnable(AfterScore);
            _pauseHandler.OnEnable(() => PauseToggle(_isPaused.Value));
            
            _currentGameState?.SetValue(GameState.Versus);
            _didGameEnded.SetValue(false);
            _baseState = GameState.Versus;
        }
        private void OnDisable()
        {
            _timeOutListener.OnDisable();
            _scoreListener.OnDisable();
            _pauseHandler.OnDisable();
        }
    }
}
