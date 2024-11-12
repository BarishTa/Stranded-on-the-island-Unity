using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI bile�enleri i�in

public class WrScripts : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 6.0f;
    public int karakterCan� = 500;
    public Transform handPosition;
    public GameObject currentSword;

    bool isGrounded;
    bool isAttack;
    public bool isDead = false; // public olarak de�i�tirildi

    private Rigidbody rb;
    private WrAnimation wrAnimation;
    private AttackAnim attackAnim;
    private GameObject nearbySword;

    public GameObject deathScreenCanvas; // �l�m ekran� Canvas

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wrAnimation = GetComponent<WrAnimation>();
        attackAnim = GetComponent<AttackAnim>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Ba�lang��ta fare i�aret�isi gizli

        if (rb == null)
        {
            Debug.LogError("Rigidbody bile�eni bulunamad�!");
        }

        if (wrAnimation == null)
        {
            Debug.LogError("WrAnimation bile�eni bulunamad�!");
        }

        if (attackAnim == null)
        {
            Debug.LogError("AttackAnim bile�eni bulunamad�!");
        }

        isAttack = false;

        if (deathScreenCanvas != null)
        {
            deathScreenCanvas.SetActive(false); // Ba�lang��ta �l�m ekran� devre d���
        }
        else
        {
            Debug.LogError("Death Screen Canvas bulunamad�!");
        }
    }

    void Update()
    {
        if (karakterCan� <= 0 && !isDead)
        {
            Die();
        }

        if (isDead)
        {
            return;
        }

        if (!isAttack && !Input.GetMouseButton(0))
        {
            float verticalInput = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.W))
            {
                Vector3 movement = transform.forward * verticalInput;
                rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);

                if (verticalInput != 0)
                {
                    wrAnimation.StartWalkingAnimation();
                }
                else
                {
                    wrAnimation.StopWalkingAnimation();
                }
            }
            else
            {
                wrAnimation.StopWalkingAnimation();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetMouseButton(0))
        {
            isAttack = true;
            wrAnimation.StopWalkingAnimation();
            attackAnim.PerformAttack();
        }
        else
        {
            isAttack = false;
        }

        if (nearbySword != null && Input.GetKeyDown(KeyCode.E))
        {
            PickupSword(nearbySword);
            Debug.Log("Colliderlar �arp���yor ve E tu�una bas�ld�. K�l�� yerden al�nd�.");
        }
        else if (nearbySword == null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E tu�una bas�ld� ancak yak�nda k�l�� yok.");
        }
    }

    public void Die()
    {
        isDead = true;
        wrAnimation.Die();

        if (deathScreenCanvas != null)
        {
            deathScreenCanvas.SetActive(true); // �l�m ekran�n� g�ster
            Cursor.lockState = CursorLockMode.None; // Fare i�aret�isini serbest b�rak
            Cursor.visible = true; // Fare i�aret�isini g�r�n�r yap
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.Locked; // Fare i�aret�isini tekrar kilitle
        Cursor.visible = false; // Fare i�aret�isini gizle
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kilic"))
        {
            nearbySword = other.gameObject;
            Debug.Log("K�l�� menzile girdi.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Kilic"))
        {
            nearbySword = null;
            Debug.Log("K�l�� menzilden ��kt�.");
        }
    }

    private void PickupSword(GameObject sword)
    {
        if (currentSword != null)
        {
            Destroy(currentSword);
            Debug.Log("Eski k�l�� yok edildi.");
        }

        currentSword = sword;
        sword.transform.SetParent(handPosition);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;

        Collider swordCollider = sword.GetComponent<Collider>();
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
        }
    }

    internal void AzalCan(int damageAmount)
    {
        throw new NotImplementedException();
    }
}
