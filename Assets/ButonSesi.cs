using UnityEngine;
using UnityEngine.EventSystems;

// Sadece IPointerDownHandler var, yani sadece TIKLAMAYI algýlayacak
public class ButonSesi : MonoBehaviour, IPointerDownHandler
{
    [Header("Ses Dosyasý")]
    public AudioClip tiklamaSesi;     // Butona basýnca çýkacak ses

    private AudioSource gizliHoparlor;

    void Start()
    {
        // Kod oyun baþlarken butona gizli bir hoparlör takar
        gizliHoparlor = gameObject.AddComponent<AudioSource>();
        gizliHoparlor.playOnAwake = false;
        gizliHoparlor.volume = 0.8f; // Sesi çok gelirse burayý 0.5f falan yapabilirsin      
    }

    // Fareye TIKLANDIÐI AN çalýþýr
    public void OnPointerDown(PointerEventData eventData)
    {
        if (tiklamaSesi != null)
        {
            gizliHoparlor.PlayOneShot(tiklamaSesi);
        }
    }
}