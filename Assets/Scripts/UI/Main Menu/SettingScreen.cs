using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;
using Watona.Events;
using Watona.Utils;
using RotaryPong.Events;

namespace RotaryPong.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class SettingScreen : MonoBehaviour
    {
        [SerializeField] CodedEventListener _settingsBtnListener;
        [SerializeField] BooleanEvent _backEvent;
    #region Scriptable Objects
        [SerializeField] PresetVariable _preset;
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
    #endregion
        private VisualElement _root;
        private Settings _settings;
        private FloatVariable _colorDuration => _colorByBounceVariable.Value ? _durationBouncesVariable : _durationSecondsVariable;
        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _settings = _root.Q<Settings>();
            _root.SetVisibility(false);
        }
        private void OnEnable()
        {
            _settingsBtnListener.OnEnable(() => _root.SetVisibility(true));

            _settings.ColorByBounce.Q<Toggle>().RegisterValueChangedCallback((x) => { _settings.ColorDuration.FloatVariable = _colorDuration; });

            _settings.Save.clicked += () => { _backEvent?.Raise(true); _root.SetVisibility(false); };
            _settings.Back.clicked += () => { _backEvent?.Raise(false); _root.SetVisibility(false); };
            _preset.PropertyChanged += (x,y) => OnPresetChanged();

            SetVariablesToUIElements();
        }
        private void OnDisable()
        {
            _settingsBtnListener.OnDisable();

            _settings.ColorByBounce.Q<Toggle>().UnregisterValueChangedCallback((x) => { _settings.ColorDuration.FloatVariable = _colorDuration; });

            _settings.Save.clicked -= () => { _backEvent?.Raise(true); _root.SetVisibility(false); };
            _settings.Back.clicked -= () => { _backEvent?.Raise(false); _root.SetVisibility(false); };
            _preset.PropertyChanged -= (x,y) => OnPresetChanged();
        }

        private void SetVariablesToUIElements()
        {
            _settings.ShapeChanger.ShapeVariable = _playerShapeVariable;

            _settings.PlayerSpeed.FloatVariable = _playerSpeedVariable;
            _settings.SmoothRotation.BooleanVariable = _smoothRotationVariable;

            _settings.BallEffect.BooleanVariable = _ballEffectVariable;
            _settings.EffectTimer.FloatVariable = _effectTimerVariable;
            _settings.ColorByBounce.BooleanVariable = _colorByBounceVariable;
            _settings.ColorDuration.FloatVariable = _colorDuration;

            _settings.ControlMapSpin.BooleanVariable = _controlMapSpinVariable;
            _settings.MapSpeed.FloatVariable = _mapSpinSpeedVariable;

            _settings.MatchTimer.FloatVariable = _matchDurationVariable;
            _settings.PlayerBounce.BooleanVariable = _playerBounceVariable;
            _settings.FrontOnlyGoals.BooleanVariable = _godWallsVariable;

            _settings.Music.FloatVariable = _musicVolumen;
            _settings.SFX.FloatVariable = _sfxVolumen;
        }
        private void OnPresetChanged()
        {
            switch(_preset.Value)
            {
                case Preset.Rotary:
                    _playerShapeVariable.SetValue(Shape.Bar);
                    _playerSpeedVariable.SetValue(10);
                    _smoothRotationVariable.SetValue(false);
                    _ballEffectVariable.SetValue(false);
                    _colorByBounceVariable.SetValue(false);
                    _colorDuration.SetValue(2);
                    _controlMapSpinVariable.SetValue(false);
                    _mapSpinSpeedVariable.SetValue(95);
                    _matchDurationVariable.SetValue(90);
                    _playerBounceVariable.SetValue(false);
                    _godWallsVariable.SetValue(false);
                    break;
                    
                case Preset.Classic:
                    _playerShapeVariable.SetValue(Shape.Bar);
                    _playerSpeedVariable.SetValue(10);
                    _smoothRotationVariable.SetValue(false);
                    _ballEffectVariable.SetValue(false);
                    _colorByBounceVariable.SetValue(false);
                    _colorDuration.SetValue(2);
                    _controlMapSpinVariable.SetValue(false);
                    _mapSpinSpeedVariable.SetValue(90);
                    _matchDurationVariable.SetValue(180);
                    _playerBounceVariable.SetValue(false);
                    _godWallsVariable.SetValue(true);
                    break;

                case Preset.CShape:
                    _playerShapeVariable.SetValue(Shape.C);
                    _playerSpeedVariable.SetValue(12);
                    _smoothRotationVariable.SetValue(true);
                    _ballEffectVariable.SetValue(false);
                    _colorByBounceVariable.SetValue(true);
                    _colorDuration.SetValue(4);
                    _controlMapSpinVariable.SetValue(true);
                    _mapSpinSpeedVariable.SetValue(100);
                    _matchDurationVariable.SetValue(120);
                    _playerBounceVariable.SetValue(false);
                    _godWallsVariable.SetValue(false);
                    break;
            }
        }
    }
}
