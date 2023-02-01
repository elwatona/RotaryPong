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
        [SerializeField] FloatVariable _durationBouncesVariable;
        [SerializeField] FloatVariable _camShakeMagVariable;
        [SerializeField] FloatVariable _endFireworksVariable;

        private int _currentPreset;
    #endregion
        [SerializeField] GameEvent _done;
        private VisualElement _root;
        private FloatVariable ColorDuration() => _colorByBounceVariable.Value ? _durationBouncesVariable : _durationSecondsVariable;
        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
        }
        private void OnEnable()
        {
            SubscribeShapeButtons();

            SubscribeValueChanger("Color-Duration", 1, 2, 8);
            SubscribeValueChanger("Player-Speed", 5, 5, 20, _playerSpeedVariable);
            SubscribeValueChanger("Effect-Timer", 0.05f, 0.25f, 0.75f, _effectTimerVariable);
            SubscribeValueChanger("Map-Speed", 5, 80, 110, _mapSpinSpeedVariable);
            SubscribeValueChanger("Match-Timer", 5, 90, 180, _matchDurationVariable);

            SubscribeToggle("Smooth-Rotation", _smoothRotationVariable);
            SubscribeToggle("Color-By-Bounce", _colorByBounceVariable);
            SubscribeToggle("Ball-Effect", _ballEffectVariable);
            SubscribeToggle("Control-Map-Spin", _controlMapSpinVariable);
            SubscribeToggle("Front-Only-Goals", _godWallsVariable);
            SubscribeToggle("Player-Bounce", _playerBounceVariable);
        }
        private void OnDisable()
        {
            UnsubscribeShapeButtons();

            UnsubscribeValueChanger("Color-Duration", 1, 2, 8);
            UnsubscribeValueChanger("Player-Speed", 5, 5, 20, _playerSpeedVariable);
            UnsubscribeValueChanger("Effect-Timer", 0.05f, 0.25f, 0.75f, _effectTimerVariable);
            UnsubscribeValueChanger("Map-Speed", 5, 80, 110, _mapSpinSpeedVariable);
            UnsubscribeValueChanger("Match-Timer", 5, 90, 180, _matchDurationVariable);

            UnsubscribeToggle("Smooth-Rotation", _smoothRotationVariable);
            UnsubscribeToggle("Color-By-Bounce", _colorByBounceVariable);
            UnsubscribeToggle("Ball-Effect", _ballEffectVariable);
            UnsubscribeToggle("Control-Map-Spin", _controlMapSpinVariable);
            UnsubscribeToggle("Front-Only-Goals", _godWallsVariable);
            UnsubscribeToggle("Player-Bounce", _playerBounceVariable);
        }
        private void SubscribeShapeButtons()
        {
            GetButton("Player-Shape", "prev").clickable.clicked += () => ChangeShape(-1);
            GetButton("Player-Shape", "next").clickable.clicked += () => ChangeShape(1);
        }
        private void UnsubscribeShapeButtons()
        {
            GetButton("Player-Shape", "prev").clickable.clicked -= () => ChangeShape(-1);
            GetButton("Player-Shape", "next").clickable.clicked -= () => ChangeShape(1);
        }
        private void SubscribeValueChanger(string fatherName, float input, float minValue, float maxValue, FloatVariable variable = null)
        {
            if(variable != null)
            {
                GetButton(fatherName, "subtract").clickable.clicked += () => { ChangeValue(-input, minValue, maxValue, variable); };
                GetButton(fatherName, "add").clickable.clicked += () => { ChangeValue(input, minValue, maxValue, variable); };
                return;
            }
            GetButton(fatherName, "subtract").clickable.clicked += () => { ChangeValue(-input, minValue, maxValue); };
            GetButton(fatherName, "add").clickable.clicked += () => { ChangeValue(input, minValue, maxValue); };
        }
        private void UnsubscribeValueChanger(string fatherName, float input, float minValue, float maxValue, FloatVariable variable = null)
        {
            if(variable != null)
            {
                GetButton(fatherName, "subtract").clickable.clicked -= () => { ChangeValue(-input, minValue, maxValue, variable); };
                GetButton(fatherName, "add").clickable.clicked -= () => { ChangeValue(input, minValue, maxValue, variable); };
                return;
            }
            GetButton(fatherName, "subtract").clickable.clicked -= () => { ChangeValue(-input, minValue, maxValue); };
            GetButton(fatherName, "add").clickable.clicked -= () => { ChangeValue(input, minValue, maxValue); };
        }
        private void SubscribeToggle(string fatherName, BooleanVariable variable)
        {
            GetToggle(fatherName).RegisterValueChangedCallback((evt) => {variable.SetValue(evt.newValue); UpdateValues();});
        }
        private void UnsubscribeToggle(string fatherName, BooleanVariable variable)
        {
            GetToggle(fatherName).UnregisterValueChangedCallback((evt) => {variable.SetValue(evt.newValue); UpdateValues();});
        }
        // Start is called before the first frame update
        void Start()
        {
            UpdateValues();
        }
        private void UpdateValues()
        {
            string colorByBounce = _colorByBounceVariable.Value ? "Bounces" : "Seconds";
            //Column 1
            SetLabelText("Player-Shape", _playerShapeVariable);
            SetLabelText("Player-Speed", _playerSpeedVariable);
            SetToggleValue("Smooth-Rotation", _smoothRotationVariable);

            //Column 2
            SetToggleValue("Color-By-Bounce", _colorByBounceVariable);
            SetLabelText("Color-Duration", ColorDuration());
            SetLabelText("Color-Duration", "definition", colorByBounce);
            SetToggleValue("Ball-Effect", _ballEffectVariable);
            SetLabelText("Effect-Timer", _effectTimerVariable);

            //Column 3
            SetLabelText("Map-Speed", _mapSpinSpeedVariable);
            SetToggleValue("Control-Map-Spin", _controlMapSpinVariable);
            SetLabelText("Match-Timer", _matchDurationVariable);
            SetToggleValue("Front-Only-Goals", _godWallsVariable);
            SetToggleValue("Player-Bounce", _playerBounceVariable);
        }

        private void ChangeShape(int value)
        {
            var shapes = Enum.GetValues(typeof(Shape));
            int currentShape = (int)_playerShapeVariable.Value;
            int newShape = currentShape + value;
            int desiredShape = currentShape + value > shapes.Length-1 ? 0 : currentShape + value < 0 ? shapes.Length-1 : currentShape + value;

            _playerShapeVariable.Value = (Shape)desiredShape;

            UpdateValues();
        }
        private void ChangeValue(float input, float minValue, float maxValue, FloatVariable variable = null)
        {
            FloatVariable current = ColorDuration();

            float currentValue = variable != null ? variable.Value : current.Value;
            float newValue = currentValue + input;
            float desiredValue = newValue > maxValue ? maxValue : newValue < minValue ? minValue : newValue;

            // print(string.Format("Current Value = {0}", currentValue));
            // print(string.Format("New Value = {0}", newValue));
            // print(string.Format("Desired Value = {0}", desiredValue));

            if(variable != null)
                variable.SetValue(desiredValue);
            else
                current.SetValue(desiredValue);

            UpdateValues();
        }

        private void SetToggleValue(string fatherName, BooleanVariable variable)
        {
            GetToggle(fatherName).value = variable.Value;
        }
        private void SetLabelText(string fatherName, FloatVariable variable, string labelName = null)
        {
            string value = variable.Value.ToString("0.##");
            if(labelName != null)
            {
                GetChildLabel(fatherName, labelName).text = value;
                return;
            }
            GetChildLabel(fatherName).text = value;
        }
        private void SetLabelText(string fatherName, string labelName, string value)
        {
            GetChildLabel(fatherName, labelName).text = value;
        }
        private void SetLabelText(string fatherName, ShapeVariable variable, string labelName = "current-shape")
        {
            GetChildLabel(fatherName, labelName).text = variable.Value.ToString();
        }

        private Button GetButton(string fatherName, string buttonName) => _root.Q<VisualElement>(fatherName).Q<Button>(buttonName);
        private Button GetButton(string buttonName) => _root.Q<Button>(buttonName);
        private Toggle GetToggle(string fatherName) => _root.Q<VisualElement>(fatherName).Q<Toggle>("input");
        private Label GetChildLabel(string fatherName, string labelName = "value") => _root.Q<VisualElement>(fatherName).Q<Label>(labelName);
        private Label GetLabel(string labelName) => _root.Q<Label>(labelName);
    }
}
