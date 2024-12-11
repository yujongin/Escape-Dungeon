using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using static Define;
public class GameManager : MonoBehaviour
{
    public bool isStart = false;
    public bool isStart2 = false;
    public bool isClear = false;
    public bool isGetKey= false;
    public bool isBossDead= false;
    public bool isAbleOpenDoor = false;
    public bool isAbleOpenChest = false;
    public GameObject GameOverPanel;
    public GameObject ClearPanel;
    public GameObject MenuPanel;


    public int EnemyCount = 5;
    public BaseInteractionObject InteractionObject;
    public GameObject Door;
    public GameObject Key;
    public GameObject Chest;


    Define.EGameState gameState;
    public void EnemyDead()
    {
        EnemyCount--;
        if(EnemyCount == 0)
        {
            Key.SetActive(true);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MenuPanel.SetActive(!MenuPanel.activeSelf);
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            if(InteractionObject != null)
            {
                InteractionObject.Operate();
            }

            //if (isAbleOpenDoor)
            //{
            //    Door.transform.DORotate(new Vector3(0, 90, 0), 1f);
            //    Managers.Game.isStart2 = true;
            //    isAbleOpenDoor = false;
            //    Door.GetComponent<OpenDoor>().OpenText.SetActive(false);
            //}
            if (isAbleOpenChest)
            {
                Chest.transform.DOLocalRotate(new Vector3(-90, 0, 0), 1f);
                Managers.Game.EnableClearPanel();
                isAbleOpenChest = false;
                Chest.GetComponent<OPenChest>().OpenText.SetActive(false);
            }
        }
    }


    public void EnableGameOverPanel() 
    {
        GameOverPanel.SetActive(true);
    }
    public void EnableClearPanel() 
    {
        isClear = true;
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
