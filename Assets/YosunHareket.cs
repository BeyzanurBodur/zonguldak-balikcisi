using UnityEngine;

public class YosunHareket : MonoBehaviour
{
    public float hareketHizi = 2f;
    private Transform taka; // Gemiyi takip etmesi için

    void Start()
    {
        // Sahnedeki "Taka" isimli gemiyi bul
        GameObject g = GameObject.Find("Taka");
        if (g != null) taka = g.transform;
    }

    void Update()
    {
        // Yosunu sola doðru akýt
        transform.Translate(Vector2.left * hareketHizi * Time.deltaTime);

        // YOK OLMA MANTIÐI:
        // Eðer gemi sahnede varsa ve yosun geminin 20 birim gerisinde (solunda) kaldýysa yok et
        if (taka != null)
        {
            if (transform.position.x < taka.position.x - 20f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Eðer gemi bulunamazsa (güvenlik önlemi)
            if (transform.position.x < -150f)
            {
                Destroy(gameObject);
            }
        }
    }
}