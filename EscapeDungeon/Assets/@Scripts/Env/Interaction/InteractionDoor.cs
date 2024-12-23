using UnityEngine;
using DG.Tweening;
public class InteractionDoor : BaseInteractionObject
{
    public override void Operate()
    {
        transform.DORotate(new Vector3(0, 90, 0), 1f);
        Managers.Game.ChangeNextState();
        ChangeOperateState(false);
        OpenText.SetActive(false);
    }
}
