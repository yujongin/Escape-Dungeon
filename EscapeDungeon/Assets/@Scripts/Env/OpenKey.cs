using UnityEngine;

public class OpenKey : MonoBehaviour
{
    BaseInteractionObject door;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            door.IsOperatable = true;
            gameObject.SetActive(false);
        }
    }
}
