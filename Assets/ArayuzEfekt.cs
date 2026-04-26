using UnityEngine;
using System.Collections;

public class ArayuzEfekt : MonoBehaviour
{
    public Transform taka;           // Taka (Gemi) objesini buraya sürükle
    public CanvasGroup arayuzGrubu;  // Az önce oluþturduðumuz "OyunArayuzu"nu buraya sürükle
    public float baslangicX = 15.78f; // Senin belirlediðin o kritik nokta
    public float gecisHizi = 0.5f;   // Ne kadar sürede tam aydýnlansýn? (Düþük sayý = yavaþ geçiþ)

    private bool efektCalisti = false;

    void Start()
    {
        // Oyun baþýnda her þey gizli baþlasýn
        if (arayuzGrubu != null) arayuzGrubu.alpha = 0;
    }

    void Update()
    {
        // Tekne o noktayý geçtiði an efekti baþlat
        if (!efektCalisti && taka.position.x >= baslangicX)
        {
            StartCoroutine(YavascaGoster());
            efektCalisti = true;
        }
    }

    IEnumerator YavascaGoster()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * gecisHizi;
            arayuzGrubu.alpha = t; // Saydamlýðý yavaþ yavaþ artýrýyor
            yield return null;
        }
        arayuzGrubu.alpha = 1f; // Tam görünür yap
    }
}