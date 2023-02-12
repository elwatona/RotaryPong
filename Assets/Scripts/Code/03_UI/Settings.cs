using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;

namespace RotaryPong
{
    public class Settings : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<Settings, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }
    }
}
