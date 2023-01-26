using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;
using Watona.Events;

namespace RotaryPong
{
    public class MainMenuToolkit : MonoBehaviour
    {
    #region Scriptable Objects
        [Header("Column 1"), SerializeField] FloatVariable _matchDurationVariable;
        [SerializeField] BooleanVariable _godWallsVariable;
        [SerializeField] BooleanVariable _controlMapSpinVariable;
        [SerializeField] FloatVariable _mapSpinSpeedVariable;
        [SerializeField] BooleanVariable _playerBounceVariable;
        [SerializeField] FloatVariable _bouncePowerVariable;
        [Header("Column 2"), SerializeField] ShapeVariable _playerShapeVariable;
        [SerializeField] FloatVariable _playerSpeedVariable;
        [SerializeField] FloatVariable _rotationAmountVariable;
        [SerializeField] BooleanVariable _smoothRotationVariable;
        [SerializeField] FloatVariable _rotationSpeedVariable;
        [Header("Column 3"), SerializeField] BooleanVariable _ballEffectVariable;
        [SerializeField] FloatVariable _effectTimerVariable;
        [SerializeField] FloatVariable _ballSpeedVariable;
        [SerializeField] FloatVariable _ballMinSpeedVariable;
        [SerializeField] BooleanVariable _colorByBounceVariable;
        [SerializeField] FloatVariable _durationSecondsVariable;
        [SerializeField] IntVariable _durationBouncesVariable;
        [SerializeField] FloatVariable _camShakeMagVariable;
        [SerializeField] FloatVariable _endFireworksVariable;
    #endregion
        [SerializeField] GameEvent _startGame;
        [SerializeField] GameEvent _exitGame;
        private VisualElement _root;
        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
        }
        private void OnEnable()
        {
            SubUI();
        }
        private void OnDisable()
        {
            UnsubUI();
        }
        // Start is called before the first frame update
        void Start()
        {
            SetUIs();
        }
        private void SubUI()
        {
            GetButton("Play", "btn").clicked += () => _startGame?.Raise();
            GetButton("Exit", "btn").clicked += () => _exitGame?.Raise();

            GetButton("Player-Shape", "next").clickable.clicked += () => 
            {
                SetCurrentShape(1); 
                SetLabelValue("Player-Shape", "current-shape", _playerShapeVariable);
            };
            GetButton("Player-Shape", "prev").clickable.clicked += () => 
            {
                SetCurrentShape(-1);
                SetLabelValue("Player-Shape", "current-shape", _playerShapeVariable);
            };

            GetToggle("Enable-God-Walls").RegisterValueChangedCallback((x) => _godWallsVariable.SetValue(x.newValue));
            GetToggle("Control-Map-Spin").RegisterValueChangedCallback((x) => _controlMapSpinVariable.SetValue(x.newValue));
            GetToggle("Player-Bounce").RegisterValueChangedCallback((x) => 
            {
                _playerBounceVariable.SetValue(x.newValue);
                GetTextField("Bounce-Power").SetEnabled(_playerBounceVariable.Value);
            });
            GetToggle("Smooth-Rotation").RegisterValueChangedCallback((x) =>
            {
                _smoothRotationVariable.SetValue(x.newValue);
                GetTextField("Rotation-Speed").SetEnabled(_smoothRotationVariable.Value);
            });
            GetToggle("Ball-Effect").RegisterValueChangedCallback((x) =>
            {
                _ballEffectVariable.SetValue(x.newValue);
                GetTextField("Rotation-Speed").SetEnabled(_ballEffectVariable.Value);
            });
        }
        private void UnsubUI()
        {
            GetButton("Play", "btn").clicked -= () => _startGame?.Raise();
            GetButton("Exit", "btn").clicked -= () => _exitGame?.Raise();

            GetButton("Player-Shape", "next").clickable.clicked -= () => 
            {
                SetCurrentShape(1); 
                SetLabelValue("Player-Shape", "current-shape", _playerShapeVariable);
            };
            GetButton("Player-Shape", "prev").clickable.clicked -= () => 
            {
                SetCurrentShape(-1);
                SetLabelValue("Player-Shape", "current-shape", _playerShapeVariable);
            };
            
            GetToggle("Enable-God-Walls").UnregisterValueChangedCallback((x) => _godWallsVariable.SetValue(x.newValue));
            
        }
        private void SetCurrentShape(int value)
        {
            var shapes = Enum.GetValues(typeof(Shape));
            int currentShape = (int)_playerShapeVariable.Value;

            int desiredShape = currentShape + value > shapes.Length-1 ? 0 : currentShape + value < 0 ? shapes.Length-1 : currentShape + value;

            _playerShapeVariable.Value = (Shape)desiredShape;
        }
        private void SetUIs()
        {
            //Column 1
            SetTextFieldValue("Match-Duration", _matchDurationVariable);
            SetToggleValue("Enable-God-Walls", _godWallsVariable);
            SetToggleValue("Control-Map-Spin", _controlMapSpinVariable);
            SetTextFieldValue("Map-Spin-Speed", _mapSpinSpeedVariable);
            SetToggleValue("Player-Bounce", _playerBounceVariable);
            SetTextFieldValue("Bounce-Power", _bouncePowerVariable);

            //Column 2
            SetLabelValue("Player-Shape", "current-shape", _playerShapeVariable);
            SetTextFieldValue("Player-Speed", _playerSpeedVariable);
            SetTextFieldValue("Rotation-Amount", _rotationAmountVariable);
            SetToggleValue("Smooth-Rotation", _smoothRotationVariable);
            SetTextFieldValue("Rotation-Speed", _rotationSpeedVariable);

            //Column 3
            SetToggleValue("Ball-Effect", _ballEffectVariable);
            SetTextFieldValue("Effect-Timer", _effectTimerVariable);
            SetTextFieldValue("Ball-Speed", _ballSpeedVariable);
            SetTextFieldValue("Ball-Min-Speed", _ballMinSpeedVariable);
            SetToggleValue("Color-By-Bounce", _colorByBounceVariable);
            SetTextFieldValue("Duration-Seconds", _durationSecondsVariable);
            SetTextFieldValue("Duration-Bounces", _durationBouncesVariable);
            SetTextFieldValue("Cam-Shake-Mag", _camShakeMagVariable);
            SetTextFieldValue("End-Fireworks", _endFireworksVariable);

            CheckToggles();
        }
        private void CheckToggles()
        {
            GetTextField("Bounce-Power").SetEnabled(_playerBounceVariable.Value);
            GetTextField("Effect-Timer").SetEnabled(_ballEffectVariable.Value);
            GetTextField("Rotation-Amount").SetEnabled(!_smoothRotationVariable.Value);
            GetTextField("Rotation-Speed").SetEnabled(_smoothRotationVariable.Value);
            GetTextField("Duration-Seconds").SetEnabled(!_colorByBounceVariable.Value);
            GetTextField("Duration-Bounces").SetEnabled(_colorByBounceVariable.Value);
        }

