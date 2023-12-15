using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace RotaryPong.UICursor
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(GamepadCursor))]
    public class SceneHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private GamepadCursor _gamepadCursor;
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneChanged;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneChanged;
        }
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _gamepadCursor = GetComponent<GamepadCursor>();
        }
        private void OnSceneChanged(Scene scene, LoadSceneMode mode)
        {
            Cursor.visible = false;
            switch(scene.name)
            {
                case "MainMenu": 
                _playerInput.enabled = true;
                _gamepadCursor.enabled = true;
                break;

                case "GameScene":
                _playerInput.enabled = false;
                _gamepadCursor.enabled = false;
                break;
            }
        }
    }
}
