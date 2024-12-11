using UnityEngine;
using DG.Tweening;
public class OpenDoor : MonoBehaviour
{
    public GameObject OpenText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        if (Managers.Game.isGetKey)
        {
            Managers.Game.isAbleOpenDoor = true;
            OpenText.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        Managers.Game.isAbleOpenDoor = false;
        OpenText.SetActive(false);
    }

}
