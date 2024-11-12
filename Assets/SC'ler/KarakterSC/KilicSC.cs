using UnityEngine;

public class KilicSC : MonoBehaviour
{
    // Düþmanlarýn canýný azaltacak miktar
    public int azalacakCanMiktari = 50;

    // Kýlýcýn collider'ý bir þeye çarparsa bu fonksiyon çaðrýlýr
    private void OnTriggerEnter(Collider other)
    {
        // Bu nesnenin tag'ý "Kilic" ise
        if (gameObject.CompareTag("Kilic"))
        {
            // Çarpýlan nesne "Enemy" tag'ine sahipse
            if (other.CompareTag("Enemy"))
            {
                // Çarpýlan nesnenin "SpiderSC" script'ini direkt olarak al
                SpiderSC spider = other.GetComponent<SpiderSC>();

                // Eðer çarpýlan nesne bir örümcek ise ve örümcek script'i varsa
                if (spider != null)
                {
                    // Örümceðin canýný azalt
                    spider.AzalCan(azalacakCanMiktari);

                    // Örümceðin yeni canýný debug log ile göster
                    Debug.Log("Örümceðin caný azaldý! Yeni can: " + spider.orumcekCaný);
                }
            }
            // Çarpýlan nesne "Boss" tag'ine sahipse
            else if (other.CompareTag("Boss"))
            {
                // Çarpýlan nesnenin "BossSc" script'ini direkt olarak al
                BossSc boss = other.GetComponent<BossSc>();

                // Eðer çarpýlan nesne bir boss ise ve boss script'i varsa
                if (boss != null)
                {
                    // Boss'un canýný azalt
                    boss.AzalCan(azalacakCanMiktari);

                    // Boss'un yeni canýný debug log ile göster
                    Debug.Log("Boss'un caný azaldý! Yeni can: " + boss.bossCaný);
                }
            }
        }
    }
}
