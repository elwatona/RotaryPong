using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;
using RotaryPong.UI.Component;

namespace RotaryPong.UI
{
    public class Pause : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<Pause, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        private const string styleSheet = "Pause";
        private const string ussRoot = "pause";
        private const string ussLeftDiv = "pause__left";
        private const string ussRightDiv = "pause__right";
        private const string ussVolumenDiv = "pause__volumen-container";
        private const string ussButtonDiv = "pause__btn-container";
        
        public VisualElement LeftDiv;
        public VisualElement RightDiv;

        public FloatVariable MusicVolumen;
        public FloatVariable SFXVolumen;

        public Pause()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/{0}", styleSheet)));
            AddToClassList(ussRoot);

            LeftDiv = new() {name = "left-container"};
            LeftDiv.AddToClassList(ussLeftDiv);
            hierarchy.Add(LeftDiv);

            RightDiv = new() {name = "right-container"};
            RightDiv.AddToClassList(ussRightDiv);
            hierarchy.Add(RightDiv);

            VisualElement musicContainer = new();
            LeftDiv.Add(musicContainer);
            musicContainer.AddToClassList(ussVolumenDiv);

            VolumenChanger music = new() {name = "Music", FloatVariable = MusicVolumen};
            musicContainer.Add(music);

            VisualElement leftButtons = new() {name = "btns-container"};
            leftButtons.AddToClassList(ussButtonDiv);
            LeftDiv.Add(leftButtons);

            AspectRatioButton resume = new(ButtonType.Menu, "RESUME") {name = "resume", AspectRatioX = 11, AspectRatioY = 2, BalanceY = 15, BalanceX = 15};
            leftButtons.Add(resume);

            AspectRatioButton reset = new(ButtonType.Menu, "RESET") {name = "reset", AspectRatioX = 11, AspectRatioY = 2, BalanceY = 85, BalanceX = 15};
            leftButtons.Add(reset);

            VisualElement sfxContainer = new();
            RightDiv.Add(sfxContainer);
            sfxContainer.AddToClassList(ussVolumenDiv);

            VolumenChanger sfx = new() {name = "SFX", FloatVariable = SFXVolumen, VolumenController = Volumen.SFX};
            sfxContainer.Add(sfx);

            VisualElement rightButtons = new() {name = "btns-container"};
            rightButtons.AddToClassList(ussButtonDiv);
            RightDiv.Add(rightButtons);

            AspectRatioButton menu = new(ButtonType.Menu, "GO TO MENU") {name = "menu", AspectRatioX = 11, AspectRatioY = 2, BalanceY = 15, BalanceX = 15};
            rightButtons.Add(menu);

            AspectRatioButton exit = new(ButtonType.Menu, "EXIT GAME") {name = "exit", AspectRatioX = 11, AspectRatioY = 2, BalanceY = 85, BalanceX = 15};
            rightButtons.Add(exit);
        }
    }
}
