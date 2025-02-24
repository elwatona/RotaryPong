using UnityEngine;
using UnityEngine.SceneManagement;

namespace RotaryPong.UICursor
{
    [RequireComponent(typeof(CursorController))]
    public class SceneHandler : MonoBehaviour
    {
        private InputManager.Player _playerInput;
        private CursorController _gamepadCursor;
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
            // _playerInput = GetComponent<PlayerInput>();
            _gamepadCursor = GetComponent<CursorController>();
        }
        private void OnSceneChanged(Scene scene, LoadSceneMode mode)
        {
            Cursor.visible = false;
            switch(scene.name)
            {
                case "MainMenu": 
                // _playerInput.enabled = true;
                _gamepadCursor.enabled = true;
                break;

                case "GameScene":
                // _playerInput.enabled = false;
                _gamepadCursor.enabled = false;
                break;
            }
        }
    }
}
