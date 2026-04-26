using UnityEngine;

public class TakaKontrol : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float hiz = 4f;

    [Header("Ekran Sınırları")]
    // BURADAKİ SAYILARI İSKELE VE DENİZİN EN UÇ NOKTALARINA GÖRE GÜNCELLE
    public float solSinir = -60f; // İskele tarafı (Örn: -60)
    public float sagSinir = 60f;  // Açık deniz tarafı (Örn: 60)

    private Rigidbody2D rb;
    private float hareketGirdisi;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        hareketGirdisi = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        // Hareket kodun gayet iyi
        rb.linearVelocity = new Vector2(hareketGirdisi * hiz, rb.linearVelocity.y);

        // SINIRLANDIRMA
        // Eğer tekne iskelede duruyorsa bu Clamp onu ışınlıyordu. 
        // Sayıları (solSinir, sagSinir) Inspector'dan genişletmeyi unutma!
        float sinirlandirilmisX = Mathf.Clamp(transform.position.x, solSinir, sagSinir);
        transform.position = new Vector3(sinirlandirilmisX, transform.position.y, transform.position.z);
    }
}