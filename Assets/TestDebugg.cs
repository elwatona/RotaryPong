using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;

using RotaryPong.UI;

public class TestDebugg : MonoBehaviour
{
    private VisualElement _root;
    private BooleanValue _valueChanger;
    [SerializeField] BooleanVariable _variable;
    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _valueChanger = _root.Q<BooleanValue>("Debug");
    }
    private void Start()
    {
        // _valueChanger.BooleanVariable = _variable;
    }
    private void Update()
    {
        // _valueChanger.Definition = _variable.name;
        // _valueChanger.Value = _variable.Value;
    }
}
