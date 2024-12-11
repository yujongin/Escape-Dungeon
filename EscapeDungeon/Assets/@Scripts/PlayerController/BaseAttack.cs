using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    public string AttackTag;
    public AudioSource AudioSource;
    public AudioClip SwordAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == AttackTag)
        {
            Debug.Log("hit Enemy");
            other.GetComponent<BaseObject>().OnDamaged();
            if (AudioSource != null)
                AudioSource.PlayOneShot(SwordAttack);
        }
    }
}
