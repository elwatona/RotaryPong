using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;

namespace RotaryPong.UI
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
        private const string ussSquareContainer = "square-container";
        private const string ussSquare = "square";
    #endregion

        private Label definitionLabel;
        private Label valueLabel;

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

                ate.UpdateValues();
            }
        }

        public FloatVariable FloatVariable 
        {
            get { return floatVariable; } 
            set { floatVariable = value; UpdateValues(); }
        }
        private FloatVariable floatVariable;

        public string Definition 
        { 
            get { return definitionLabel.text; }
        }
        public float Value 
        { 
            get {return FloatVariable != null ? FloatVariable.Value : float.Parse(valueLabel.text);}
        }
        public float MinValue {get; private set;}
        public float MaxValue {get; private set;}
        public float ChangeValue {get; private set;}

        public ValueChanger()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_Uss/{0}", styleSheet)));
            AddToClassList(ussRootContainer);

            definitionLabel = new Label() { name = "definition" };
            definitionLabel.AddToClassList(ussDefinition);
            hierarchy.Add(definitionLabel);

            VisualElement container = new VisualElement() { name = "value-container" };
            container.AddToClassList(ussValueContainer);
            hierarchy.Add(container);

            VisualElement subtractContainer = new VisualElement() { name = "btn-container" };
            subtractContainer.AddToClassList(ussSquareContainer);
            container.Add(subtractContainer);

            AspectRatioPanel subAR = new AspectRatioPanel() { name = "sub-aspect-ratio", AspectRatioX = 1, AspectRatioY = 1 };
            subtractContainer.Add(subAR);

            Button subtract = new Button() { name = "subtract" };
            subtract.AddToClassList(ussSubtract);
            // subtract.AddToClassList(ussSquare);
            subtract.clicked += () => OnButtonClicked(false);
            subAR.Add(subtract);

            valueLabel = new Label() { name = "value" };
            valueLabel.AddToClassList(ussValue);
            container.Add(valueLabel);

            VisualElement addContainer = new VisualElement() { name = "btn-container" };
            addContainer.AddToClassList(ussSquareContainer);
            container.Add(addContainer);

            AspectRatioPanel addAR = new AspectRatioPanel() { name = "add-aspect-ratio", AspectRatioX = 1, AspectRatioY = 1 };
            addContainer.Add(addAR);

            Button add = new Button() { name = "add" };
            add.AddToClassList(ussAdd);
            // add.AddToClassList(ussSquare);
            add.clicked += () => OnButtonClicked(true);
            addAR.Add(add);

            UpdateValues();
        }

        private void OnButtonClicked(bool positive)
        {
            if(FloatVariable != null)
            {
                float changeValue = positive ? ChangeValue : -ChangeValue;
                float newValue = FloatVariable.Value + changeValue;
                float desiredValue = newValue > MaxValue ? MaxValue : newValue < MinValue ? MinValue : newValue;

                FloatVariable.SetValue(desiredValue);
                UpdateValues();
            }
            else
            {
                Debug.LogWarning(string.Format("{0} necesita un FloatReference", this.name));
            }
        }

        private void UpdateValues()
        {
            definitionLabel.text = FloatVariable != null ? FloatVariable.name : "default-text";
            valueLabel.text = FloatVariable != null ? FloatVariable.Value.ToString("0.##") : "000";
        }
    }
}
