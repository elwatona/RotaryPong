using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;

namespace RotaryPong.UI
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

        public BooleanVariable BooleanVariable
        {
            get { return _booleanVariable; }
            set { _booleanVariable = value; UpdateValues(); }
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
    }
}
