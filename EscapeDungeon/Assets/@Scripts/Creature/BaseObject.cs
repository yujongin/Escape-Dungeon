using System.Collections;
using UnityEngine;
using static Define;
public class BaseObject : InitBase
{
    public EObjectType ObjectType { get; protected set; } = EObjectType.None;
    public Collider Collider { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public AudioSource AudioSource { get; private set; }

    public Material MeshMaterial;
    //맞았을 때 이펙트
    //private HurtFlashEffect HurtFlash;
    [HideInInspector]
    public bool isDamaged = false;
    [HideInInspector]
    public bool isDead = false;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider = GetComponent<Collider>();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        AudioSource = GetComponent<AudioSource>();

        return true;
    }

    public static Vector3 GetLookAtRotation(Vector3 dir)
    {
        float deg = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        return new Vector3(0, deg, 0);
    }
    #region Battle
    public virtual void OnDamaged()
    {
        //HurtFlash.Flash();
        if (!isDead)
        {
            StartCoroutine(DamagedAnimation());
        }
    }

    public virtual void OnDead()
    {
        isDead = true;
        Animator.SetTrigger("Death");
    }
    #endregion

    IEnumerator DamagedAnimation()
    {
        isDamaged = true;
        MeshMaterial.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        MeshMaterial.color = Color.white;

        isDamaged = false;
    }
}
