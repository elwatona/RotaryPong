using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Watona.Utils;
using Watona.Variables;

namespace RotaryPong
{
    public class ConfigurationCanvas : MonoBehaviour
    {
        private string fileName = "configurations.json";
    #region SOVariables
        [SerializeField, Header("Scriptable Objects"),Header("Player")] FloatVariable _playerSpeed;
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
        [SerializeField] IntVariable _ballColorBounces;
        [SerializeField] BooleanVariable _canBallEffect;
        [SerializeField] FloatVariable _ballEffectTimer;
    #endregion
    #region UI Inputs
        [SerializeField, Header("UI Inputs"), Header("Player")] InputField _playerSpeedInputField;
        [SerializeField] Text _playerSpeedText;
        [SerializeField, Space] InputField _rotationAmountInputField;
        [SerializeField] Text _rotationAmountText;
        [SerializeField, Space] Toggle _enableSmoothRotationToggle;
        [SerializeField, Space] InputField _smoothRotationSpeedInputField;
        [SerializeField] Text _smoothRotationSpeedText;
        [SerializeField, Space] Toggle _enablePlayerBounceToggle;
        [SerializeField] InputField _bouncePowerInputField;
        [SerializeField] Text _bouncePowerText;
        [SerializeField] Toggle _playerShapeToggle;
        [SerializeField, Header("Match")] InputField _matchDurationInputField;
        [SerializeField] Text _matchDurationText;
        [SerializeField, Space] Toggle _enableGodWallsToggle;
        [SerializeField, Space] InputField _mapSpinSpeedInputField;
        [SerializeField] Text _mapSpinSpeedText;
        [SerializeField, Space] InputField _camShakeMagnitudeInputField;
        [SerializeField] Text _camShakeMagnitudeText;
        [SerializeField, Space] InputField _celebrationFireworksInputField;
        [SerializeField] Text _celebrationFireworksText;
        [SerializeField, Space] Toggle _enableMapSpinControlToggle;
        [SerializeField, Header("Ball")] InputField _ballSpeedInputField;
        [SerializeField] Text _ballSpeedText;
        [SerializeField, Space] InputField _ballMinSpeedInputField;
        [SerializeField] Text _ballMinSpeedText;
        [SerializeField, Space] Toggle _canChangeColorWithBouncesToggle;
        [SerializeField] InputField _ballColorDurationInputField;
        [SerializeField] Text _ballColorDurationText;
        [SerializeField] InputField _ballColorBouncesInputField;
        [SerializeField] Text _ballColorBouncesText;
        [SerializeField, Space] Toggle _canBallEffectToggle;
        [SerializeField, Space] InputField _ballEffectTimerInputField;
        [SerializeField] Text _ballEffectTimerText;
    #endregion
        // Start is called before the first frame update
        private void Awake()
        {
            Configurations configuration = FileHandler.ReadFromJSON<Configurations>(fileName);

            if(configuration != default(Configurations))
                LoadValues(configuration);
        }
        private void Start()
        {
            SetUIs();
        }
        private void SetUIs()
        {
            _playerSpeedText.text = _playerSpeed.Value.ToString();
            _rotationAmountText.text = _rotationAmount.Value.ToString();
            _enableSmoothRotationToggle.isOn = _enableSmoothRotation.Value;
            _smoothRotationSpeedText.text = _smoothRotationSpeed.Value.ToString();
            _matchDurationText.text = _matchDuration.Value.ToString();
            _enableGodWallsToggle.isOn = _enableGodWalls.Value;
            _mapSpinSpeedText.text = _mapSpinSpeed.Value.ToString();
            _camShakeMagnitudeText.text = _camShakeMagnitude.Value.ToString();
            _celebrationFireworksText.text = _celebrationFireworks.Value.ToString();
            _enableMapSpinControlToggle.isOn = _enableMapSpinControl.Value;
            _ballSpeedText.text = _ballSpeed.Value.ToString();
            _ballMinSpeedText.text = _ballMinSpeed.Value.ToString();
            _canChangeColorWithBouncesToggle.isOn = _canChangeColorWithBounces.Value;
            _ballColorDurationText.text = _ballColorDuration.Value.ToString();
            _ballColorBouncesText.text = _ballColorBounces.Value.ToString();
            _canBallEffectToggle.isOn = _canBallEffect.Value;
            _ballEffectTimerText.text = _ballEffectTimer.Value.ToString();
            _enablePlayerBounceToggle.isOn = _enablePlayerBounce.Value;
            _bouncePowerText.text = _bouncePower.Value.ToString();
            _playerShapeToggle.isOn = _playerShape.Value != Shape.Bar ? true : false;
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
                playerShape = _playerShape.Value
            };
            FileHandler.SaveToJSON<Configurations>(newConfigurations, fileName);
            Debug.Log("Configurations saved");
        }
        public void ChangeValues()
        {
            //Player Panel
            CheckInputField(_playerSpeedInputField, _playerSpeed);
            CheckInputField(_rotationAmountInputField, _rotationAmount);
            CheckToggle(_enableSmoothRotationToggle, _enableSmoothRotation);
            CheckInputField(_smoothRotationSpeedInputField, _smoothRotationSpeed);
            CheckToggle(_enablePlayerBounceToggle, _enablePlayerBounce);
            CheckInputField(_bouncePowerInputField, _bouncePower);
            _playerShape.SetValue(_playerShapeToggle.isOn ? Shape.C : Shape.Bar);

            //Match Panel
            CheckInputField(_matchDurationInputField, _matchDuration);
            CheckToggle(_enableGodWallsToggle, _enableGodWalls);
            CheckInputField(_mapSpinSpeedInputField, _mapSpinSpeed);
            CheckInputField(_camShakeMagnitudeInputField, _camShakeMagnitude);
            CheckToggle(_enableMapSpinControlToggle, _enableMapSpinControl);
            CheckInputField(_celebrationFireworksInputField, _celebrationFireworks);

            //Ball Panel
            CheckInputField(_ballSpeedInputField, _ballSpeed);
            CheckInputField(_ballMinSpeedInputField, _ballMinSpeed);
            CheckToggle(_canChangeColorWithBouncesToggle, _canChangeColorWithBounces);
            CheckInputField(_ballColorDurationInputField, _ballColorDuration);
            CheckInputField(_ballColorBouncesInputField, _ballColorBounces);
            CheckToggle(_canBallEffectToggle, _canBallEffect);
            CheckInputField(_ballEffectTimerInputField, _ballEffectTimer);
            

            SaveValues();
        }
        private void CheckInputField(InputField inputField, FloatVariable floatVariable)
        {
            float value = inputField.textComponent.text == "" ? floatVariable.Value : float.Parse(inputField.textComponent.text);
            floatVariable.SetValue(value);
        }
        private void CheckInputField(InputField inputField, IntVariable intVariable)
        {
            int value = inputField.textComponent.text == "" ? intVariable.Value : int.Parse(inputField.textComponent.text);
            intVariable.SetValue(value);
        }
        private void CheckToggle(Toggle toggle, BooleanVariable booleanVariable)
        {
            booleanVariable.Value = toggle.isOn;
        }
    }
}
