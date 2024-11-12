using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnim : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing on Boss!");
        }
    }

    public void StartWalking()
    {
        animator.SetBool("BossWalk", true);
        animator.SetBool("BossAttack", false);
    }

    public void StartAttacking()
    {
        animator.SetBool("BossWalk", false);
        animator.SetBool("BossAttack", true);
    }

    public void StopAttacking()
    {
        animator.SetBool("BossAttack", false);
    }

    public void StartDying()
    {
        animator.SetBool("BossDie", true);
        animator.SetBool("BossWalk", false);
        animator.SetBool("BossAttack", false);
    }
}
