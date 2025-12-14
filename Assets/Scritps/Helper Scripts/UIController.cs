using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class UIController : MonoBehaviour
{
    public static UIController Instance;

    private ProgressBar healthBar;
    private Label playerScore;
    private Label enemyScore;

    private VisualElement WinContainer;
    private Label WinText;
    private Label NewGameText;

    private int playerScoreValue = 0;
    private int enemyScoreValue = 0;


    private int currentRound = 0;
    private const int maxRounds = 5;

    [SerializeField] private Character player;
    [SerializeField] private Character enemy;


    private void Awake()
    {
        Instance = this;

        var root = GetComponent<UIDocument>().rootVisualElement;

        healthBar = root.Q<ProgressBar>("HealthBar");
        playerScore = root.Q<Label>("PlayerScore");
        enemyScore = root.Q<Label>("EnemyScore");


        WinContainer = root.Q<VisualElement>("WinContainer");
        WinText = root.Q<Label>("WinText");
        NewGameText = root.Q<Label>("NewGameText");

        WinContainer.style.display = DisplayStyle.None;

        healthBar.value = 100f;
        healthBar.title = "100%";
    }

    public void UpdateHealth(int current, int max)
    {
        float percent = (float)current / max * 100f;

        healthBar.value = percent;
        healthBar.title = Mathf.RoundToInt(percent) + "%";
    }

    public void IncreasePlayerScore()
    {
        playerScoreValue++;
        playerScore.text = playerScoreValue.ToString();
    }

    public void IncreaseEnemyScore()
    {
        enemyScoreValue++;
        enemyScore.text = enemyScoreValue.ToString();
    }

    public void OnRoundEnd(Character deadCharacter)
    {
        currentRound++;

        if (currentRound < maxRounds)
        {
            StartCoroutine(RespawnAfterDelay(deadCharacter, 1.5f));
        }
        else
        {
            DecideWinner();
        }
    }


    private void Update()
    {
        if (WinContainer.style.display == DisplayStyle.Flex && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RestartAfterDelay(1f));
        }
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        RestartGame();
    }

    private IEnumerator RespawnAfterDelay(Character deadCharacter, float delay)
    {
        yield return new WaitForSeconds(delay);

        deadCharacter.Respawn();
    }

    private void DecideWinner()
    {
        WinContainer.style.display = DisplayStyle.Flex;

        if (playerScoreValue > enemyScoreValue)
        {
            WinText.text = "PLAYER WINS 🏆";
        }
        else if (enemyScoreValue > playerScoreValue)
        {
            WinText.text = "ENEMY WINS 🏆";
        }
        else
        {
            WinText.text = "MATCH DRAW 🤝";
        }

        NewGameText.text =
            $"Player Score is {playerScoreValue} | Enemy Score is {enemyScoreValue}\nPress R for New Game";

        Time.timeScale = 0f;
    }
    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}