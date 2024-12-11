using UnityEngine;
using static Define;

public class Test_InputBinding : MonoBehaviour
{
    public InputBinding _binding = new InputBinding()
    {
        localDirectoryPath = @"Rito/2. Study/2021_0129_Input Binding/Presets",
        fileName = "InputBindingPreset",
        extName = "txt",
        id = "001"
    };

    private void Start()
    {
        _binding.LoadFromFile();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_binding.Bindings[UserAction.MoveLeft]))
        {
            LogBindingInfo(UserAction.MoveLeft);
        }
        if (Input.GetKeyDown(_binding.Bindings[UserAction.MoveRight]))
        {
            LogBindingInfo(UserAction.MoveRight);
        }
        if (Input.GetKeyDown(_binding.Bindings[UserAction.Attack]))
        {
            LogBindingInfo(UserAction.Attack);
        }
    }

    private void LogBindingInfo(UserAction action)
    {
        Debug.Log($"Action : {action}, KeyCode : {_binding.Bindings[action]}");
    }
}
