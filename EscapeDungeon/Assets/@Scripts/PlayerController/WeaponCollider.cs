using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public Collider Collider;
    void OnColiderEnable()
    {
        Collider.enabled = true;
    }
    void OnColiderDisable()
    {
        Collider.enabled = false;
    }
}
