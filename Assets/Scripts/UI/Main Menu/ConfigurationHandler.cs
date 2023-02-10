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
            _playerSpeed.SetValue(configuration.playerSpeed);
            _rotationAmount.SetValue(configuration.rotationAmount);
            _enableSmoothRotation.Value = configuration.enableSmoothRotation;
            _smoothRotationSpeed.SetValue(configuration.smoothRotationSpeed);
            _matchDuration.SetValue(configuration.matchDuration);
            _enablePlayerBounce.SetValue(configuration.playerBounce);
            _bouncePower.SetValue(configuration.bouncePower);
            _enableGodWalls.Value = configuration.enableGodWalls;
            _mapSpinSpeed.SetValue(configuration.mapSpinSpeed);
            _camShakeMagnitude.SetValue(configuration.camShakeMagnitude);
            _celebrationFireworks.SetValue(configuration.celebrationFireworks);
            _enableMapSpinControl.SetValue(configuration.enableMapSpinControl);
            _ballSpeed.SetValue(configuration.ballSpeed);
            _ballMinSpeed.SetValue(configuration.ballMinSpeed);
            _canChangeColorWithBounces.SetValue(configuration.ballCanChangeWithBounces);
            _ballColorDuration.SetValue(configuration.ballColorDuration);
            _ballColorBounces.SetValue(configuration.ballColorBounces);
            _canBallEffect.SetValue(configuration.ballEffect);
            _ballEffectTimer.SetValue(configuration.ballEffectTimer);
            _playerShape.SetValue(configuration.playerShape);
            _musicVolumen.SetValue(configuration.musicVolumen);
            _sfxVolumen.SetValue(configuration.sfxVolumen);

            Debug.Log("Configurations loaded");
        }
        private void SaveValues()
        {
            Configurations newConfigurations = new Configurations
            {
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
