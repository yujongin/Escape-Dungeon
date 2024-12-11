using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Creature
{
    private Rigidbody rb;
    private Vector2 dirVector;
    private Vector2 oldDirVector;

    Vector3 move = new Vector3();
    Vector3 moveVector = new Vector3();
    Coroutine lerpVectorCoroutine;
    Coroutine attackCoroutine;

    Animator animator;

    [SerializeField] float dashForce = 30f;  // ��� ��
    [SerializeField] float dashDuration = 0.2f; // ��� ���� �ð�
    [SerializeField] float dashCoolDown = 1f; // ��� ���� �ð�

    public bool isDashing = false; // ��� ���� Ȯ��
    public Slider HPBar;
    bool isMove = true;

    bool isAttacking = false;                 // ���� �� ����
    bool isComboEnable = false;
    bool isComboExist = false;
    float attackCheck = 1;

    int potionCount = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        dirVector = Vector2.zero;
        Hp = MaxHp;
        HPBar.value = Hp / MaxHp;
    }
    void Update()
    {
        if (isDeath || Managers.Game.isClear) return;
        HandleMovement();

        //Attack
        UpdateAttacking();
    }

    #region Move and Dash
    IEnumerator Dash()
    {
        if (move.magnitude == 0) yield break; // ��� ������ ������ �������� ����

        isDashing = true;
        isMove = false; // ��� �� �̵� �Ұ�

        // ��� �������� ����
        Vector3 dashDirection = move.normalized;
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);

        // ��� ���� �� �̵� �簳
        isMove = true;

        yield return new WaitForSeconds(dashCoolDown);
        isDashing = false;
    }
    IEnumerator LerpMoveVector()
    {
        Vector3 targetVec = new Vector3(dirVector.x, 0, dirVector.y);
        Vector3 startVec = move;

        float t = 0f;
        while (move != targetVec)
        {
            t += Time.deltaTime * 3;
            move = Vector3.Lerp(startVec, targetVec, t);
            yield return null;
        }
    }
    private void HandleMovement()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");
        oldDirVector = dirVector;
        dirVector.Set(dirX, dirY);
        dirVector = Vector2.ClampMagnitude(dirVector, 1f);

        if (oldDirVector != dirVector)
        {
            if (lerpVectorCoroutine != null)
            {
                StopCoroutine(lerpVectorCoroutine);
            }
            lerpVectorCoroutine = StartCoroutine(LerpMoveVector());
        }

        //move
        if (move.magnitude != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //TODO : ���� ����� �� �ٶ󺸵���
                CheckDetectEnemy("Enemy");
                if (Target != null && !isAttacking)
                {
                    LookAtTarget(Target.position);
                }

                Vector3 worldPos = new Vector3(transform.position.x + move.x, 0, transform.position.z + move.z);
                Vector3 localPos = transform.InverseTransformPoint(worldPos);
                animator.SetBool("ShiftRun", true);
                animator.SetFloat("BlendX", localPos.x);
                animator.SetFloat("BlendY", 0);
            }
            else
            {
                if (!isAttacking)
                {
                    Vector3 eulerAngle = GetLookAtRotation(move);
                    transform.localRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, eulerAngle.y, 0), RotateSpeed * Time.deltaTime);
                }
                animator.SetBool("ShiftRun", false);
                animator.SetFloat("BlendX", 0);
                animator.SetFloat("BlendY", move.magnitude);
            }

            //Dash
            if (Input.GetKeyDown(KeyCode.Space) && !isDashing && !isAttacking)
            {
                StartCoroutine(Dash());
            }
        }
        else
        {
            animator.SetBool("ShiftRun", false);
            animator.SetFloat("BlendX", move.magnitude);
            animator.SetFloat("BlendY", move.magnitude);
        }

        if (isMove)
        {
            moveVector = move * MoveSpeed * attackCheck;
            rb.linearVelocity = new Vector3(moveVector.x, rb.linearVelocity.y, moveVector.z);
        }
    }

    #endregion
    #region Combo Attack
    private void UpdateAttacking()
    {
        if (Input.GetKeyDown(KeyCode.J) == false)
            return;

        attackCheck = 0;
        moveVector = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        if (isComboEnable)   // �Է��� �Ǹ� ����, ������ ����
        {
            isComboEnable = false;
            isComboExist = true;

            return;
        }

        if (isAttacking == true)
            return;

        //AudioSource.PlayOneShot(SwordSwing);
        isAttacking = true;
        animator.SetBool("IsAttacking", isAttacking);
    }

    private void EnableCombo()
    {
        isComboEnable = true;
        isComboExist = false;
    }

    private void DisableCombo()
    {
        isComboEnable = false;
    }

    private void ExistCombo()
    {
        if (isComboExist == false)
            return;

        animator.SetTrigger("NextCombo");
    }

    private void DisableExistCombo()
    {
        isComboExist = false;
    }
    private void EndAttack()
    {
        if (!isComboExist)
        {
            isAttacking = false;
            animator.SetBool("IsAttacking", isAttacking);
            attackCheck = 1;
        }
    }

    public override void OnDamaged()
    {
        if (!isDamaged)
        {
            base.OnDamaged();
            Hp -= 3f;
            HPBar.value = Hp / MaxHp;
            if (Hp <= 0)
            {
                OnDead();
                Managers.Game.isStart = false;
                Managers.Game.EnableGameOverPanel();
                isDeath = true;
            }
        }
    }
    #endregion
}
