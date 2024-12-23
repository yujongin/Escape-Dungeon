using UnityEngine;
using static Define;

public class Creature : BaseObject
{
    #region Stats
    public float Hp { get; set; }
    public float MaxHp;
    public float Atk;
    public float MoveSpeed;
    public float RotateSpeed;
    public float DetectRange;
    public float AttackRange;
    #endregion
    public Transform Target;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Creature;

        return true;
    }
    public void CheckDetectEnemy(string CheckMask)
    {
        Collider[] overlaps = Physics.OverlapSphere(transform.position, DetectRange, LayerMask.GetMask(CheckMask));
        if (overlaps != null && overlaps.Length > 0)
        {
            Target = overlaps[0].transform;
        }
        else
        {
            Target = null;
        }
    }
    public void LookAtTarget(Vector3 targetPos)
    {
        Vector3 lookVec = targetPos - transform.position;
        lookVec.Set(lookVec.x, 0, lookVec.z);
        Quaternion targetRotation = Quaternion.LookRotation(lookVec);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DetectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

    }
}
