using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watona.Events;
using UnityEngine.SceneManagement;

namespace RotaryPong
{
    public class ScreenHandler : MonoBehaviour
    {
        [SerializeField] CodedEventListener _exitButtonListener;
        [SerializeField] CodedEventListener _playButtonListener;

        [SerializeField] GameObject _mainMenu;
        [SerializeField] GameObject _settings;

        private void OnEnable()
        {
            _playButtonListener.OnEnable(Play);
            _exitButtonListener.OnEnable(Exit);
        }
        private void OnDisable()
        {
            _playButtonListener.OnDisable();
            _exitButtonListener.OnDisable();
        }
        private void Play()
        {
            SceneManager.LoadScene(1);
        }
        private void Exit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
            return;
        }
    }
}
