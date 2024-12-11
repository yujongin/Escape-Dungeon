using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    public static Managers Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private UserInputManager _userInput;
    private GameManager _game;

    public static UserInputManager UserInput { get { return Instance?._userInput; } }
    public static GameManager Game { get { return Instance?._game; } }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _userInput = GetComponentInChildren<UserInputManager>();
        _game = GetComponentInChildren<GameManager>();
    }
    

}
