using UnityEngine;
using UnityEngine.SceneManagement;

namespace RotaryPong
{
    public class MainMenuCanvas : MonoBehaviour
    {
        public void StartButton()
        {
            SceneManager.LoadScene(1);
        }
    }
}