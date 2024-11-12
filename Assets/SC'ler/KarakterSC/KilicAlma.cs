using UnityEngine;

public class KilicAlma : MonoBehaviour
{
    private GameObject player;
    private GameObject currentSword;

    private void Start()
    {
        // Oyun baþladýðýnda Player objesini bul
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Kilic") && Input.GetKeyDown(KeyCode.E))
        {
            // E tuþuna basýldýðýnda ve Kilic tag'li bir nesneye temas edildiðinde
            if (player != null)
            {
                // Eðer Player'ýn elinde zaten bir kýlýç varsa, onu yok et
                if (currentSword != null)
                {
                    Destroy(currentSword);
                }

                // Kilic tag'li nesneyi Player'ýn eline al
                other.transform.parent = player.transform;
                other.transform.localPosition = Vector3.zero;
                other.transform.localRotation = Quaternion.identity;

                // Yerden alýnan kýlýcý kaydet
                currentSword = other.gameObject;
            }
        }
    }
}
