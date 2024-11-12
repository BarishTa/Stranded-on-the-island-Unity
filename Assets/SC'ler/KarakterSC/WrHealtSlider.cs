using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WrHealthSlider : MonoBehaviour
{
    public Slider healthSlider; // Sa�l�k slider'� referans�
    public Image fillImage; // Slider'�n fill image referans�
    public Color fullColor = Color.green; // Tam dolu renk
    public Color emptyColor = Color.red; // Bo� renk
    public TextMeshProUGUI healthText; // Karakterin can�n� g�steren TMP bile�eni

    private WrScripts playerScript; // Karakterin scripti referans�

    void Start()
    {
        playerScript = FindObjectOfType<WrScripts>(); // WrScripts scriptini bul
        healthSlider.maxValue = playerScript.karakterCan�; // Slider'�n maksimum de�erini karakterin can�na ayarla
        healthSlider.value = playerScript.karakterCan�; // Slider'�n ba�lang�� de�erini ayarla

        UpdateSliderColor();
        UpdateHealthText();
    }

    void Update()
    {
        healthSlider.value = playerScript.karakterCan�; // Slider de�erini g�ncelle
        UpdateSliderColor(); // Sa�l�k rengi ge�i�ini g�ncelle
        UpdateHealthText(); // Sa�l�k metnini g�ncelle
    }

    private void UpdateSliderColor()
    {
        // Slider doluluk rengini g�ncelle
        fillImage.color = Color.Lerp(emptyColor, fullColor, healthSlider.value / healthSlider.maxValue);
    }

    private void UpdateHealthText()
    {
        // Sa�l�k metnini g�ncelle
        healthText.text = playerScript.karakterCan�.ToString();
    }
}
