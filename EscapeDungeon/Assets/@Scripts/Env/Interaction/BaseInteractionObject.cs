using TMPro;
using UnityEngine;

public abstract class BaseInteractionObject : InitBase
{
    public bool IsOperatable { get; private set; }
    public GameObject OpenText;
    public abstract void Operate();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        OpenText = GetComponentInChildren<TMP_Text>().gameObject;
        OpenText.SetActive(false);
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //add interaction object in manager
        if (IsOperatable)
        {
            OpenText.SetActive(true);
            Managers.Game.InteractionObject = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        OpenText.SetActive(false);
        Managers.Game.InteractionObject = null;
    }

    public void ChangeOperateState(bool state)
    {
        IsOperatable = state;
    }
}
