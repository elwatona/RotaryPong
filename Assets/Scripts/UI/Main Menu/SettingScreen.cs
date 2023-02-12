using System;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;
using Watona.Events;
using RotaryPong.UI;

namespace RotaryPong
{
    [RequireComponent(typeof(UIDocument))]
    public class SettingScreen : MonoBehaviour
    {
        [SerializeField] CodedEventListener _settingsBtnListener;
        [SerializeField] GameEvent _doneButtonEvent;
    #region Scriptable Objects
        [Header("Volumen"), SerializeField] FloatVariable _sfxVolumen;
        [SerializeField] FloatVariable _musicVolumen;
        [Header("Column 1"), SerializeField] FloatVariable _matchDurationVariable;
        [SerializeField] BooleanVariable _godWallsVariable;
        [SerializeField] BooleanVariable _controlMapSpinVariable;
        [SerializeField] FloatVariable _mapSpinSpeedVariable;
        [SerializeField] BooleanVariable _playerBounceVariable;
        [Header("Column 2"), SerializeField] ShapeVariable _playerShapeVariable;
        [SerializeField] FloatVariable _playerSpeedVariable;
        [SerializeField] BooleanVariable _smoothRotationVariable;
        [Header("Column 3"), SerializeField] BooleanVariable _ballEffectVariable;
        [SerializeField] FloatVariable _effectTimerVariable;
        [SerializeField] BooleanVariable _colorByBounceVariable;
        [SerializeField] FloatVariable _durationSecondsVariable;
        [SerializeField] FloatVariable _durationBouncesVariable;

        private int _currentPreset;
    #endregion
        private VisualElement _root;
        private FloatVariable ColorDuration() => _colorByBounceVariable.Value ? _durationBouncesVariable : _durationSecondsVariable;
        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _root.style.display = DisplayStyle.None;
        }
        private void OnEnable()
        {
            _settingsBtnListener.OnEnable(DisplayThis); 
            GetBooleanValue("Color-By-Bounce").Q<Toggle>().RegisterValueChangedCallback((x) => { GetValueChanger("Color-Duration").FloatVariable = ColorDuration(); });

            GetButton("Done", "btn").clicked += () => { _doneButtonEvent?.Raise(); _root.style.display = DisplayStyle.None; };

            SetVariablesToUIElements();
        }
        private void OnDisable()
        {
            _settingsBtnListener.OnDisable();
            GetBooleanValue("Color-By-Bounce").Q<Toggle>().UnregisterValueChangedCallback((x) => { GetValueChanger("Color-Duration").FloatVariable = ColorDuration(); });

            GetButton("Done", "btn").clicked -= () => { _doneButtonEvent?.Raise(); _root.style.display = DisplayStyle.None; };
        }
        private void DisplayThis()
        {
            _root.style.display = DisplayStyle.Flex;
        }

        private void SetVariablesToUIElements()
        {
            GetShapeChanger("Player-Shape").ShapeVariable = _playerShapeVariable;

            GetValueChanger("Player-Speed").FloatVariable = _playerSpeedVariable;
            GetBooleanValue("Smooth-Rotation").BooleanVariable = _smoothRotationVariable;
            
            GetBooleanValue("Ball-Effect").BooleanVariable = _ballEffectVariable;
            GetValueChanger("Effect-Timer").FloatVariable = _effectTimerVariable;
            GetBooleanValue("Color-By-Bounce").BooleanVariable = _colorByBounceVariable;
            GetValueChanger("Color-Duration").FloatVariable = ColorDuration();

            GetBooleanValue("Control-Map-Spin").BooleanVariable = _controlMapSpinVariable;
            GetValueChanger("Map-Speed").FloatVariable = _mapSpinSpeedVariable;

            GetValueChanger("Match-Timer").FloatVariable = _matchDurationVariable;
            GetBooleanValue("Player-Bounce").BooleanVariable = _playerBounceVariable;
            GetBooleanValue("Front-Only-Goals").BooleanVariable = _godWallsVariable;

            GetVolumenChanger("SFX").FloatVariable = _sfxVolumen;
            GetVolumenChanger("Music").FloatVariable = _musicVolumen;

            // SetLabelText("Player-Shape", _playerShapeVariable);
        }
        private Button GetButton(string father, string name) => _root.Q<VisualElement>(father).Q<Button>(name);
        private ValueChanger GetValueChanger(string name) => _root.Q<ValueChanger>(name);
        private BooleanValue GetBooleanValue(string name) => _root.Q<BooleanValue>(name);
        private VolumenChanger GetVolumenChanger(string name) => _root.Q<VolumenChanger>(name);
        private ShapeChanger GetShapeChanger(string name) => _root.Q<ShapeChanger>(name);
    }
}
