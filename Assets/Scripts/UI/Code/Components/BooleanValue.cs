using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;
using Watona.Events;

namespace RotaryPong
{
    public class BooleanValue : VisualElement
    {
    #region USS names
        private const string styleSheet = "BooleanValue";
        private const string ussRootContainer = "boolean-value";
        private const string ussToggleContainer = "boolean-value__toggle-container";
        private const string ussToggleAR = "boolean-value__toggle-aspect-ratio";
    #endregion
        public new class UxmlFactory : UxmlFactory<BooleanValue, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits { }


        private Toggle _valueToggle;
        private Label _definitionLabel;
        private BooleanVariable _booleanVariable;
        private GameEvent _clicked;

        private bool _subscribed;

        public BooleanVariable BooleanVariable
        {
            get { return _booleanVariable; }
            set { CheckSub(value); }
        }
        public string Definition
        {
            get { return _definitionLabel.text; }
        }
        public bool Value
        {
            get { return _booleanVariable != null ? _booleanVariable.Value : _valueToggle.value; }
        }

        public BooleanValue() 
        {
			_clicked = Resources.Load<GameEvent>(string.Format("09_Events/ButtonClicked"));

            styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/{0}", styleSheet)));
            AddToClassList(ussRootContainer);

            VisualElement toggleContainer = new VisualElement() { name = "toggle-container" };
            toggleContainer.AddToClassList(ussToggleContainer);
            hierarchy.Add(toggleContainer);

            AspectRatioPanel toggleAR = new AspectRatioPanel() { name = "toggle-aspect-ratio", AspectRatioX = 1, AspectRatioY = 1, BalanceX = 0, BalanceY = 0 };
            // toggleAR.AddToClassList(ussToggleAR);
            toggleContainer.Add(toggleAR);

            _valueToggle = new Toggle() { name = "input" };
            toggleAR.Add(_valueToggle);
            _valueToggle.RegisterValueChangedCallback<bool>((x) => ChangeValue(x.newValue));
            _valueToggle.RegisterCallback<ClickEvent>((x) => _clicked?.Raise());

            _definitionLabel = new Label() { name = "definition" };

            hierarchy.Add(_definitionLabel);

            UpdateValues();
        }
        private void ChangeValue(bool value)
        {
            _booleanVariable?.SetValue(value);

            UpdateValues(value);
        }
        private void UpdateValues(bool value = false)
        {
            _definitionLabel.text = _booleanVariable != null ? _booleanVariable.name : "default-text";
            _valueToggle.value = _booleanVariable != null ? _booleanVariable.Value : value;
        }
        private void CheckSub(BooleanVariable value)
        {
            if(_subscribed) UnsubscribeVariable();
            _booleanVariable = value;
            SubscribeVariable();
            UpdateValues();
            this.SetTooltip(_booleanVariable.DeveloperDescription);
        }
        private void SubscribeVariable()
        {
            _booleanVariable.PropertyChanged += (x,y) => UpdateValues();
            _subscribed = true;
        }
        private void UnsubscribeVariable()
        {
            _booleanVariable.PropertyChanged -= (x,y) => UpdateValues();
            _subscribed = false;
        }
    }
}
