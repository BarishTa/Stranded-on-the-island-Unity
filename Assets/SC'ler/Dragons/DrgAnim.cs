using UnityEngine;

public class DrgAnim : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWalk(bool isWalking)
    {
        animator.SetBool("DrgWalk", isWalking);
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("DrgAttack");
    }

    public void SetDie(bool isDying)
    {
        animator.SetBool("DrgDie", isDying);
    }
}
