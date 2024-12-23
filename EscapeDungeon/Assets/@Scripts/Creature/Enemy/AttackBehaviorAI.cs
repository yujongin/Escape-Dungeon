using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviorAI : Creature
{

    Vector3 originPosition;
    BehaviorTreeRunner btRunner;

    public AudioClip DeadClip;
    public bool isBoss;
    private void Start()
    {
        originPosition = transform.position;

        btRunner = new BehaviorTreeRunner(SettingAttackBT());
        Hp = MaxHp;
        if (gameObject.CompareTag("Enemy") && !isBoss)
        {
            Managers.Game.AddEnemyCount();
        }
    }

    private void Update()
    {
        if (!isDead)
        {
            if (Managers.Game.gameState == Define.EGameState.Room1 && gameObject.CompareTag("Enemy"))
            {
                if (!isBoss)
                {
                    btRunner.Operate();
                }
            }
            else if (Managers.Game.gameState == Define.EGameState.Room2 && isBoss)
            {
                btRunner.Operate();
            }
        }
    }

    INode SettingAttackBT()
    {
        INode root = new SelectorNode(new List<INode> {
            new SequenceNode(new List<INode>() {
                new ActionNode(CheckIsAttacking), new ActionNode(CheckInAttackRange), new ActionNode(DoAttack) }), new SequenceNode(new List<INode>() {
                    new ActionNode(CheckDetectEnemy), new ActionNode(MoveToDetectedEnemy) }), new ActionNode(ReturnToOrigin) });
        return root;
    }

    NodeState CheckIsAttacking()
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return NodeState.Running;
        }
        return NodeState.Success;
    }

    NodeState CheckInAttackRange()
    {
        if (Target != null)
        {
            if (Vector3.Magnitude(Target.position - transform.position) < AttackRange)
            {
                base.Animator.SetBool("Running", false);
                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }

    NodeState DoAttack()
    {
        if (Target != null && !Target.GetComponent<Creature>().isDead)
        {
            Animator.SetTrigger("Attack");
            return NodeState.Success;
        }

        return NodeState.Failure;
    }

    NodeState CheckDetectEnemy()
    {
        Collider[] overlaps = Physics.OverlapSphere(transform.position, DetectRange, LayerMask.GetMask("Player"));
        if (overlaps != null && overlaps.Length > 0)
        {
            Target = overlaps[0].transform;
            if (!Target.GetComponent<Creature>().isDead)
                return NodeState.Success;
        }

        Target = null;
        return NodeState.Failure;
    }

    NodeState MoveToDetectedEnemy()
    {
        if (Target != null)
        {
            if (Vector3.Distance(Target.position, transform.position) < AttackRange)
            {
                return NodeState.Success;
            }
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Time.deltaTime * MoveSpeed);
            base.Animator.SetBool("Running", true);
            LookAtTarget(Target.position);
            return NodeState.Running;
        }

        return NodeState.Failure;
    }

    NodeState ReturnToOrigin()
    {
        if (Vector3.Distance(originPosition, transform.position) < float.Epsilon)
        {
            base.Animator.SetBool("Running", false);
            return NodeState.Success;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, originPosition, Time.deltaTime * MoveSpeed);
            LookAtTarget(originPosition);
            base.Animator.SetBool("Running", true);
            return NodeState.Running;
        }
    }

    public override void OnDamaged()
    {
        if (!isDamaged && !isDead)
        {
            base.OnDamaged();
            Animator.SetTrigger("Hit");
            if (Hp <= 0)
            {
                OnDead();
                AudioSource.PlayOneShot(DeadClip);
                isDead = true;
                Managers.Game.EnemyDead();
                Destroy(gameObject, 3f);
            }
            else
            {
                Hp -= 5f;
            }
        }
    }

    public override void OnDead()
    {
        base.OnDead();
        if (isBoss)
        {
            Managers.Game.ChangeNextState();
        }
    }

}
