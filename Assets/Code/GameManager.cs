using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text gameTimerText;
    [SerializeField] Text player1ScoreText;
    [SerializeField] Text player2ScoreText;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] float gameTimer;
    [SerializeField] GameObject map;
    [SerializeField] float mapSpinSpeed;
    [SerializeField] GameObject goalParticles;
    [SerializeField] GameObject announcementScore;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject[] goalsToTurn;
    [SerializeField] GameObject[] walls;
    [SerializeField] Animator UICanvasAnim;
    [SerializeField] PostProcessVolume ppVolume;
    [SerializeField] float celebrationFireworks;
    [SerializeField] Color[] celebrationFireworksColors;
    [SerializeField] float camShakeMagnitude;

    Ball savedBall;
    float mapRotDir;
    Text announcementText;
    int player1Score;
    int player2Score;
    bool updatingPP;
    bool gameFinished;
    bool gamePaused;
    Coroutine goalRoutine;
    Bloom ppBloom;
    ChromaticAberration ppChromaticAberration;
    LensDistortion ppLensDistort;

    private void Start()
    {
        if (PlayerPrefs.HasKey("matchDuration"))
        {
            gameTimer = PlayerPrefs.GetFloat("matchDuration");
        }
        if (PlayerPrefs.HasKey("mapSpinSpeed"))
        {
            mapSpinSpeed = PlayerPrefs.GetFloat("mapSpinSpeed");
        }
        if (PlayerPrefs.HasKey("celebrationFireworks"))
        {
            celebrationFireworks = PlayerPrefs.GetFloat("celebrationFireworks");
        }
        if (PlayerPrefs.HasKey("camShakeMagnitude"))
        {
            camShakeMagnitude = PlayerPrefs.GetFloat("camShakeMagnitude");
        }
        if (PlayerPrefs.HasKey("goalsBackWall"))
        {
            int goalBackWallBool = PlayerPrefs.GetInt("goalsBackWall");
            if (goalBackWallBool == 0)
            {
                TurnAllGoals(true);
                TurnBackWall(false);
            }
            else if (goalBackWallBool == 1)
            {
                TurnAllGoals(false);
                TurnBackWall(true);
            }
        }
        mapRotDir = -1;
        announcementText = announcementScore.GetComponent<Text>();
        player1ScoreText.text = "P1:     " + player1Score;
        player2ScoreText.text = "P2:     " + player2Score;
        ppVolume.profile.TryGetSettings(out ppBloom);
        ppVolume.profile.TryGetSettings(out ppChromaticAberration);
        ppVolume.profile.TryGetSettings(out ppLensDistort);
    }

    void TurnAllGoals(bool onOrOff)
    {
        foreach (GameObject goal in goalsToTurn)
        {
            goal.SetActive(onOrOff);
        }
    }

    void TurnBackWall(bool onOrOff)
    {
        foreach (GameObject goal in walls)
        {
            goal.SetActive(onOrOff);
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
        float speedRot = mapSpinSpeed * Time.deltaTime * mapRotDir;
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
        if (ball.paint == BallPaint.pink)
        {
            announcementText.text = "Point for Player 1";
            announcementText.color = ball.playerMaterials[0].color;
            UICanvasAnim.SetTrigger("player1Score");
            player1Score++;
            player1ScoreText.text = "P1:     " + player1Score;
        }
        else if (ball.paint == BallPaint.blue)
        {
            announcementText.text = "Point for Player 2";
            announcementText.color = ball.playerMaterials[1].color;
            UICanvasAnim.SetTrigger("player2Score");
            player2Score++;
            player2ScoreText.text = "P2:     " + player2Score;
        }
        announcementScore.SetActive(true);
        StartCoroutine(CamShake(1f, camShakeMagnitude));
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
        UICanvasAnim.SetTrigger("Back");
        yield return null;
    }


    void UpdateTimer()
    {
        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0 && !gameFinished)
        {
            gameFinished = true;
            EndGameFunction();
            return;
        }
        float gameTimerWholeNums = Mathf.Floor(gameTimer);
        float gTimer = gameTimer;
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
        gameTimerText.text = "Time: " + "\n" + gameTimerWholeNums + "." + extraNum + gameTimerDecimals;
    }

    void EndGameFunction()
    {
        if (goalRoutine != null)
        {
            StopCoroutine(goalRoutine);
            ppBloom.intensity.value = 0.6f;
            ppChromaticAberration.intensity.value = 0f;
            ppLensDistort.intensity.value = 0f;
        }
        if (player1Score > player2Score)
        {
            announcementText.text = "Player 1 wins the game";
            announcementText.color = savedBall.playerMaterials[0].color;
            UICanvasAnim.SetTrigger("player1Score");
        }
        else if (player1Score < player2Score)
        {
            announcementText.text = "Player 2 wins the game";
            announcementText.color = savedBall.playerMaterials[1].color;
            UICanvasAnim.SetTrigger("player2Score");
        }
        else if (player1Score == player2Score)
        {
            announcementText.text = "The game ends in a tie!";
            announcementText.color = Color.white;
        }
        announcementScore.SetActive(true);
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