        private Button GetButton(string fatherName, string buttonName)
        {
            return _root.Q<VisualElement>(fatherName).Q<Button>(buttonName);
        }
        private Button GetButton(string buttonName)
        {
            return _root.Q<Button>(buttonName);
        }
        private TextField GetTextField(string fatherName)
        {
            return _root.Q<VisualElement>(fatherName).Q<TextField>("input");
        }
        private Toggle GetToggle(string fatherName)
        {
            return _root.Q<VisualElement>(fatherName).Q<Toggle>("input");
        }
        private Label GetLabel(string fatherName, string labelName)
        {
            return _root.Q<VisualElement>(fatherName).Q<Label>(labelName);
        }
        private void SetTextFieldValue(string fatherName, FloatVariable variable)
        {
            GetTextField(fatherName).value = variable.Value.ToString();
        }
        private void SetTextFieldValue(string fatherName, IntVariable variable)
        {
            GetTextField(fatherName).value = variable.Value.ToString();
        }
        private void SetToggleValue(string fatherName, BooleanVariable variable)
        {
            GetToggle(fatherName).value = variable.Value;
        }
        private void SetLabelValue(string fatherName, string labelName, ShapeVariable variable)
        {
            GetLabel(fatherName, labelName).text = variable.Value.ToString();
        }
    }
}
