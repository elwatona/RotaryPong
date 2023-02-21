using UnityEngine;
using UnityEngine.UIElements;

namespace RotaryPong.UI
{
    public class MainMenu : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<MainMenu, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        private const string styleSheet = "MainMenu";
        private const string ussRoot = "main-menu";
        private const string ussTitle = "main-menu__title";
        private const string ussTitleLabel = "main-menu__title--label";
        private const string ussTitleLabelLeft = "main-menu__title--label_left";
        private const string ussTitleLabelRight = "main-menu__title--label_right";
        private const string ussElementsContainer = "main-menu___elements-container";
        private const string ussBtnContainer = "main-menu__btn-container";

        public AspectRatioButton Play;
        public AspectRatioButton Settings;
        public AspectRatioButton ExitGame;

        public MainMenu()
        {
			styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/{0}", styleSheet)));
            AddToClassList(ussRoot);

            VisualElement titleContainer = new() { name = "title-container" };
            titleContainer.AddToClassList(ussTitle);
            hierarchy.Add(titleContainer);

            Label titleLeft = new("ROTARY");
            titleLeft.AddToClassList(ussTitleLabel);
            titleLeft.AddToClassList(ussTitleLabelLeft);
            titleContainer.Add(titleLeft);

            Label titleRight = new("PONG");
            titleRight.AddToClassList(ussTitleLabel);
            titleRight.AddToClassList(ussTitleLabelRight);
            titleContainer.Add(titleRight);

            VisualElement elements = new() { name = "elements "};
            elements.AddToClassList(ussElementsContainer);
            hierarchy.Add(elements);

            VisualElement playContainer = new();
            playContainer.AddToClassList(ussBtnContainer);
            playContainer.style.width = Length.Percent(50);
            elements.Add(playContainer);

            Play = new(ButtonType.Menu, "PLAY") { name = "btn-play", AspectRatioX = 11, AspectRatioY = 2};
            playContainer.Add(Play);

            VisualElement settingContainer = new();
            settingContainer.AddToClassList(ussBtnContainer);
            elements.Add(settingContainer);

            Settings = new(ButtonType.Menu, "SETTINGS") { name = "btn-settings", AspectRatioX = 11, AspectRatioY = 2};
            settingContainer.Add(Settings);

            VisualElement exitContainer = new();
            exitContainer.AddToClassList(ussBtnContainer);
            elements.Add(exitContainer);

            ExitGame = new(ButtonType.Menu, "EXIT GAME") { name = "btn-exit", AspectRatioX = 11, AspectRatioY = 2};
            exitContainer.Add(ExitGame);
        }
    }
}
