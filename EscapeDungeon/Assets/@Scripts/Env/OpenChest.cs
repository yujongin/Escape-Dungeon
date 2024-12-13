using UnityEngine;
public class OPenChest : MonoBehaviour
{
    public GameObject OpenText;
    private void Update()
    {
        if (Managers.Game.isAbleOpenChest)
        {
            OpenText.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        if (Managers.Game.isBossDead)
        {
            Managers.Game.isAbleOpenChest = true;
            OpenText.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        Managers.Game.isAbleOpenChest = false;
        OpenText.SetActive(false);
    }
}
