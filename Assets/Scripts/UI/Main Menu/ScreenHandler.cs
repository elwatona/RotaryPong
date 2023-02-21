using UnityEngine;
using Watona.Events;
using Watona.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace RotaryPong.UI
{
    public class ScreenHandler : MonoBehaviour
    {
        [SerializeField] CodedEventListener _exitButtonListener;
        [SerializeField] CodedEventListener _playButtonListener;
        private void OnEnable()
        {
            _playButtonListener.OnEnable(Play);
            _exitButtonListener.OnEnable(Exit);
        }
        private void OnDisable()
        {
            _playButtonListener.OnDisable();
            _exitButtonListener.OnDisable();
        }
        private void Play()
        {
            SceneManager.LoadScene(1);
        }
        private void Exit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
            return;
        }
    }
}
