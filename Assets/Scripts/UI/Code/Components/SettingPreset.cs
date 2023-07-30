using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RotaryPong.UI.Component
{
    public class SettingPreset : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<SettingPreset, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        private const string styleSheet = "SettingPreset";
        private const string ussRootContainer = "setting-preset";
        private const string ussBtnContainer = "setting-preset__btn-container";

        private Label _definition;
        private AspectRatioButton _preset1;
        private AspectRatioButton _preset2;
        private AspectRatioButton _preset3;
        private AspectRatioButton _preset4;
        private PresetVariable _presetVariable;

        public List<AspectRatioButton> Buttons = new();
        public PresetVariable Preset
        {
            get { return _presetVariable; }
            set { _presetVariable = value; }
        }

		public SettingPreset()
		{
            _presetVariable = Resources.Load<PresetVariable>("Presets");
            _presetVariable.PropertyChanged += (x,y) => UpdateText();
            
			styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/{0}", styleSheet)));
            AddToClassList(ussRootContainer);

            _definition = new("Preset") { name = "definition" };
            hierarchy.Add(_definition);

            VisualElement buttonContainer = new() { name = "btn-container" };
            buttonContainer.AddToClassList(ussBtnContainer);
            hierarchy.Add(buttonContainer);

            Buttons.Add(_preset1);
            Buttons.Add(_preset2);
            Buttons.Add(_preset3);
            Buttons.Add(_preset4);

            for (int i = 0; i < Buttons.Count; i++)
            {
                string index = i < 3 ? string.Format("{0}", i+1) : "C";
                int balanceX = i * 33 == 99 ? 100 : i * 33;
                Buttons[i] = new(ButtonType.Preset, index) {AspectRatioX = 1, AspectRatioY = 1, BalanceX = balanceX};
                Buttons[i].name = index;
                buttonContainer.Add(Buttons[i]);
            }
            foreach(AspectRatioButton btn in Buttons)
            {
                int index = Buttons.IndexOf(btn);
                btn.clicked += () => ChangePreset(index);
            }

            UpdateText();
		}
        private void ChangePreset(int index)
        {
            if(_presetVariable == null) return;

            Debug.Log(index);
            _presetVariable.SetValue((Preset)index);
        }
        private void UpdateText()
        {
            _definition.text = _presetVariable.Value != RotaryPong.Preset.Custom ? string.Format("Preset: {0}", _presetVariable.Value) : "Custom Settings";
        }
    }
}
