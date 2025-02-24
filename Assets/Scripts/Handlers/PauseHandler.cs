using UnityEngine;
using Watona.Events;
using Watona.Variables;
using RotaryPong.Events;

namespace RotaryPong
{
    public class PauseHandler : MonoBehaviour
    {
        [SerializeField, Header("Listener & Event")] CodedGameEventListener<PauseParameters> _pauseListener;
        [SerializeField] CodedEventListener _resumeListener;
        [SerializeField, Space] GameEvent _pause;
        [SerializeField, Header("Variables")] BooleanVariable _isPaused;
        private InputManager.Player _currentPlayer;
        private UICursor.CursorController _currentGamepadCursor;
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
                _currentPlayer = null;
                _currentGamepadCursor = null;
            }
            _pause.Raise();
        }
        private void Resume()
        {
            _currentGamepadCursor.enabled = false;
            Pause(false);
        }
        private void CheckPlayer(PauseParameters parameters)
        {
            var player = parameters.sourcePlayerInput;
            _currentGamepadCursor = parameters.sourceGamepadCursor;

            if (_currentGamepadCursor.enabled) _currentGamepadCursor.enabled = false;

            if(_currentPlayer == null) 
            {
                _currentPlayer = player;
                _currentGamepadCursor.enabled = true;
            }

            if(_currentPlayer != player) return;
            Pause(!_isPaused.Value);
        }
    }
}
