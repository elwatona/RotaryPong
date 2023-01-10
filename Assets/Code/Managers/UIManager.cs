using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Watona.Utils;
using Watona.Utils.Variables;

namespace RotaryPong
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        ///<summary> Define si el GameObject ingresado esta activo o no despues de <paramref name="seconds"/> </summary>
        IEnumerator SetActiveAfterSeconds(GameObject gameObject, float seconds, bool value)
        {
            yield return new WaitForSeconds(seconds);
            gameObject.SetActive(value);
        }
        
        [SerializeField] Animator _uiCanvasAnimator;

        [SerializeField, Header("Parameters")] IntReference _blueTeamPoints;
        [SerializeField] IntReference _pinkTeamPoints;
        [SerializeField] StringReference _announcement;
        [SerializeField] StringReference _timer;

        [SerializeField, Header("Text")] Text _pinkTeamText;
        [SerializeField] Text _pinkTeamScoreText;
        [SerializeField, Space] Text _blueTeamText;
        [SerializeField] Text _blueTeamScoreText;
        [SerializeField, Space] Text _announcementText;
        [SerializeField, Space] Text _timerText;
        [SerializeField, Space] Color[] _colors;

        ///<summary> Actualiza el texto de <paramref name="score"/> </summary>
        ///<param name="score"> El Text a modificar </param>
        ///<param name="value"> El valor a mostrar </param>
        private void UpdateScoreText(Text score, int value)
        {
            score.text = value.ToString();
        }
        ///<summary> Actualiza el texto de <paramref name="_announcementText"/> segun el valor de <paramref name="_announcement"/> </summary>
        private void UpdateAnnouncementText()
        {
            _announcementText.text = _announcement.Value;
        }
        ///<summary> Modifica valores de interfaz a la hora de anotar un gol </summary>
        public void Goal(Paint team)
        {
            _announcementText.gameObject.SetActive(true);
            _uiCanvasAnimator.SetTrigger(string.Format("player{0}Score", (int)team));
            _announcementText.color = _colors[(int)team];
            
            UpdateAnnouncementText();
            UpdateScoreText(_pinkTeamScoreText, _pinkTeamPoints.Value);
            UpdateScoreText(_blueTeamScoreText, _blueTeamPoints.Value);

            StartCoroutine(SetActiveAfterSeconds(_announcementText.gameObject, 2, false));
        }
        ///<summary> Actualiza el texto de <paramref name="_timerText"/> segun el valor de <paramref name="_timer"/> </summary>
        public void UpdateTimerText()
        {
            _timerText.text = _timer.Value;
        }
        ///<summary> Maneja la logica de interfaz a ocurrir cuando termina la partida </summary>
        public void EndGame(Paint team)
        {
            UpdateAnnouncementText();
            _announcementText.gameObject.SetActive(true);
            
            if(team == Paint.White)
            {
                _announcementText.color = _colors[0];
                return;
            }
            _uiCanvasAnimator.SetTrigger(string.Format("player{0}Score", (int)team));
            _announcementText.color = _colors[(int)team];
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
    }
}
