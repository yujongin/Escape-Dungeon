using UnityEngine;

public class OpenKey : MonoBehaviour
{
    public BaseInteractionObject Door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Door.ChangeOperateState(true);
            gameObject.SetActive(false);
        }
    }
}
