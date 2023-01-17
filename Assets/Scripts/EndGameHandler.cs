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

        [SerializeField, Space] IntVariable _pinkScore;
        [SerializeField] IntVariable _blueScore;
        [SerializeField] WinnerUIEvent _updateWinnerUI;
        private void CheckWinner()
        {
            int pinkScore = _pinkScore.Value;
            int blueScore = _blueScore.Value;
            Paint winner = pinkScore == blueScore ? Paint.White : pinkScore > blueScore ? Paint.Pink : Paint.Blue;

            _updateWinnerUI.Raise(winner);
        }

        private void OnEnable()
        {
            _timeOutListener.OnEnable(CheckWinner);
        }
        private void OnDisable()
        {
            _timeOutListener.OnDisable();
        }
    }
}
