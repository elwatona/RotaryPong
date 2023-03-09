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

        [SerializeField] Animator _uiCanvasAnimator;

        [SerializeField, Header("Parameters")] IntVariable _blueTeamPoints;
        [SerializeField] IntVariable _pinkTeamPoints;
        [SerializeField] StringVariable _timer;
        [SerializeField] GameStateVariable _currentGameState;
        
        [SerializeField, Header("Text")] Text _pinkTeamScoreText;
        [SerializeField] Text _blueTeamScoreText;
        [SerializeField, Space] Text _timerText;
        [SerializeField, Space] Color[] _colors;
        private Color _baseColor;

        private void Awake()
        {
            _baseColor = _timerText.color;
            Cursor.visible = false;
        }
        private void OnEnable()
        {
            _updateScores?.OnEnable(Goal);
            _winner?.OnEnable(EndGame);
        }
        private void OnDisable()
        {
            _updateScores?.OnDisable();
            _winner?.OnDisable();
        }
        private void Start()
        {
            UpdateScoreText(_pinkTeamScoreText, _pinkTeamPoints.Value);
            UpdateScoreText(_blueTeamScoreText, _blueTeamPoints.Value);
        }
        private void Update()
        {
            _timerText.text = _timer.Value;
        }

        private void UpdateScoreText(Text score, int value)
        {
            score.text = value.ToString("00");
        }
        private void UpdateAnnouncementText(string value)
        {
            _timer.SetValue(value);
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

            if(_currentGameState.Value != GameState.SuddenDeath) Invoke("ResetColor", 2);
        }
        private void ResetColor()
        {
            _timerText.color = _baseColor;
        }

        ///<summary> Configura la UI de acuerdo a la pintura (<paramref name="Paint"/>) del equipo </summary>
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
    }
}
