using UnityEngine;

public class AttackAnim : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator bile�eni bulunamad�!");
        }
    }

    public void PerformAttack()
    {
        animator.SetTrigger("Attack");
      
    }
}
