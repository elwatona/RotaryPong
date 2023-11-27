using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;

namespace RotaryPong
{
    public class ValueChanger : VisualElement
    { 
    #region USS names
        private const string styleSheet = "ValueChanger";
        private const string ussRootContainer = "value-changer";
        private const string ussDefinition = "value-changer__definition";
        private const string ussSubtract = "value-changer__subtract";
        private const string ussAdd = "value-changer__add";
        private const string ussValue = "value-changer__value";
        private const string ussValueContainer = "value-changer__container";
        private const string ussBtnContainer = "value-changer__btn-container";
    #endregion

        public new class UxmlFactory : UxmlFactory<ValueChanger, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlFloatAttributeDescription m_MinValue =
                new UxmlFloatAttributeDescription { name = "min-value", defaultValue = 0 };
                
            UxmlFloatAttributeDescription m_MaxValue =
                new UxmlFloatAttributeDescription { name = "max-value", defaultValue = 10 };
                
            UxmlFloatAttributeDescription m_ChangeValue =
                new UxmlFloatAttributeDescription { name = "change-value", defaultValue = 1 };
                

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as ValueChanger;

                ate.MinValue = m_MinValue.GetValueFromBag(bag, cc);
                ate.MaxValue = m_MaxValue.GetValueFromBag(bag, cc);
                ate.ChangeValue = m_ChangeValue.GetValueFromBag(bag, cc);
            }
        }


        private Label _definitionLabel;
        private Label _valueLabel;
        private FloatVariable _floatVariable;

        private bool _subscribed;

        public FloatVariable FloatVariable 
        {
            get { return _floatVariable; } 
            set { SetFloatVariable(value); }
        }
        public string Definition 
        { 
            get { return _definitionLabel.text; }
        }
        public float Value 
        { 
            get {return _floatVariable != null ? _floatVariable.Value : float.Parse(_valueLabel.text);}
        }
        public float MinValue {get; set;}
        public float MaxValue {get; set;}
        public float ChangeValue {get; set;}

        public ValueChanger()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/{0}", styleSheet)));
            AddToClassList(ussRootContainer);

            _definitionLabel = new Label() { name = "definition" };
            _definitionLabel.AddToClassList(ussDefinition);
            hierarchy.Add(_definitionLabel);

            VisualElement container = new VisualElement() { name = "value-container" };
            container.AddToClassList(ussValueContainer);
            hierarchy.Add(container);

            AspectRatioButton subtractContainer = new AspectRatioButton(ButtonType.Back) { name = "btn-container", AspectRatioX = 1, AspectRatioY = 1, BalanceX = 0, BalanceY = 50};
            subtractContainer.clicked += () => OnButtonClicked(false);
            container.Add(subtractContainer);

            _valueLabel = new Label() { name = "value" };
            _valueLabel.AddToClassList(ussValue);
            container.Add(_valueLabel);

            AspectRatioButton addContainer = new AspectRatioButton(ButtonType.Next) { name = "btn-container", AspectRatioX = 1, AspectRatioY = 1, BalanceX = 100, BalanceY = 50};
            addContainer.clicked += () => OnButtonClicked(true);
            container.Add(addContainer);

            UpdateValues();
        }

        private void OnButtonClicked(bool positive)
        {
            if(_floatVariable == null)
            {
                Debug.LogWarning(string.Format("{0} necesita un FloatReference", this.name));
                return;
            }

            float changeValue = positive ? ChangeValue : -ChangeValue;
            float newValue = _floatVariable.Value + changeValue;
            float desiredValue = newValue > MaxValue ? MaxValue : newValue < MinValue ? MinValue : newValue;

            _floatVariable.SetValue(desiredValue);
            UpdateValues();
        }

        private void UpdateValues()
        {
            _definitionLabel.text = _floatVariable != null ? _floatVariable.name : "default-text";
            _valueLabel.text = _floatVariable != null ? _floatVariable.Value.ToString("0.##") : "000";
        }
        private void SetFloatVariable(FloatVariable value)
        {
            if(_subscribed) UnsubscribeVariable();
            _floatVariable = value;
            SubscribeVariable();
            UpdateValues();
            this.SetTooltip(_floatVariable.DeveloperDescription);
        }
        private void SubscribeVariable()
        {
            _floatVariable.PropertyChanged += (x,y) => UpdateValues();
            _subscribed = true;
        }
        private void UnsubscribeVariable()
        {
            _floatVariable.PropertyChanged -= (x,y) => UpdateValues();
            _subscribed = false;
        }
    }
}
