using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;

namespace RotaryPong.UI.Component
{
    public enum Volumen
    {
        Music,
        SFX
    }
    public class VolumenChanger : VisualElement
    {
    #region USS names
        private const string styleSheet = "VolumenChanger";
        private const string ussRootContainer = "volumen-changer";
        private const string ussImageContainer = "volumen-changer__image-container";
        private const string ussImageAR = "volumen-changer__image-aspect-ratio";
        private const string ussImageIcon = "volumen-changer__image";
        private const string ussSliderContainer = "volumen-changer__slider-container";
        private const string ussSliderAR = "volumen-changer__slider-aspect-ratio";
        private const string ussSliderFiller = "volumen-changer__slider-filler";
        private const string ussValueContainer = "volumen-changer__value-container";
    #endregion
        public new class UxmlFactory : UxmlFactory<VolumenChanger, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits 
        {
            UxmlEnumAttributeDescription<Volumen> m_VolumenController =
                new UxmlEnumAttributeDescription<Volumen> { name = "volumen-controller", defaultValue = Volumen.Music };
        

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as VolumenChanger;

                ate.VolumenController = m_VolumenController.GetValueFromBag(bag, cc);

                ate.UpdateValues();
            }
        }


        private Image _icon;
        private Label _value;
        private VisualElement _filler;
        private FloatVariable _floatVariable;
        private Slider _slider;
        private Volumen _volumenController;

        public Volumen VolumenController
        {
            get { return _volumenController; }
            set { _volumenController = value; UpdateValues(); }
        }
        public FloatVariable FloatVariable
        {
            get { return _floatVariable; }
            set { _floatVariable = value; UpdateValues(); }
        }
        public float Value
        {
            get { return _floatVariable != null ? _floatVariable.Value : float.Parse(_value.text); }
        }

        public VolumenChanger() 
        {
            styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/{0}", styleSheet)));
            AddToClassList(ussRootContainer);

            VisualElement imageContainer = new VisualElement() { name = "image-container" };
            imageContainer.AddToClassList(ussImageContainer);
            hierarchy.Add(imageContainer);

            AspectRatioPanel imageAR = new AspectRatioPanel() { name = "image-aspect-ratio", AspectRatioX = 1, AspectRatioY = 1 };
            imageAR.AddToClassList(ussImageAR);
            imageContainer.Add(imageAR);

            _icon = new Image() { name = "image" };
            _icon.AddToClassList(ussImageIcon);
            imageAR.Add(_icon);

            VisualElement sliderContainer = new VisualElement() { name = "slider-container" };
            sliderContainer.AddToClassList(ussSliderContainer);
            hierarchy.Add(sliderContainer);

            AspectRatioPanel sliderAR = new AspectRatioPanel() { name = "slider-aspect-ratio", AspectRatioX = 1, AspectRatioY = 5 };
            sliderAR.AddToClassList(ussSliderAR);
            sliderContainer.Add(sliderAR);

            _slider = new Slider(100f, 0f, SliderDirection.Vertical) { name = "slider" };
            sliderAR.Add(_slider);
            _slider.RegisterValueChangedCallback<float>((x) => ChangeFillerHeight(x.newValue));

            _filler = _slider.Q<VisualElement>("unity-tracker");

            VisualElement valueContainer = new VisualElement() { name = "value-container" };
            valueContainer.AddToClassList(ussValueContainer);
            hierarchy.Add(valueContainer);

            AspectRatioPanel valueAR = new AspectRatioPanel() { name = "value-aspect-ratio", AspectRatioX = 1, AspectRatioY = 1 };
            valueContainer.Add(valueAR);

            _value = new Label("test") { name = "current-value" };
            valueAR.Add(_value);
        }
        private void UpdateValues()
        {
            Color musicColor = new Color(0, 191, 243);
            Color sfxColor = new Color(237, 0, 140);

            _slider.value = _floatVariable != null ? _floatVariable.Value : 50;
            _value.text = _slider.value.ToString("0.##") + "%";

            if(VolumenController == Volumen.Music)
            {
                _slider.Q<VisualElement>("unity-tracker").style.backgroundColor = musicColor;
                _slider.style.borderTopColor = musicColor;
                _slider.style.borderBottomColor = musicColor;
                _slider.style.borderLeftColor = musicColor;
                _slider.style.borderRightColor = musicColor;
                _icon.image = Resources.Load<Texture>("07_Images/Music-Icon");
                return;
            }
            _slider.Q<VisualElement>("unity-tracker").style.backgroundColor = sfxColor;
            _slider.style.borderTopColor = sfxColor;
            _slider.style.borderBottomColor = sfxColor;
            _slider.style.borderLeftColor = sfxColor;
            _slider.style.borderRightColor = sfxColor;
            _icon.image = Resources.Load<Texture>("07_Images/SFX-Icon");

        }
        private void ChangeFillerHeight(float value)
        {
            _filler.style.height = Length.Percent(value);
            _value.text = value.ToString("0.##") + "%";
            if(_slider.value != _floatVariable.Value) _floatVariable?.SetValue(value);
        }
    }
}
