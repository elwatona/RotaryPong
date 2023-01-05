using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RotaryPong
{
    public class MainMenuCanvas : MonoBehaviour
    {
        [SerializeField] GameObject controlPanel;
        [SerializeField] InputField playerSpeedInputField;
        [SerializeField] InputField rotationAmmountInputField;
        [SerializeField] InputField matchDurationInputField;
        [SerializeField] InputField ballSpeedInputField;
        [SerializeField] InputField ballDragInputField;
        [SerializeField] InputField ballMinSpeedInputField;
        [SerializeField] InputField ballColorDurationInputField;
        [SerializeField] InputField mapSpeedSpinInputField;
        [SerializeField] InputField celebrationFireworksInputField;
        [SerializeField] InputField camShakeMagnitudeInputField;
        [SerializeField] GameObject rotationSpeedGO;
        [SerializeField] Text speedExmpText;
        [SerializeField] Text rotAmmExmpText;
        [SerializeField] Toggle smoothRotToggle;
        [SerializeField] Toggle goalBackWallToggle;
        [SerializeField] Text smoothRotSpeedExmpText;
        [SerializeField] Text matchDurationExmpText;
        [SerializeField] Text ballSpeedExmpText;
        [SerializeField] Text ballDragExmpText;
        [SerializeField] Text ballMinSpeedExmpText;
        [SerializeField] Text ballColorDurationExmpText;
        [SerializeField] Text mapSpinSpeedExmpText;
        [SerializeField] Text celebrationFireworksExmpText;
        [SerializeField] Text camShakeMagnitudeExmpText;

        private InputField rotationSpeedInputField;
        private float savedPlayerSpeed;
        private float savedPlayerRotAmmount;
        private bool savedSmoothRotBool;
        private bool savedGoalBackWallBool;
        private float savedRotSpeed;
        private float savedMatchDuration;
        private float savedBallSpeed;
        private float savedBallDrag;
        private float savedBallMinSpeed;
        private float savedBallColorDuration;
        private float savedMapSpinSpeed;
        private float savedCelebrationFireworks;
        private float savedCamShakeMagnitude;
        
        private void Start()
        {
            rotationSpeedInputField = rotationSpeedGO.GetComponent<InputField>();
            LoadValues();
        }

        private void LoadValues()
        {
            if (PlayerPrefs.HasKey("playerSpeed"))
            {
                savedPlayerSpeed = PlayerPrefs.GetFloat("playerSpeed");
            }
            if (PlayerPrefs.HasKey("playerRotAmmount"))
            {
                savedPlayerRotAmmount = PlayerPrefs.GetFloat("playerRotAmmount");
            }
            if (PlayerPrefs.HasKey("playerRotSmooth"))
            {
                int rotInt = PlayerPrefs.GetInt("playerRotSmooth");
                if (rotInt == 0)
                {
                    savedSmoothRotBool = false;
                }
                else if (rotInt == 1)
                {
                    savedSmoothRotBool = true;
                }
            }
            if (PlayerPrefs.HasKey("goalsBackWall"))
            {
                int backWallInt = PlayerPrefs.GetInt("goalsBackWall");
                if (backWallInt == 0)
                {
                    savedGoalBackWallBool = false;
                }
                else if (backWallInt == 1)
                {
                    savedGoalBackWallBool = true;
                }
            }
            if (PlayerPrefs.HasKey("playerRotSpeed"))
            {
                savedRotSpeed = PlayerPrefs.GetFloat("playerRotSpeed");
            }
            if (PlayerPrefs.HasKey("matchDuration"))
            {
                savedMatchDuration = PlayerPrefs.GetFloat("matchDuration");
            }
            if (PlayerPrefs.HasKey("ballSpeed"))
            {
                savedBallSpeed = PlayerPrefs.GetFloat("ballSpeed");
            }
            if (PlayerPrefs.HasKey("ballDrag"))
            {
                savedBallDrag = PlayerPrefs.GetFloat("ballDrag");
            }
            if (PlayerPrefs.HasKey("ballMinSpeed"))
            {
                savedBallMinSpeed = PlayerPrefs.GetFloat("ballMinSpeed");
            }
            if (PlayerPrefs.HasKey("ballColorDuration"))
            {
                savedBallColorDuration = PlayerPrefs.GetFloat("ballColorDuration");
            }
            if (PlayerPrefs.HasKey("mapSpinSpeed"))
            {
                savedMapSpinSpeed = PlayerPrefs.GetFloat("mapSpinSpeed");
            }
            if (PlayerPrefs.HasKey("celebrationFireworks"))
            {
                savedCelebrationFireworks = PlayerPrefs.GetFloat("celebrationFireworks");
            }
            if (PlayerPrefs.HasKey("camShakeMagnitude"))
            {
                savedCamShakeMagnitude = PlayerPrefs.GetFloat("camShakeMagnitude");
            }

            if (savedPlayerSpeed != 0)
            {
                speedExmpText.text = savedPlayerSpeed.ToString();
            }
            if (savedPlayerRotAmmount != 0)
            {
                rotAmmExmpText.text = savedPlayerRotAmmount.ToString();
            }
            smoothRotToggle.isOn = savedSmoothRotBool;
            rotationSpeedGO.SetActive(savedSmoothRotBool);
            goalBackWallToggle.isOn = savedGoalBackWallBool;
            if (savedRotSpeed != 0)
            {
                smoothRotSpeedExmpText.text = savedRotSpeed.ToString();
            }
            if (savedMatchDuration != 0)
            {
                matchDurationExmpText.text = savedMatchDuration.ToString();
            }
            if (savedBallSpeed != 0)
            {
                ballSpeedExmpText.text = savedBallSpeed.ToString();
            }
            if (savedBallDrag != 0)
            {
                ballDragExmpText.text = savedBallDrag.ToString();
            }
            if (savedBallMinSpeed != 0)
            {
                ballMinSpeedExmpText.text = savedBallMinSpeed.ToString();
            }
            if (savedBallColorDuration != 0)
            {
                ballColorDurationExmpText.text = savedBallColorDuration.ToString();
            }
            if (savedMapSpinSpeed != 0)
            {
                mapSpinSpeedExmpText.text = savedMapSpinSpeed.ToString();
            }
            if (savedCelebrationFireworks != 0)
            {
                celebrationFireworksExmpText.text = savedCelebrationFireworks.ToString();
            }
            if (savedCamShakeMagnitude != 0)
            {
                camShakeMagnitudeExmpText.text = savedCamShakeMagnitude.ToString();
            }
        }

        public void OnSmoothRotToggle()
        {
            rotationSpeedGO.SetActive(smoothRotToggle.isOn);
        }

        public void AcceptButton()
        {
            if (playerSpeedInputField.textComponent.text != "")
            {
                float newPlayerSpeed = float.Parse(playerSpeedInputField.textComponent.text);
                PlayerPrefs.SetFloat("playerSpeed", newPlayerSpeed);
            }
            if (rotationAmmountInputField.textComponent.text != "")
            {
                float newPlayerRot = float.Parse(rotationAmmountInputField.textComponent.text);
                PlayerPrefs.SetFloat("playerRotAmmount", newPlayerRot);
            }
            if (smoothRotToggle.isOn)
            {
                PlayerPrefs.SetInt("playerRotSmooth", 1);
            }
            else
            {
                PlayerPrefs.SetInt("playerRotSmooth", 0);
            }
            if (goalBackWallToggle.isOn)
            {
                PlayerPrefs.SetInt("goalsBackWall", 1);
            }
            else
            {
                PlayerPrefs.SetInt("goalsBackWall", 0);
            }
            if (rotationSpeedInputField.textComponent.text != "")
            {
                float newPlayerRotSpeed = float.Parse(rotationSpeedInputField.textComponent.text);
                PlayerPrefs.SetFloat("playerRotSpeed", newPlayerRotSpeed);
            }
            if (matchDurationInputField.textComponent.text != "")
            {
                float newMatchDuration = float.Parse(matchDurationInputField.textComponent.text);
                PlayerPrefs.SetFloat("matchDuration", newMatchDuration);
            }
            if (ballSpeedInputField.textComponent.text != "")
            {
                float newBallSpeed = float.Parse(ballSpeedInputField.textComponent.text);
                PlayerPrefs.SetFloat("ballSpeed", newBallSpeed);
            }
            if (ballDragInputField.textComponent.text != "")
            {
                float newBallDrag = float.Parse(ballDragInputField.textComponent.text);
                PlayerPrefs.SetFloat("ballDrag", newBallDrag);
            }
            if (ballMinSpeedInputField.textComponent.text != "")
            {
                float newBallMinSpeed = float.Parse(ballMinSpeedInputField.textComponent.text);
                PlayerPrefs.SetFloat("ballMinSpeed", newBallMinSpeed);
            }
            if (ballColorDurationInputField.textComponent.text != "")
            {
                float newBallColorDuration = float.Parse(ballColorDurationInputField.textComponent.text);
                PlayerPrefs.SetFloat("ballColorDuration", newBallColorDuration);
            }
            if (mapSpeedSpinInputField.textComponent.text != "")
            {
                float newMapSpinSpeed = float.Parse(mapSpeedSpinInputField.textComponent.text);
                PlayerPrefs.SetFloat("mapSpinSpeed", newMapSpinSpeed);
            }
            if (celebrationFireworksInputField.textComponent.text != "")
            {
                float newCelebrationFireworks = float.Parse(celebrationFireworksInputField.textComponent.text);
                PlayerPrefs.SetFloat("celebrationFireworks", newCelebrationFireworks);
            }
            if (camShakeMagnitudeInputField.textComponent.text != "")
            {
                float newCamShakeMagnitude = float.Parse(camShakeMagnitudeInputField.textComponent.text);
                PlayerPrefs.SetFloat("camShakeMagnitude", newCamShakeMagnitude);
            }
            controlPanel.SetActive(false);
        }

        public void StartButton()
        {
            SceneManager.LoadScene(1);
        }

        public void ControlsButton()
        {
            controlPanel.SetActive(true);
        }
    }
}