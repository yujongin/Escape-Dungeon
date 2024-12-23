using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;
public class GameManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public GameObject ClearPanel;
    public GameObject MenuPanel;

    public int enemyCount { get; private set; } = 0;
    public BaseInteractionObject InteractionObject;
    public GameObject Key;

    public Define.EGameState gameState { get; private set; }
    private void Start()
    {
        gameState = EGameState.None;
    }
    public void EnemyDead()
    {
        enemyCount--;
        if (enemyCount == 0)
        {
            Key.SetActive(true);
        }
    }
    public void AddEnemyCount()
    {
        enemyCount++;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MenuPanel.SetActive(!MenuPanel.activeSelf);
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (InteractionObject != null)
            {
                InteractionObject.Operate();
            }
        }
    }

    public void ChangeNextState()
    {
        gameState++;
    }

    public void EnableGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
    public void EnableClearPanel()
    {
        ClearPanel.SetActive(true);
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
