using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;

namespace RotaryPong.UI
{
    public class Settings : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<Settings, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        private const string styleSheet = "Settings";
        private const string ussRoot = "settings";
        private const string ussTitle = "settings__title";
        private const string ussTitleLabel = "settings__title--label";
        private const string ussTitleLabelLeft = "settings__title--label_left";
        private const string ussTitleLabelRight = "settings__title--label_right";
        private const string ussContainer = "settings__elements-container";
        private const string ussColumn = "setting__column";
        private const string ussElementTitle = "setting__element-title";
        private const string ussElement = "settings__element";
        
        public VisualElement TitleContainer;
        private VisualElement _elements;
        public VisualElement Options;

        public VolumenChanger Music;
        public VolumenChanger SFX;

        public ShapeChanger ShapeChanger;
        public ValueChanger PlayerSpeed;
        public BooleanValue SmoothRotation;

        public SettingPreset SettingPreset;
        public BooleanValue BallEffect;
        public ValueChanger EffectTimer;
        public BooleanValue ColorByBounce;
        public ValueChanger ColorDuration;

        public BooleanValue ControlMapSpin;
        public ValueChanger MapSpeed;
        public ValueChanger MatchTimer;
        public BooleanValue PlayerBounce;
        public BooleanValue FrontOnlyGoals;

        public AspectRatioButton Done;

        private List<VisualElement> options = new();
        private PresetVariable _presetVariable;

        public Settings()
        {
            _presetVariable = Resources.Load<PresetVariable>("Presets");

			styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/{0}", styleSheet)));
            AddToClassList(ussRoot);

            TitleContainer = new();
            TitleContainer.AddToClassList(ussTitle);
            hierarchy.Add(TitleContainer);

            Label leftTitle = new("SETT");
            leftTitle.AddToClassList(ussTitleLabel);
            leftTitle.AddToClassList(ussTitleLabelLeft);
            TitleContainer.Add(leftTitle);

            Label rightTitle = new("INGS");
            rightTitle.AddToClassList(ussTitleLabel);
            rightTitle.AddToClassList(ussTitleLabelRight);
            TitleContainer.Add(rightTitle);

            _elements = new();
            _elements.AddToClassList(ussContainer);
            hierarchy.Add(_elements);

            Music = new() { name = "Music", VolumenController = Volumen.Music};
            Music.style.flexGrow = 3;
            _elements.Add(Music);

            Options = new();
            Options.AddToClassList(ussContainer);
            _elements.Add(Options);

            InstantiateOptions(Options);

            SFX = new() { name = "SFX", VolumenController = Volumen.SFX};
            SFX.style.flexGrow = 3;
            _elements.Add(SFX);

            VisualElement btnContainer = new();
            btnContainer.style.marginTop = 20;
            btnContainer.style.marginBottom = 20;
            btnContainer.style.flexBasis = Length.Percent(15);
            hierarchy.Add(btnContainer);

            Done = new(ButtonType.Menu, "DONE") { name = "Done", AspectRatioX = 5, AspectRatioY = 2 };
            btnContainer.Add(Done);

            SetOptionList();

            _presetVariable.PropertyChanged += (x,y) => 
            {
                if(_presetVariable.Value != Preset.Custom) 
                {
                    SetOptions(false); 
                    return;
                } 
                SetOptions(true);
            };
        }
        private void InstantiateOptions(VisualElement father)
        {
            VisualElement player = new() { name = "Player-Column" };
            AddColumn(player, father);

            Label playerTitle = new("Player");
            playerTitle.AddToClassList(ussElementTitle);
            SetFlexGrow(playerTitle, 4);
            player.Add(playerTitle);

            ShapeChanger = new();
            SetFlexGrow(ShapeChanger, 4);
            player.Add(ShapeChanger);

            PlayerSpeed = new() {MinValue = 8, MaxValue = 12, ChangeValue = 1};
            AddElement(PlayerSpeed, player, 4);

            SmoothRotation = new();
            AddElement(SmoothRotation, player, 4);

            VisualElement ball = new() { name = "Ball-Column" };
            AddColumn(ball, father);

            SettingPreset = new();
            AddElement(SettingPreset, ball, 2);

            VisualElement ballElements = new();
            ballElements.style.flexGrow = 2;
            ballElements.style.justifyContent = Justify.FlexEnd;
            AddElement(ballElements, ball, 2);

            Label ballTitle = new("Ball");
            ballTitle.AddToClassList(ussElementTitle);
            SetFlexGrow(ballTitle, 5);
            ballElements.Add(ballTitle);

            BallEffect = new();
            AddElement(BallEffect, ballElements, 5);

            EffectTimer = new() {MinValue = 0.2f, MaxValue = 0.75f, ChangeValue = 0.05f};
            AddElement(EffectTimer, ballElements, 5);

            ColorByBounce = new();
            AddElement(ColorByBounce, ballElements, 5);

            ColorDuration = new() {MinValue = 1, MaxValue = 4, ChangeValue = 1};
            AddElement(ColorDuration, ballElements, 5);

            VisualElement mapMatch = new() { name = "Map-Match-Column" };
            AddColumn(mapMatch, father);

            VisualElement map = new();
            AddElement(map, mapMatch, 2);

            Label mapLabel = new("Map");
            mapLabel.AddToClassList(ussElementTitle);
            SetFlexGrow(mapLabel, 3);
            map.Add(mapLabel);

            ControlMapSpin = new();
            AddElement(ControlMapSpin, map, 3);

            MapSpeed = new() {MinValue = 90, MaxValue = 115, ChangeValue = 5};
            AddElement(MapSpeed, map, 3);

            VisualElement match = new();
            match.style.flexGrow = 2;
            match.style.justifyContent = Justify.FlexEnd;
            AddElement(match, mapMatch, 2);

            Label matchLabel = new("Match");
            matchLabel.AddToClassList(ussElementTitle);
            SetFlexGrow(matchLabel, 4);
            match.Add(matchLabel);

            MatchTimer = new() {MinValue = 90, MaxValue = 180, ChangeValue = 30};
            AddElement(MatchTimer, match, 4);

            PlayerBounce = new();
            AddElement(PlayerBounce, match, 4);

            FrontOnlyGoals = new();
            AddElement(FrontOnlyGoals, match, 4);
        }
        private void AddColumn(VisualElement column, VisualElement father)
        {
            column.AddToClassList(ussColumn);
            father.Add(column);
        }
        private void AddElement(VisualElement element, VisualElement father, float flexGrow)
        {
            SetFlexGrow(element, flexGrow);
            element.AddToClassList(ussElement);
            father.Add(element);
        }
        private void SetFlexGrow(VisualElement element, float value)
        {
            element.style.flexGrow = value;
        }
        private void SetOptionList()
        {
            options.Add(ShapeChanger);
            options.Add(PlayerSpeed);
            // options.Add(SmoothRotation);
            options.Add(BallEffect);
            options.Add(EffectTimer);
            options.Add(ColorByBounce);
            options.Add(ColorDuration);
            options.Add(ControlMapSpin);
            options.Add(MapSpeed);
            // options.Add(MatchTimer);
            options.Add(PlayerBounce);
            options.Add(FrontOnlyGoals);
        }
        public void SetOptions(bool value)
        {
            foreach(VisualElement ve in options)
            {
                ve.SetEnabled(value);
            }
        }
    }
}
