using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
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
        [SerializeField, Header("Parameters")] FloatReference _matchDuration;
        [SerializeField] FloatReference _mapSpinSpeed;
        [SerializeField] FloatReference _celebrationFireworks;
        [SerializeField] FloatReference _camShakeMagnitude;
        [SerializeField] IntReference _pinkScore;
        [SerializeField] IntReference _blueScore;
        [SerializeField] BooleanReference _enableGodWalls;
        [SerializeField, Space] Text gameTimerText;
        [SerializeField] Text player1ScoreText;
        [SerializeField] Text player2ScoreText;
        [SerializeField] GameObject pauseScreen;
        [SerializeField] GameObject map;
        [SerializeField] GameObject goalParticles;
        [SerializeField] GameObject announcementScore;
        [SerializeField] GameObject cam;
        [SerializeField] GameObject[] goalsToTurn;
        [SerializeField] GameObject[] walls;
        [SerializeField] Animator UICanvasAnim;
        [SerializeField] PostProcessVolume ppVolume;
        [SerializeField] Color[] celebrationFireworksColors;

        private Ball savedBall;
        private float mapRotDir;
        private Text announcementText;
        private bool updatingPP;
        private bool gameFinished;
        private bool gamePaused;
        private Coroutine goalRoutine;
        private Bloom ppBloom;
        private ChromaticAberration ppChromaticAberration;
        private LensDistortion ppLensDistort;

        private void Start()
        {
            bool enableGodWalls = _enableGodWalls.Value;
            TurnAllGoals(!enableGodWalls);
            TurnBackWall(enableGodWalls);

            mapRotDir = -1;
            announcementText = announcementScore.GetComponent<Text>();

            player1ScoreText.text = _pinkScore.Value.ToString();
            player2ScoreText.text = _blueScore.Value.ToString();

            ppVolume.profile.TryGetSettings(out ppBloom);
            ppVolume.profile.TryGetSettings(out ppChromaticAberration);
            ppVolume.profile.TryGetSettings(out ppLensDistort);
        }

        private void TurnAllGoals(bool value)
        {
            foreach (GameObject goal in goalsToTurn)
            {
                goal.SetActive(value);
            }
        }

        void TurnBackWall(bool value)
        {
            foreach (GameObject goal in walls)
            {
                goal.SetActive(value);
            }
        }

        private void Update()
        {
            if (!gameFinished)
            {
                UpdateTimer();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        private void FixedUpdate()
        {
            if (!gameFinished)
            {
                RotateMap();
                UpdatePostProcessing();
            }
        }

        void PauseGame()
        {
            pauseScreen.SetActive(true);
            gamePaused = true;
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            pauseScreen.SetActive(false);
            gamePaused = false;
            Time.timeScale = 1;
        }

        void RotateMap()
        {
            float speedRot = _mapSpinSpeed.Value * Time.deltaTime * mapRotDir;
            map.transform.Rotate(0, 0, speedRot);
        }

        public void PlayerScore(Ball ball)
        {
            if (!gameFinished)
            {
                if (savedBall == null)
                {
                    savedBall = ball;
                }
                Fireworks(ball.transform.position, false);
                goalRoutine = StartCoroutine(GoalRoutine(ball));
            }
        }

        void Fireworks(Vector3 position, bool multiColor)
        {
            GameObject particles = Instantiate(goalParticles, position, Quaternion.identity);
            if (multiColor && particles.TryGetComponent(out ParticleSystem particleSystem))
            {
                var main = particleSystem.main;
                main.startColor = celebrationFireworksColors[Random.Range(0, celebrationFireworksColors.Length)];
                // gParticles.GetComponent<ParticleSystem>().startColor = celebrationFireworksColors[Random.Range(0, celebrationFireworksColors.Length)];
            }
            Destroy(particles, 3);
        }
        void UpdatePostProcessing()
        {
            if (updatingPP)
            {
                ppBloom.intensity.value += 0.05f;
                ppChromaticAberration.intensity.value += 0.02f;
                ppLensDistort.intensity.value -= 0.7f;
                return;
            }
            if (ppBloom.intensity.value > 0.6f)
            {
                ppBloom.intensity.value -= 0.05f;
            }
            else if (ppBloom.intensity.value != 0.6f)
            {
                ppBloom.intensity.value = 0.6f;
            }
            if (ppChromaticAberration.intensity.value > 0)
            {
                ppChromaticAberration.intensity.value -= 0.03f;
            }
            else if (ppChromaticAberration.intensity.value != 0)
            {
                ppChromaticAberration.intensity.value = 0;
            }
            if (ppLensDistort.intensity.value < 0)
            {
                ppLensDistort.intensity.value += 0.7f;
            }
            else if (ppLensDistort.intensity.value != 0)
            {
                ppLensDistort.intensity.value = 0;
            }
        }

        IEnumerator GoalRoutine(Ball ball)
        {
            Goal(ball.Paint);
            UIManager.Instance.Goal(ball.Paint);

            StartCoroutine(CamShake(1f, _camShakeMagnitude.Value));
            ball.SetInvis();
            updatingPP = true;
            yield return new WaitForSeconds(0.5f);
            updatingPP = false;
            yield return new WaitForSeconds(1.5f);
            ppBloom.intensity.value = 0.6f;
            ppChromaticAberration.intensity.value = 0f;
            ppLensDistort.intensity.value = 0f;
            mapRotDir *= -1;
            ball.TurnBackOn();
            announcementScore.SetActive(false);
            yield return null;
        }
        private void Goal(Paint team)
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
        void UpdateTimer()
        {
            float matchDuration = _matchDuration.Value;
            
            matchDuration -= Time.deltaTime;
            if (matchDuration <= 0 && !gameFinished)
            {
                gameFinished = true;
                EndGameFunction();
                return;
            }
            float gameTimerWholeNums = Mathf.Floor(matchDuration);
            float gTimer = matchDuration;
            gTimer *= 100;
            gTimer = Mathf.Floor(gTimer);
            float gameTimerDecimals = gTimer - (gameTimerWholeNums * 100);
            string extraNum = "";
            if (gameTimerDecimals >= 10)
            {
                extraNum = "";
            }
            else
            {
                extraNum = "0";
            }
            _matchDuration.Variable.SetValue(matchDuration);
            gameTimerText.text = "Time: " + "\n" + gameTimerWholeNums + "." + extraNum + gameTimerDecimals;
        }

        void EndGameFunction()
        {
            int pinkScore = _pinkScore.Value;
            int blueScore = _blueScore.Value;
            Paint winner = pinkScore == blueScore ? Paint.White : pinkScore > blueScore ? Paint.Pink : Paint.Blue;

            if (goalRoutine != null)
            {
                StopCoroutine(goalRoutine);
                ppBloom.intensity.value = 0.6f;
                ppChromaticAberration.intensity.value = 0f;
                ppLensDistort.intensity.value = 0f;
            }

            UIManager.Instance.EndGame(winner);

            StartCoroutine(EndGameRoutine());
        }

        IEnumerator CamShake(float duration, float magnitude)
        {
            Vector3 originalPosition = cam.transform.localPosition;
            float elapsedTime = 0.0f;
            while (elapsedTime < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                cam.transform.localPosition = originalPosition + new Vector3(x, y, 0);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            cam.transform.localPosition = originalPosition;
            yield return null;
        }

        IEnumerator EndGameRoutine()
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
}
}