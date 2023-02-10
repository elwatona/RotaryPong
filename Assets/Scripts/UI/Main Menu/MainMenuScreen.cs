using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Events;
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
        private Button _playBtn;
        private Button _settingsBtn;
        private Button _exitBtn;
        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _playBtn = GetButton("Play");
            _settingsBtn = GetButton("Settings");
            _exitBtn = GetButton("Exit");
        }
        private void OnEnable()
        {
            _backToMenuListener.OnEnable(DisplayThis);
            _playBtn.clicked += () => _play.Raise();
            _settingsBtn.clicked += () => {_settings.Raise(); _root.style.display = DisplayStyle.None;};
            _exitBtn.clicked += () => _exit.Raise();
        }
        private void OnDisable()
        {
            _playBtn.clicked -= () => _play.Raise();
            _settingsBtn.clicked -= () => {_settings.Raise(); _root.style.display = DisplayStyle.None;};
            _exitBtn.clicked -= () => _exit.Raise();
        }
        private void DisplayThis()
        {
            _root.style.display = DisplayStyle.Flex;
        }
        private Button GetButton(string father) => _root.Q<VisualElement>(father).Q<Button>("btn");
    }
}
