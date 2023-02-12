using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;

namespace RotaryPong.UI
{
    public class ShapeChanger : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<ShapeChanger, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }
        #region USS names
            private const string styleSheet = "ShapeChanger";
            private const string ussRootContainer = "shape-changer";
            private const string ussRootAR = "shape-changer__root-aspect-ratio";
            private const string ussFooter = "shape-changer__footer";
        #endregion

        private ShapeVariable _shapeVariable;
        private Label _definition;

        public ShapeVariable ShapeVariable
        {
            get { return _shapeVariable; }
            set { _shapeVariable = value; UpdateValues(); }
        }

        public ShapeChanger()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/{0}", styleSheet)));
            AddToClassList(ussRootContainer);

            AspectRatioPanel rootAR = new AspectRatioPanel() { name = "root-aspect-ratio", AspectRatioX = 1, AspectRatioY = 1 };
            rootAR.AddToClassList(ussRootAR);
            hierarchy.Add(rootAR);

            VisualElement footer = new VisualElement() { name = "footer" };
            footer.AddToClassList(ussFooter);
            rootAR.Add(footer);

            SquareButton back = new SquareButton(ButtonType.Back) { name = "back-btn", AspectRatioX = 1, AspectRatioY = 1, BalanceX = 5, BalanceY = 50 };
            back.clickable.clicked += () => ChangeShape(-1);
            footer.Add(back);

            _definition = new Label("default-text") { name = "definition" };
            footer.Add(_definition);

            SquareButton next = new SquareButton(ButtonType.Next) { name = "next-btn", AspectRatioX = 1, AspectRatioY = 1, BalanceX = 95, BalanceY = 50 };
            next.clickable.clicked += () => ChangeShape(1);
            footer.Add(next);
        }
        private void ChangeShape(int value)
        {
            if(_shapeVariable == null)
            {
                Debug.LogWarning("Shape Changer necesita su variable");
                return;
            }

            var shapes = Enum.GetValues(typeof(Shape));
            int currentShape = (int)_shapeVariable.Value;
            int newShape = currentShape + value;
            int desiredShape = currentShape + value > shapes.Length-1 ? 0 : currentShape + value < 0 ? shapes.Length-1 : currentShape + value;

            _shapeVariable.Value = (Shape)desiredShape;

            UpdateValues();
        }
        private void UpdateValues()
        {
            if(_shapeVariable == null)
            {
                Debug.LogWarning("Shape Changer necesita su variable");
                return;
            }
            _definition.text = _shapeVariable.Value.ToString();
        }
    }
}
