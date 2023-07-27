using UnityEngine;
using UnityEngine.UIElements;
using RotaryPong.UI.Component;

namespace RotaryPong.UI
{
    public class MainMenu : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<MainMenu, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        private const string STYLE_SHEET = "MainMenu";
        private const string USS_ROOT = "main-menu";
        private const string USS_TITLE = "main-menu__title";
        private const string USS_TITLE_LABEL = "main-menu__title--label";
        private const string USS_TITLE_LABEL_LEFT = "main-menu__title--label_left";
        private const string USS_TITLE_LABEL_RIGHT = "main-menu__title--label_right";
        private const string USS_ELEMENTS_CONTAINER = "main-menu___elements-container";
        private const string USS_BTN_CONTAINER = "main-menu__btn-container";
        private const string USS_DESIGN_BY_LABEL = "main-menu__design-by";
        private const string USS_PABLO = "main-menu__pablo";
        private const string USS_VERSION = "main-menu__version";

        private const string PATCH_NOTES_URL = "https://elwa-bazaes.itch.io/rotary-pong";
        private const string GORIGOITIA_URL = "https://gorigoitia.itch.io";

        public AspectRatioButton Play;
        public AspectRatioButton Settings;
        public AspectRatioButton ExitGame;

        public MainMenu()
        {
			styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/{0}", STYLE_SHEET)));
            AddToClassList(USS_ROOT);

            VisualElement titleContainer = new() { name = "title-container" };
            titleContainer.AddToClassList(USS_TITLE);
            hierarchy.Add(titleContainer);

            Label titleLeft = new("ROTARY");
            titleLeft.AddToClassList(USS_TITLE_LABEL);
            titleLeft.AddToClassList(USS_TITLE_LABEL_LEFT);
            titleContainer.Add(titleLeft);

            Label titleRight = new("PONG");
            titleRight.AddToClassList(USS_TITLE_LABEL);
            titleRight.AddToClassList(USS_TITLE_LABEL_RIGHT);
            titleContainer.Add(titleRight);

            VisualElement elements = new() { name = "elements "};
            elements.AddToClassList(USS_ELEMENTS_CONTAINER);
            hierarchy.Add(elements);

            VisualElement playContainer = new();
            playContainer.AddToClassList(USS_BTN_CONTAINER);
            // playContainer.style.width = Length.Percent(50);
            // playContainer.style.paddingBottom = 20;
            // playContainer.style.paddingTop = 20;
            elements.Add(playContainer);

            Play = new(ButtonType.Menu, "PLAY") { name = "btn-play", AspectRatioX = 361, AspectRatioY = 95};
            Play.style.fontSize = 64;
            playContainer.Add(Play);

            VisualElement settingContainer = new();
            settingContainer.AddToClassList(USS_BTN_CONTAINER);
            elements.Add(settingContainer);

            Settings = new(ButtonType.Menu, "SETTINGS") { name = "btn-settings", AspectRatioX = 261, AspectRatioY = 95};
            settingContainer.Add(Settings);

            VisualElement exitContainer = new();
            exitContainer.AddToClassList(USS_BTN_CONTAINER);
            elements.Add(exitContainer);

            ExitGame = new(ButtonType.Menu, "EXIT GAME") { name = "btn-exit", AspectRatioX = 261, AspectRatioY = 95};
            exitContainer.Add(ExitGame);

            AddBottomRightElements();
            AddBottomLeftElements();
        }

        private void AddBottomRightElements()
        {
            VisualElement RightBottomCorner = new() {name = "Absolute-Bottom-Right"};
            RightBottomCorner.style.position = Position.Absolute;
            RightBottomCorner.style.flexBasis = Length.Percent(25);
            RightBottomCorner.style.right = 50;
            RightBottomCorner.style.bottom = 25;
            RightBottomCorner.style.width = 500;
            RightBottomCorner.style.height = 250;
            RightBottomCorner.style.flexDirection = FlexDirection.Column;
            RightBottomCorner.style.alignItems = Align.FlexEnd;
            RightBottomCorner.style.justifyContent = Justify.SpaceBetween;
            Add(RightBottomCorner);

            VisualElement achievementsContainer = new();
            achievementsContainer.style.width = 100;
            achievementsContainer.style.height = 100;
            RightBottomCorner.Add(achievementsContainer);

            AspectRatioButton achievements = new(ButtonType.Icon) {AspectRatioX = 1, AspectRatioY = 1};
            achievementsContainer.Add(achievements);

            Image achievementsIcon = new();
            achievementsIcon.sprite = Resources.Load<Sprite>("07_Images/Achievements-icon");
            achievements.Add(achievementsIcon);

            VisualElement labelContainer = new();
            labelContainer.style.width = Length.Percent(100);
            labelContainer.style.flexDirection = FlexDirection.Row;
            labelContainer.style.justifyContent = Justify.FlexEnd;
            RightBottomCorner.Add(labelContainer);

            Label designByLabel = new("Design by ");
            designByLabel.AddToClassList(USS_DESIGN_BY_LABEL);
            labelContainer.Add(designByLabel);

            Label pablo = new("Pablo Gorigoitia");
            pablo.AddToClassList(USS_PABLO);
            pablo.RegisterCallback<ClickEvent>((x) => Application.OpenURL(GORIGOITIA_URL));
            labelContainer.Add(pablo);

            VisualElement creditsContainer = new();
            creditsContainer.style.height = 75;
            creditsContainer.style.width = Length.Percent(100);
            RightBottomCorner.Add(creditsContainer);

            AspectRatioButton credits = new AspectRatioButton(ButtonType.Menu, "CREDITS") {name = "btn-credits", AspectRatioX = 231, AspectRatioY = 75, BalanceX = 100};
            creditsContainer.Add(credits);
        }
        private void AddBottomLeftElements()
        {

            Label LeftBottomCorner = new("Patch " + Application.version) {name = "Absolute-Bottom-Right"};
            LeftBottomCorner.AddToClassList(USS_VERSION);
            LeftBottomCorner.RegisterCallback<ClickEvent>((x) => Application.OpenURL(PATCH_NOTES_URL));
            Add(LeftBottomCorner);
        }
    }
}
