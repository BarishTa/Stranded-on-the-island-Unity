using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WrHealthSlider : MonoBehaviour
{
    public Slider healthSlider; // Saðlýk slider'ý referansý
    public Image fillImage; // Slider'ýn fill image referansý
    public Color fullColor = Color.green; // Tam dolu renk
    public Color emptyColor = Color.red; // Boþ renk
    public TextMeshProUGUI healthText; // Karakterin canýný gösteren TMP bileþeni

    private WrScripts playerScript; // Karakterin scripti referansý

    void Start()
    {
        playerScript = FindObjectOfType<WrScripts>(); // WrScripts scriptini bul
        healthSlider.maxValue = playerScript.karakterCaný; // Slider'ýn maksimum deðerini karakterin canýna ayarla
        healthSlider.value = playerScript.karakterCaný; // Slider'ýn baþlangýç deðerini ayarla

        UpdateSliderColor();
        UpdateHealthText();
    }

    void Update()
    {
        healthSlider.value = playerScript.karakterCaný; // Slider deðerini güncelle
        UpdateSliderColor(); // Saðlýk rengi geçiþini güncelle
        UpdateHealthText(); // Saðlýk metnini güncelle
    }

    private void UpdateSliderColor()
    {
        // Slider doluluk rengini güncelle
        fillImage.color = Color.Lerp(emptyColor, fullColor, healthSlider.value / healthSlider.maxValue);
    }

    private void UpdateHealthText()
    {
        // Saðlýk metnini güncelle
        healthText.text = playerScript.karakterCaný.ToString();
    }
}
