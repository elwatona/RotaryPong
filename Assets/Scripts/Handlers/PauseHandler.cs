using UnityEngine;
using Watona.Events;
using Watona.Variables;

namespace RotaryPong
{
    public class PauseHandler : MonoBehaviour
    {
        [SerializeField, Header("Listener & Event")] CodedGameEventListener<int> _pauseListener;
        [SerializeField] CodedEventListener _resumeListener;
        [SerializeField, Space] GameEvent _pause;
        [SerializeField, Header("Variables")] BooleanVariable _isPaused;
        private int _currentPlayer;
        private void OnEnable()
        {
            _pauseListener.OnEnable(CheckPlayer);
            _resumeListener.OnEnable(Resume);
        }
        private void Awake()
        {
            _isPaused.SetValue(false);
        }
        private void OnDisable()
        {
            _pauseListener.OnDisable();
            _resumeListener.OnDisable();
        }
        private void Pause(bool value)
        {
            _isPaused?.SetValue(value);
            Time.timeScale = value ? 0 : 1;
            if(_isPaused.Value == false) 
            {
                _currentPlayer = 0;
            }
            _pause.Raise();
        }
        private void Resume()
        {
            Pause(false);
        }
        private void CheckPlayer(int player)
        {
            if(_currentPlayer == 0) _currentPlayer = player;
            if(_currentPlayer != player) return;
            Pause(!_isPaused.Value);
        }
    }
}
