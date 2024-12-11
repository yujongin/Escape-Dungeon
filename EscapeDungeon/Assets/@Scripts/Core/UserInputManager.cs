using System;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class UserInputManager : MonoBehaviour
{
    public InputBindingManager InputBindingManager;
    public InputBinding binding;
    public event Action<float,float> OnMoveEvent = null;
    public event Action OnAttackEvent = null;
    public event Action OnDashEvent = null;
    public event Action OnPotionEvent = null;
    void Start()
    {
        InputBindingManager = GetComponent<InputBindingManager>();
        binding = InputBindingManager._binding;

    }

    void Update()
    {
        float dirX;
        float dirY;
        if (Input.GetKeyDown(binding.Bindings[UserAction.MoveLeft]) || Input.GetKeyDown(binding.Bindings[UserAction.MoveRight])
            || Input.GetKeyDown(binding.Bindings[UserAction.MoveForward]) || Input.GetKeyDown(binding.Bindings[UserAction.MoveBackward]))
        {
            dirX = Input.GetAxisRaw("Horizontal");
            dirY = Input.GetAxisRaw("Vertical");

            //OnMoveEvent.Invoke(dirX, dirY);
        }

        //if (Input.GetKeyDown(binding.Bindings[UserAction.Attack]))
        //{
        //    OnAttackEvent.Invoke();
        //}        
        //if (Input.GetKeyDown(binding.Bindings[UserAction.Dash]))
        //{
        //    OnDashEvent.Invoke();
        //}        
        //if (Input.GetKeyDown(binding.Bindings[UserAction.Postion]))
        //{
        //    OnPotionEvent.Invoke();
        //}
    }
}
