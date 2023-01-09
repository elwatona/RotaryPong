using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Watona.Utils;
using Watona.Utils.Variables;

namespace RotaryPong
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [SerializeField, Header("Parameters")] IntReference _blueTeamPoints;
        [SerializeField] IntReference _pinkTeamPoints;
        [SerializeField, Space] FloatReference _timer;
        [SerializeField, Space] Animator _uiCanvasAnimator;
        [SerializeField, Header("Text")] Text _pinkTeamText;
        [SerializeField] Text _pinkTeamScoreText;
        [SerializeField, Space] Text _blueTeamText;
        [SerializeField] Text _blueTeamScoreText;
        [SerializeField, Space] Text _announcementScoreText;
        [SerializeField, Space] Text _timerText;
        [SerializeField, Space] Color[] _colors;

        public void Goal(Paint team)
        {
            string announcement = string.Format("Point for {0}", team);
            
            _announcementScoreText.gameObject.SetActive(true);
            _uiCanvasAnimator.SetTrigger(string.Format("player{0}Score", (int)team));
            _announcementScoreText.color = _colors[(int)team];
            _announcementScoreText.text = announcement;
            
            UpdateScoreText(_pinkTeamScoreText, _pinkTeamPoints.Value);
            UpdateScoreText(_blueTeamScoreText, _blueTeamPoints.Value);
        }
        private void UpdateScoreText(Text score, int value)
        {
            score.text = value.ToString();
        }
        public void UpdateTimerText(float value)
        {
            _timerText.text = value.ToString();
        }
        public void EndGame(Paint team)
        {
            string announcement = "";
            _announcementScoreText.gameObject.SetActive(true);
            
            if(team == Paint.White)
            {
                announcement = "The game ends in a tie!";
                _announcementScoreText.color = _colors[0];
                _announcementScoreText.text = announcement;
                return;
            }
            announcement = string.Format("{0} team wins the game", team);
            _uiCanvasAnimator.SetTrigger(string.Format("player{0}Score", (int)team));
            _announcementScoreText.color = _colors[(int)team];
            _announcementScoreText.text = announcement;
        }
    }
}
