using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watona.Utils;
using Watona.Variables;
using Watona.Events;

namespace RotaryPong
{
    public class ConfigurationHandler : MonoBehaviour
    {
        public CodedEventListener _backButtonListener;
        private string fileName = "configurations.json";
    #region SOVariables
        [SerializeField, Header("Scriptable Objects")] FloatVariable _musicVolumen;
        [SerializeField] FloatVariable _sfxVolumen;
        [SerializeField] PresetVariable _presetVariable;
        [SerializeField, Header("Player")] FloatVariable _playerSpeed;
        [SerializeField] FloatVariable _rotationAmount;
        [SerializeField] BooleanVariable _enableSmoothRotation;
        [SerializeField] FloatVariable _smoothRotationSpeed;
        [SerializeField] BooleanVariable _enablePlayerBounce;
        [SerializeField] FloatVariable _bouncePower;
        [SerializeField] ShapeVariable _playerShape;
        [SerializeField, Header("Match")] FloatVariable _matchDuration;
        [SerializeField] BooleanVariable _enableGodWalls;
        [SerializeField] FloatVariable _mapSpinSpeed;
        [SerializeField] FloatVariable _camShakeMagnitude;
        [SerializeField] FloatVariable _celebrationFireworks;
        [SerializeField] BooleanVariable _enableMapSpinControl;
        [SerializeField, Header("Ball")] FloatVariable _ballSpeed;
        [SerializeField] FloatVariable _ballMinSpeed;
        [SerializeField] BooleanVariable _canChangeColorWithBounces;
        [SerializeField] FloatVariable _ballColorDuration;
        [SerializeField] FloatVariable _ballColorBounces;
        [SerializeField] BooleanVariable _canBallEffect;
        [SerializeField] FloatVariable _ballEffectTimer;
    #endregion
        // Start is called before the first frame update
        private void Awake()
        {
            Configurations configuration = FileHandler.ReadFromJSON<Configurations>(fileName);

            if(configuration != default(Configurations))
                LoadValues(configuration);
        }
        private void OnEnable()
        {
            _backButtonListener?.OnEnable(SaveValues);
        }
        private void OnDisable()
        {
            _backButtonListener?.OnDisable();
        }
        private void LoadValues(Configurations configuration)
        {
            _presetVariable.SetValue(configuration.preset);
            _playerSpeed.SetValueWithoutNotify(configuration.playerSpeed);
            _rotationAmount.SetValueWithoutNotify(configuration.rotationAmount);
            _enableSmoothRotation.SetValueWithoutNotify(configuration.enableSmoothRotation);
            _smoothRotationSpeed.SetValueWithoutNotify(configuration.smoothRotationSpeed);
            _matchDuration.SetValueWithoutNotify(configuration.matchDuration);
            _enablePlayerBounce.SetValueWithoutNotify(configuration.playerBounce);
            _bouncePower.SetValueWithoutNotify(configuration.bouncePower);
            _enableGodWalls.SetValueWithoutNotify(configuration.enableGodWalls);
            _mapSpinSpeed.SetValueWithoutNotify(configuration.mapSpinSpeed);
            _camShakeMagnitude.SetValueWithoutNotify(configuration.camShakeMagnitude);
            _celebrationFireworks.SetValueWithoutNotify(configuration.celebrationFireworks);
            _enableMapSpinControl.SetValueWithoutNotify(configuration.enableMapSpinControl);
            _ballSpeed.SetValueWithoutNotify(configuration.ballSpeed);
            _ballMinSpeed.SetValueWithoutNotify(configuration.ballMinSpeed);
            _canChangeColorWithBounces.SetValueWithoutNotify(configuration.ballCanChangeWithBounces);
            _ballColorDuration.SetValueWithoutNotify(configuration.ballColorDuration);
            _ballColorBounces.SetValueWithoutNotify(configuration.ballColorBounces);
            _canBallEffect.SetValueWithoutNotify(configuration.ballEffect);
            _ballEffectTimer.SetValueWithoutNotify(configuration.ballEffectTimer);
            _playerShape.SetValueWithoutNotify(configuration.playerShape);
            _musicVolumen.SetValueWithoutNotify(configuration.musicVolumen);
            _sfxVolumen.SetValueWithoutNotify(configuration.sfxVolumen);

            Debug.Log("Configurations loaded");
        }
        private void SaveValues()
        {
            Configurations newConfigurations = new Configurations
            {
                preset = _presetVariable.Value,
                playerSpeed = _playerSpeed.Value,
                rotationAmount = _rotationAmount.Value,
                enableSmoothRotation = _enableSmoothRotation.Value,
                smoothRotationSpeed = _smoothRotationSpeed.Value,
                matchDuration = _matchDuration.Value,
                playerBounce = _enablePlayerBounce.Value,
                bouncePower = _bouncePower.Value,
                enableGodWalls = _enableGodWalls.Value,
                mapSpinSpeed = _mapSpinSpeed.Value,
                camShakeMagnitude = _camShakeMagnitude.Value,
                celebrationFireworks = _celebrationFireworks.Value,
                enableMapSpinControl = _enableMapSpinControl.Value,
                ballSpeed = _ballSpeed.Value,
                ballMinSpeed = _ballMinSpeed.Value,
                ballCanChangeWithBounces = _canChangeColorWithBounces.Value,
                ballColorDuration = _ballColorDuration.Value,
                ballColorBounces = _ballColorBounces.Value,
                ballEffect = _canBallEffect.Value,
                ballEffectTimer = _ballEffectTimer.Value,
                playerShape = _playerShape.Value,
                musicVolumen = _musicVolumen.Value,
                sfxVolumen = _sfxVolumen.Value
            };
            FileHandler.SaveToJSON<Configurations>(newConfigurations, fileName);
            Debug.Log("Configurations saved");
        }
    }
}
