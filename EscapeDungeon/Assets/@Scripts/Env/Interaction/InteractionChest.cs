using UnityEngine;
using DG.Tweening;
public class InteractionChest : BaseInteractionObject
{
    public override void Operate()
    {
        transform.DOLocalRotate(new Vector3(-90, 0, 0), 1f);
        Managers.Game.EnableClearPanel();
        ChangeOperateState(false);
        OpenText.SetActive(false);
    }
}
