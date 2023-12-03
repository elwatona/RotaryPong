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
        [SerializeField] CodedEventListener _endGameListener;
        [SerializeField] CodedEventListener _backToMenuListener;
        [SerializeField] CodedEventListener _resetGameListener;
        [SerializeField] CodedEventListener _exitGameListener;
        
        [SerializeField, Header("Parameters")] FloatVariable _camShakeMagnitude;
        [SerializeField] BooleanVariable _didGameEnded;
        [SerializeField] PostProcessVolume _postProcessVolume;

        private bool _isUpdatingPostProcess;

        [SerializeReference] private Coroutine _goalRoutine;
        private Bloom _bloom;
        private ChromaticAberration _chromaticAberration;
        private LensDistortion _lensDistortion;

        private void OnEnable()
        {
            _afterScoreListener?.OnEnable(() => _goalRoutine = StartCoroutine(VFX()));
            _endGameListener?.OnEnable(EndGame);
            _backToMenuListener?.OnEnable(BackToMenu);
            _resetGameListener?.OnEnable(ResetScene);
            _exitGameListener?.OnEnable(ExitGame);
        }
        private void OnDisable()
        {
            _afterScoreListener?.OnDisable();
            _endGameListener?.OnDisable();
            _backToMenuListener?.OnDisable();
            _resetGameListener?.OnDisable();
            _exitGameListener?.OnDisable();
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
            if (_goalRoutine != null)
            {
                StopCoroutine(_goalRoutine);
                _bloom.intensity.value = 0.6f;
                _chromaticAberration.intensity.value = 0f;
                _lensDistortion.intensity.value = 0f;
            }
        }
        private void BackToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
        private void ResetScene()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        private void ExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
            return;
        }
    }
}