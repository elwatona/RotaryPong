using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using Watona.Utils;
using Watona.Utils.Variables;
namespace RotaryPong
{
    public enum Paint
    {
        White,
        Pink,
        Blue
    }
    public class GameManager : SingletonBehaviour<GameManager>
    {
        ///<summary> Modifica los valores que ocurren durante y despues del gol </summary> 
        IEnumerator GoalBehaviour(Ball ball)
        {
            Point(ball.Paint);

            _announcement.Variable.Value = string.Format("Point for {0}", ball.Paint);
            
            UIManager.Instance.Goal(ball.Paint);

            StartCoroutine(CamShake(1f, _camShakeMagnitude.Value));

            ball.SetInvis();

            _isUpdatingPostProcess = true;

            yield return new WaitForSeconds(0.5f);

            _isUpdatingPostProcess = false;

            yield return new WaitForSeconds(1.5f);

            _bloom.intensity.value = 0.6f;
            _chromaticAberration.intensity.value = 0f;
            _lensDistortion.intensity.value = 0f;
            _mapRotationDirection *= -1;

            ball.TurnBackOn();

            yield return null;
        }
        
        ///<summary> Agita la camara </summary>
        ///<param name="duration"> Tiempo que durara el agitado de la camara </param>
        ///<param name="magnitude"> Fuerza con la que se agita la camara </param>
        IEnumerator CamShake(float duration, float magnitude)
        {
            Vector3 originalPosition = Camera.main.transform.localPosition;
            float elapsedTime = 0.0f;

            while (elapsedTime < duration)
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
        
        ///<summary> Maneja la logica que permite el juego termine </summary>
        IEnumerator EndGameBehaviour()
        {
            int spawnedFireworks = 0;
            float celebrationFireworks = _celebrationFireworks.Value;

            while (spawnedFireworks < celebrationFireworks)
            {
                float randomDelay = Random.Range(0.05f, 0.7f);

                Debug.Log("spawning after delay: " + randomDelay);

                yield return new WaitForSeconds(randomDelay);

                float randomX = Random.Range(-25f, 25f);
                float randomY = Random.Range(-12f, 12f);
                Vector3 spawnPos = new Vector3(randomX, randomY, 20);

                Fireworks(spawnPos, true);

                Debug.Log("spawning firework at: " + spawnPos);

                spawnedFireworks++;

                yield return null;
            }
            yield return new WaitForSeconds(3);

            BackToMenu();

            yield return null;
        }
        
        [SerializeField, Header("Parameters")] FloatReference _matchDuration;
        [SerializeField] FloatReference _mapSpinSpeed;
        [SerializeField] FloatReference _celebrationFireworks;
        [SerializeField] FloatReference _camShakeMagnitude;
        [SerializeField] IntReference _pinkScore;
        [SerializeField] IntReference _blueScore;
        [SerializeField] BooleanReference _enableGodWalls;
        [SerializeField] StringReference _announcement;
        [SerializeField] StringReference _timer;

        [SerializeField, Header("Configurations")] GameObject _pauseScreen;
        [SerializeField] GameObject _map;
        [SerializeField] GameObject[] _goals;
        [SerializeField] GameObject[] _walls;
        [SerializeField] PostProcessVolume _postProcessVolume;
        [SerializeField] Color[] celebrationFireworksColors;

        private float _mapRotationDirection;
        private float _maxMatchTimer;

        private bool _isUpdatingPostProcess;
        private bool _didGameEnded;
        private bool _isPaused;

        private Coroutine _goalRoutine;
        private Bloom _bloom;
        private ChromaticAberration _chromaticAberration;
        private LensDistortion _lensDistortion;

        public void OnPause(InputAction.CallbackContext ctx) => HandlePause(!_isPaused);

        ///<summary> Retorna las particulas a instanciar en caso de gol </summary>
        private GameObject GetGoalParticles()
        {
            return Resources.Load<GameObject>("02_Prefabs/ParticleExplosion");
        }
        ///<summary> Retorna texto anunciando los resultados </summary>
        ///<param name="team"> El color del equipo ganador </param>
        private string EndGameText(Paint team)
        {
            if(team == Paint.White) return "The game ends in a tie!";
            return string.Format("{0} team wins the game", team);
        }
        ///<summary> Maneja la logica a ejecutar en caso de pausar </summary>
        ///<param name="value"> Define si estamos en pausa </param>
        private void HandlePause(bool value)
        {
            _isPaused = value;
            _pauseScreen.SetActive(value);
            Time.timeScale = value ? 0 : 1;
        }
        ///<summary> Define el valor de <paramref name="_maxMatchTimer"/> </summary>
        private void SetTimer()
        {
            _maxMatchTimer = _matchDuration.Value;
        }
        ///<sumary> Decide el comportamiento de los arcos </summary>
        private void SetWalls()
        {
            bool enableGodWalls = _enableGodWalls.Value;
            EnableGoals(!enableGodWalls);
            EnableWalls(enableGodWalls);
        }
        ///<summary> Establece la direccion de rotacion del escenario </summary>
        private void SetMapRotation()
        {
            _mapRotationDirection = -1;
        }
        ///<summary> Toma las configuraciones del Post Procesado </summary>
        private void GetPostProcessSettings()
        {
            _postProcessVolume.profile.TryGetSettings(out _bloom);
            _postProcessVolume.profile.TryGetSettings(out _chromaticAberration);
            _postProcessVolume.profile.TryGetSettings(out _lensDistortion);
        }
        ///<summary> Reinicia los valores de puntaje a 0 </summary>
        private void ResetPoint()
        {
            _blueScore.Variable.SetValue(0);
            _pinkScore.Variable.SetValue(0);
        }
        ///<summary> Activa o desactiva los arcos, dependiendo el valor de <paramref name="value"/> </summary>
        private void EnableGoals(bool value)
        {
            foreach (GameObject goal in _goals)
            {
                goal.SetActive(value);
            }
        }
        ///<summary> Activa o desactiva las paredes de los arcos, dependiendo el valor de <paramref name="value"/> </summary>
        private void EnableWalls(bool value)
        {
            foreach (GameObject goal in _walls)
            {
                goal.SetActive(value);
            }
        }
        ///<summary> Actualiza la variable <paramref name="_timer"> al tiempo restante de partida </summary>
        private void UpdateTimer()
        {
            float timer = _maxMatchTimer -= Time.deltaTime;

            if (timer <= 0 && !_didGameEnded)
            {
                _didGameEnded = true;
                EndGame();
                return;
            }

            float gameTimerWholeNums = Mathf.Floor(timer);
            float gameTimer = timer;

            gameTimer *= 100;
            gameTimer = Mathf.Floor(gameTimer);

            float gameTimerDecimals = gameTimer - (gameTimerWholeNums * 100);
            string extraNum = "";

            CheckTimer(gameTimerDecimals, extraNum);

            string textTimer = gameTimerWholeNums + "." + extraNum + gameTimerDecimals;

            _timer.Variable.Value = textTimer;            
        }
        ///<summary> Define el valor de <paramref name="extra"/> dependiendo <paramref name="decimals"/> </summary>
        private void CheckTimer(float decimals, string extra)
        {
            if (decimals < 10)
            {
                extra = "0";
                return;
            }
            extra = "";
        }
        ///<summary> Rota el mapa, definiendo su velocidad </summary>
        private void RotateMap()
        {
            float speedRot = _mapSpinSpeed.Value * Time.deltaTime * _mapRotationDirection;
            _map.transform.Rotate(0, 0, speedRot);
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
        ///<summary> Instancia fuegos artificiales </summary>
        ///<param name="position"> Posicion deseada para la instancia </param>
        ///<param name="multiColor"> Define si se modificara el color de las particulas </param>
        private void Fireworks(Vector3 position, bool multiColor)
        {
            GameObject particles = Instantiate(GetGoalParticles(), position, Quaternion.identity);
            if (multiColor && particles.TryGetComponent(out ParticleSystem particleSystem))
            {
                ParticleSystem.MainModule main = particleSystem.main;
                main.startColor = celebrationFireworksColors[Random.Range(0, celebrationFireworksColors.Length)];
            }
            Destroy(particles, 3);
        }
        ///<summary> Agrega un punto a la puntuacion de <paramref name="team"/> </summary>
        ///<param name="team"> El color de equipo quien hizo punto </param> 
        private void Point(Paint team)
        {
            switch((int)team)
            {
                case 1:
                    _pinkScore.Variable.ApplyChange(1);
                break;
                case 2:
                    _blueScore.Variable.ApplyChange(1);
                break;
            }
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
            
            _announcement.Variable.Value = EndGameText(winner);
            
            UIManager.Instance.EndGame(winner);

            StartCoroutine(EndGameBehaviour());
        }

        ///<summary> Acciona toda la logica de gol </summary>
        public void PlayerScore(Ball ball)
        {
            if (!_didGameEnded)
            {
                Fireworks(ball.transform.position, false);
                _goalRoutine = StartCoroutine(GoalBehaviour(ball));
            }
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

        private void Awake()
        {
            SetWalls();
            SetMapRotation();
            SetTimer();
            GetPostProcessSettings();
        }
        private void Start()
        {
            ResetPoint();
        }
        private void Update()
        {
            if (!_didGameEnded)
            {
                UpdateTimer();
            }
        }
        private void FixedUpdate()
        {
            if (!_didGameEnded)
            {
                RotateMap();
                UpdatePostProcessing();
            }
        }

    }
}