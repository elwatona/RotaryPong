using UnityEngine;
using UnityEngine.UIElements;
using Watona.Events;
using Watona.Variables;
using RotaryPong.UI.Component;

namespace RotaryPong.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class ScreenController : MonoBehaviour
    {
        Pause _pauseUI;
        [SerializeField, Header("Listener & Events")] CodedEventListener _pauseListener;
        [SerializeField, Space] GameEvent _resumeEvent;
        [SerializeField] GameEvent _resetEvent;
        [SerializeField] GameEvent _backToMenuEvent;
        [SerializeField] GameEvent _exitGameEvent;
        [SerializeField, Header("Variables")] BooleanVariable _isPaused;
        [SerializeField] FloatVariable _musicVolumen;
        [SerializeField] FloatVariable _sfxVolumen;
        [SerializeField] GameStateVariable _currentGameState;
        private void Awake()
        {
            _pauseUI = new();
            GetComponent<UIDocument>().rootVisualElement.Add(_pauseUI);

            GetVolumenChanger("SFX").FloatVariable = _sfxVolumen;
            GetVolumenChanger("Music").FloatVariable = _musicVolumen;
        }
        private void OnEnable()
        {
            _pauseListener?.OnEnable(DisplayThis);

            GetAspectRatioButton("resume").clickable.clicked += () => _resumeEvent?.Raise();
            GetAspectRatioButton("reset").clickable.clicked += () => _resetEvent?.Raise();
            GetAspectRatioButton("menu").clickable.clicked += () => _backToMenuEvent?.Raise();
            GetAspectRatioButton("exit").clickable.clicked += () => _exitGameEvent?.Raise();
        }
        private void OnDisable()
        {
            _pauseListener?.OnDisable();

            GetAspectRatioButton("resume").clickable.clicked -= () => _resumeEvent?.Raise();
            GetAspectRatioButton("reset").clickable.clicked -= () => _resetEvent?.Raise();
            GetAspectRatioButton("menu").clickable.clicked -= () => _backToMenuEvent?.Raise();
            GetAspectRatioButton("exit").clickable.clicked -= () => _exitGameEvent?.Raise();
        }
        private void Start()
        {
            DisplayThis();
        }

        private void DisplayThis()
        {
            _pauseUI.style.display = _isPaused.Value ? DisplayStyle.Flex : DisplayStyle.None;
        }
        private AspectRatioButton GetAspectRatioButton(string name) => _pauseUI.Q<AspectRatioButton>(name);
        private VolumenChanger GetVolumenChanger(string name) => _pauseUI.Q<VolumenChanger>(name);
    }
}
