using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using Watona.Utils;
using Watona.Variables;
using Watona.Events;
using RotaryPong.Events;

namespace RotaryPong
{
    public class GameManager : MonoBehaviour
    {
        IEnumerator VFX()
        {
            StartCoroutine(CamShake(1f, _camShakeMagnitude.Value));
            yield return new WaitForSeconds(2);
            _bloom.intensity.value = 0.6f;
            _chromaticAberration.intensity.value = 0f;
            _lensDistortion.intensity.value = 0f;
        }
        ///<summary> Agita la camara </summary>
        ///<param name="durationInSeconds"> Tiempo que durara el agitado de la camara </param>
        ///<param name="magnitude"> Fuerza con la que se agita la camara </param>
        IEnumerator CamShake(float durationInSeconds, float magnitude)
        {
            Vector3 originalPosition = Camera.main.transform.localPosition;
            float elapsedTime = 0.0f;

            while (elapsedTime < durationInSeconds)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                Camera.main.transform.localPosition = originalPosition + new Vector3(x, y, 0);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Camera.main.transform.localPosition = originalPosition;

            yield return null;
        }

        [SerializeField] CodedEventListener _afterScoreListener;
        
        [SerializeField, Header("Parameters")] FloatVariable _camShakeMagnitude;
        [SerializeField] IntVariable _pinkScore;
        [SerializeField] IntVariable _blueScore;
        [SerializeField] StringVariable _announcement;
        [SerializeField] BooleanVariable _didGameEnded;

        [SerializeField, Header("Configurations")] GameObject _pauseScreen;
        [SerializeField] PostProcessVolume _postProcessVolume;

        private bool _isUpdatingPostProcess;
        private bool _isPaused;

        private Coroutine _goalRoutine;
        private Bloom _bloom;
        private ChromaticAberration _chromaticAberration;
        private LensDistortion _lensDistortion;

        public void OnPause(InputAction.CallbackContext context) => HandlePause(!_isPaused, context);

        ///<summary> Retorna texto anunciando los resultados </summary>
        ///<param name="team"> El color del equipo ganador </param>
        private string EndGameText(Paint team)
        {
            if(team == Paint.White) return "The game ends in a tie!";
            return string.Format("{0} team wins the game", team);
        }
        ///<summary> Maneja la logica a ejecutar en caso de pausar </summary>
        ///<param name="value"> Define si estamos en pausa </param>
        private void HandlePause(bool value, InputAction.CallbackContext context)
        {
            _isPaused = value;
            _pauseScreen.SetActive(value);
            Time.timeScale = value ? 0 : 1;
        }
        ///<summary> Toma las configuraciones del Post Procesado </summary>
        private void GetPostProcessSettings()
        {
            _postProcessVolume.profile.TryGetSettings(out _bloom);
            _postProcessVolume.profile.TryGetSettings(out _chromaticAberration);
            _postProcessVolume.profile.TryGetSettings(out _lensDistortion);
        }
        ///<summary> Actualiza los valores del post procesado </summary>
        private void UpdatePostProcessing()
        {
            if (_isUpdatingPostProcess)
            {
                _bloom.intensity.value += 0.05f;
                _chromaticAberration.intensity.value += 0.02f;
                _lensDistortion.intensity.value -= 0.7f;
                return;
            }

            float bloomIntensityValue = _bloom.intensity.value;
            float chromaticAberrationIntensityValue = _chromaticAberration.intensity.value;
            float lensDistortionIntensityValue = _lensDistortion.intensity.value;

            _bloom.intensity.value = bloomIntensityValue > 0.6f ? bloomIntensityValue -= 0.05f : 0.6f;
            _chromaticAberration.intensity.value = chromaticAberrationIntensityValue > 0 ? chromaticAberrationIntensityValue -= 0.03f : 0;
            _lensDistortion.intensity.value = lensDistortionIntensityValue < 0 ? lensDistortionIntensityValue += 0.7f : 0;
        }
        ///<summary> Prepara valores para el termino de la partida </summary>
        private void EndGame()
        {
            int pinkScore = _pinkScore.Value;
            int blueScore = _blueScore.Value;
            Paint winner = pinkScore == blueScore ? Paint.White : pinkScore > blueScore ? Paint.Pink : Paint.Blue;

            if (_goalRoutine != null)
            {
                StopCoroutine(_goalRoutine);
                _bloom.intensity.value = 0.6f;
                _chromaticAberration.intensity.value = 0f;
                _lensDistortion.intensity.value = 0f;
            }
            
            _announcement.Value = EndGameText(winner);
            
            // UIManager.Instance.EndGame(winner);

            // StartCoroutine(EndGameBehaviour());
        }
        
        public void BackToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
        public void ResetScene()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(1);
        }

        private void OnEnable()
        {
            _afterScoreListener?.OnEnable(() => _goalRoutine = StartCoroutine(VFX()));
        }
        private void OnDisable()
        {
            _afterScoreListener?.OnDisable();
        }
        private void Awake()
        {
            GetPostProcessSettings();
        }
        private void FixedUpdate()
        {
            if (!_didGameEnded.Value)
            {
                UpdatePostProcessing();
            }
        }

    }
}