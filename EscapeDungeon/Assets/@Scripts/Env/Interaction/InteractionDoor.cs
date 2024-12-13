using UnityEngine;
using DG.Tweening;
public class InteractionDoor : BaseInteractionObject
{
    public override void Operate()
    {
        transform.DORotate(new Vector3(0, 90, 0), 1f);
        Managers.Game.isStart2 = true;
        ChangeOperateState(false);
        OpenText.SetActive(false);
    }
}
