using UnityEngine;
using UnityEngine.UIElements;
using Watona.Events;
using Watona.Variables;
using RotaryPong.Events;
using RotaryPong.UI;

namespace RotaryPong
{
    [RequireComponent(typeof(UIDocument))]
    public class ScreenController : MonoBehaviour
    {
        Pause _pauseUI;
        [SerializeField] CodedEventListener _pause;
        [SerializeField] GameEvent _pauseEvent;
        [SerializeField] GameEvent _resetEvent;
        [SerializeField] GameEvent _backToMenuEvent;
        [SerializeField] GameEvent _exitGameEvent;
        [SerializeField] BooleanVariable _isPaused;
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
            _pause?.OnEnable(DisplayThis);

            GetAspectRatioButton("resume").clickable.clicked += () => _pauseEvent?.Raise();
            GetAspectRatioButton("reset").clickable.clicked += () => _resetEvent?.Raise();
            GetAspectRatioButton("menu").clickable.clicked += () => _backToMenuEvent?.Raise();
            GetAspectRatioButton("exit").clickable.clicked += () => _exitGameEvent?.Raise();
        }
        private void OnDisable()
        {
            _pause?.OnDisable();

            GetAspectRatioButton("resume").clickable.clicked -= () => _pauseEvent?.Raise();
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
            Time.timeScale = _isPaused.Value ? 0 : 1;
            UnityEngine.Cursor.visible = _isPaused.Value ? true : false;
        }
        private AspectRatioButton GetAspectRatioButton(string name) => _pauseUI.Q<AspectRatioButton>(name);
        private VolumenChanger GetVolumenChanger(string name) => _pauseUI.Q<VolumenChanger>(name);
    }
}
