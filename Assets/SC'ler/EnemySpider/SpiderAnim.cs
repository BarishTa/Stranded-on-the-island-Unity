using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnim : MonoBehaviour
{
    private Animator animator;
    private SpiderSC spiderSC;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing on Spider!");
        }

        spiderSC = GetComponent<SpiderSC>();
        if (spiderSC == null)
        {
            Debug.LogError("SpiderSC script missing on Spider!");
        }
    }

    public void StartWalking()
    {
        animator.SetTrigger("SpdWalk");
    }

    public void StartAttacking()
    {
        animator.SetTrigger("SpdAttack");
    }

    public void StopWalking()
    {
        animator.ResetTrigger("SpdWalk");
    }

    public void StopAttacking()
    {
        animator.ResetTrigger("SpdAttack");
    }

    public void StartDying()
    {
        animator.SetBool("SpdDeath", true);
        StartCoroutine(DestroyAfterDeathAnimation());
    }

    private IEnumerator DestroyAfterDeathAnimation()
    {
        // Ölüm animasyonu oynarken 10 saniye bekle
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    public void ResumeFollowing()
    {
        if (spiderSC != null && spiderSC.orumcekCaný > 0)
        {
            spiderSC.ResumeFollowing();
        }
    }
}
