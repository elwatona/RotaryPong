using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Watona.Events;
using Watona.Variables;
using UnityEngine.InputSystem;
using RotaryPong.Events;

namespace RotaryPong
{
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] CodedGameEventListener<Paint> _updateScores;
        [SerializeField] CodedGameEventListener<Paint> _winner;
        [SerializeField] CodedEventListener _pause;

        [SerializeField] Animator _uiCanvasAnimator;

        [SerializeField, Header("Parameters")] IntVariable _blueTeamPoints;
        [SerializeField] IntVariable _pinkTeamPoints;
        [SerializeField] StringVariable _timer;
        [SerializeField] BooleanVariable _isPaused;
        
        [SerializeField, Header("Text")] Text _pinkTeamScoreText;
        [SerializeField] Text _blueTeamScoreText;
        [SerializeField, Space] Text _timerText;
        [SerializeField, Space] Color[] _colors;
        private Color _baseColor;

        private void Awake()
        {
            _isPaused.SetValue(false);
            _baseColor = _timerText.color;
        }
        private void OnEnable()
        {
            _updateScores?.OnEnable(Goal);
            _winner?.OnEnable(EndGame);
            _pause?.OnEnable(() => HandlePause(!_isPaused.Value));
        }
        private void OnDisable()
        {
            _updateScores?.OnDisable();
            _winner?.OnDisable();
            _pause?.OnDisable();
        }
        private void Start()
        {
            UpdateScoreText(_pinkTeamScoreText, _pinkTeamPoints.Value);
            UpdateScoreText(_blueTeamScoreText, _blueTeamPoints.Value);
        }
        private void Update()
        {
            UpdateTimerText();
        }

        private void HandlePause(bool value)
        {
            _isPaused?.SetValue(value);
            Time.timeScale = value ? 0 : 1;
            Cursor.visible = _isPaused ? true : false;
        }
        private void UpdateScoreText(Text score, int value)
        {
            score.text = value.ToString("00");
        }
        private void UpdateAnnouncementText(string value)
        {
            _timer.SetValue(value);
        }
        private void UpdateTimerText()
        {
            _timerText.text = _timer.Value;
        }
        private string EndGameText(Paint team)
        {
            if(team == Paint.White) return "Tie!";
            return string.Format("Winner!");
        }
        private void Goal(Paint team)
        {
            _uiCanvasAnimator.SetTrigger(string.Format("player{0}Score", (int)team));
            _timerText.color = _colors[(int)team];
            
            UpdateScoreText(_pinkTeamScoreText, _pinkTeamPoints.Value);
            UpdateScoreText(_blueTeamScoreText, _blueTeamPoints.Value);

            Invoke("ResetColor", 2);
        }
        public void EndGame(Paint team)
        {
            UpdateAnnouncementText(EndGameText(team));
            
            if(team == Paint.White)
            {
                _timerText.color = _colors[0];
                return;
            }
            _uiCanvasAnimator.SetTrigger(string.Format("player{0}Score", (int)team));
            _timerText.color = _colors[(int)team];
        }
        private void ResetColor()
        {
            _timerText.color = _baseColor;
        }
    }
}
