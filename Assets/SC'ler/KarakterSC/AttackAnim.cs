using UnityEngine;

public class AttackAnim : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator bileþeni bulunamadý!");
        }
    }

    public void PerformAttack()
    {
        animator.SetTrigger("Attack");
      
    }
}
