using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Events;
using Watona.Utils;
using RotaryPong.UI;

namespace RotaryPong
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] CodedEventListener _backToMenuListener;
        [SerializeField] GameEvent _play;
        [SerializeField] GameEvent _settings;
        [SerializeField] GameEvent _exit;
        private VisualElement _root;
        private MainMenu _mainMenu;
        private Button _playBtn;
        private Button _settingsBtn;
        private Button _exitBtn;
        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _mainMenu = _root.Q<MainMenu>();
            _playBtn = _mainMenu.Play;
            _settingsBtn = _mainMenu.Settings;
            _exitBtn = _mainMenu.ExitGame;
        }
        private void OnEnable()
        {
            _backToMenuListener.OnEnable(() => _root.SetVisibility(true));
            _playBtn.clicked += () => _play.Raise();
            _settingsBtn.clicked += () => {_settings.Raise(); _root.SetVisibility(false);};
            _exitBtn.clicked += () => _exit.Raise();
        }
        private void OnDisable()
        {
            _playBtn.clicked -= () => _play.Raise();
            _settingsBtn.clicked -= () => {_settings.Raise(); _root.SetVisibility(false);};
            _exitBtn.clicked -= () => _exit.Raise();
        }
    }
}
