using UnityEngine;

public class WrAnimation : MonoBehaviour
{
    private Animator animator; // Animator bileþeni
    private bool isWalking = false; // Yürüme durumu
    private bool isDead = false; // Ölüm durumu

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator bileþenini al
    }

    void Update()
    {
        // "isWalking" ve "isDead" deðerine göre animasyonu oynat veya durdur
        if (isDead)
        {
            animator.SetBool("Die", true);
        }
        else
        {
            animator.SetBool("Walk", isWalking);
        }
    }

    // Yürüme animasyonunu baþlatan fonksiyon
    public void StartWalkingAnimation()
    {
        if (!isDead)
        {
            isWalking = true;
        }
    }

    // Yürüme animasyonunu durduran fonksiyon
    public void StopWalkingAnimation()
    {
        if (!isDead)
        {
            isWalking = false;
        }
    }

    // Ölüm animasyonunu baþlatan fonksiyon
    public void Die()
    {
        isDead = true;
        isWalking = false;
        animator.SetBool("Die", true);
    }
}
