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
        [SerializeField, Header("Match")] FloatVariable _matchDuration;
        [SerializeField] BooleanVariable _enableGodWalls;
        [SerializeField] FloatVariable _mapSpinSpeed;
        [SerializeField] FloatVariable _camShakeMagnitude;
        [SerializeField] FloatVariable _celebrationFireworks;
        [SerializeField, Header("Ball")] FloatVariable _ballSpeed;
        [SerializeField] FloatVariable _ballDeceleration;
        [SerializeField] FloatVariable _ballMinSpeed;
        [SerializeField] FloatVariable _ballColorDuration;
    #endregion
    #region UI Inputs
        [SerializeField, Header("UI Inputs"), Header("Player")] InputField _playerSpeedInputField;
        [SerializeField] Text _playerSpeedText;
        [SerializeField, Space] InputField _rotationAmountInputField;
        [SerializeField] Text _rotationAmountText;
        [SerializeField, Space] Toggle _enableSmoothRotationToggle;
        [SerializeField, Space] InputField _smoothRotationSpeedInputField;
        [SerializeField] Text _smoothRotationSpeedText;
        [SerializeField, Header("Match")] InputField _matchDurationInputField;
        [SerializeField] Text _matchDurationText;
        [SerializeField, Space] Toggle _enableGodWallsToggle;
        [SerializeField, Space] InputField _mapSpinSpeedInputField;
        [SerializeField] Text _mapSpinSpeedText;
        [SerializeField, Space] InputField _camShakeMagnitudeInputField;
        [SerializeField] Text _camShakeMagnitudeText;
        [SerializeField, Space] InputField _celebrationFireworksInputField;
        [SerializeField] Text _celebrationFireworksText;
        [SerializeField, Header("Ball")] InputField _ballSpeedInputField;
        [SerializeField] Text _ballSpeedText;
        [SerializeField, Space] InputField _ballDecelerationInputField;
        [SerializeField] Text _ballDecelerationText;
        [SerializeField, Space] InputField _ballMinSpeedInputField;
        [SerializeField] Text _ballMinSpeedText;
        [SerializeField, Space] InputField _ballColorDurationInputField;
        [SerializeField] Text _ballColorDurationText;
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
            _ballSpeedText.text = _ballSpeed.Value.ToString();
            _ballDecelerationText.text = _ballDeceleration.Value.ToString();
            _ballMinSpeedText.text = _ballMinSpeed.Value.ToString();
            _ballColorDurationText.text = _ballColorDuration.Value.ToString();
        }
        private void LoadValues(Configurations configuration)
        {
            _playerSpeed.SetValue(configuration.playerSpeed);
            _rotationAmount.SetValue(configuration.rotationAmount);
            _enableSmoothRotation.Value = configuration.enableSmoothRotation;
            _smoothRotationSpeed.SetValue(configuration.smoothRotationSpeed);
            _matchDuration.SetValue(configuration.matchDuration);
            _enableGodWalls.Value = configuration.enableGodWalls;
            _mapSpinSpeed.SetValue(configuration.mapSpinSpeed);
            _camShakeMagnitude.SetValue(configuration.camShakeMagnitude);
            _celebrationFireworks.SetValue(configuration.celebrationFireworks);
            _ballSpeed.SetValue(configuration.ballSpeed);
            _ballDeceleration.SetValue(configuration.ballDeceleration);
            _ballMinSpeed.SetValue(configuration.ballMinSpeed);
            _ballColorDuration.SetValue(configuration.ballColorDuration);

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
                enableGodWalls = _enableGodWalls.Value,
                mapSpinSpeed = _mapSpinSpeed.Value,
                camShakeMagnitude = _camShakeMagnitude.Value,
                celebrationFireworks = _celebrationFireworks.Value,
                ballSpeed = _ballSpeed.Value,
                ballDeceleration = _ballDeceleration.Value,
                ballMinSpeed = _ballMinSpeed.Value,
                ballColorDuration = _ballColorDuration.Value
            };
            FileHandler.SaveToJSON<Configurations>(newConfigurations, fileName);
        }
        public void ChangeValues()
        {
            //Player Panel
            CheckInputField(_playerSpeedInputField, _playerSpeed);
            CheckInputField(_rotationAmountInputField, _rotationAmount);
            CheckToggle(_enableSmoothRotationToggle, _enableSmoothRotation);
            CheckInputField(_smoothRotationSpeedInputField, _smoothRotationSpeed);

            //Match Panel
            CheckInputField(_matchDurationInputField, _matchDuration);
            CheckToggle(_enableGodWallsToggle, _enableGodWalls);
            CheckInputField(_mapSpinSpeedInputField, _mapSpinSpeed);
            CheckInputField(_camShakeMagnitudeInputField, _camShakeMagnitude);
            CheckInputField(_celebrationFireworksInputField, _celebrationFireworks);

            //Ball Panel
            CheckInputField(_ballSpeedInputField, _ballSpeed);
            CheckInputField(_ballDecelerationInputField, _ballDeceleration);
            CheckInputField(_ballMinSpeedInputField, _ballMinSpeed);
            CheckInputField(_ballColorDurationInputField, _ballColorDuration);

            SaveValues();
        }
        private void CheckInputField(InputField inputField, FloatVariable floatVariable)
        {
            float value = inputField.textComponent.text == "" ? floatVariable.Value : float.Parse(inputField.textComponent.text);
            floatVariable.SetValue(value);
        }
        private void CheckToggle(Toggle toggle, BooleanVariable booleanVariable)
        {
            booleanVariable.Value = toggle.isOn;
        }
    }
}
