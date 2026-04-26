using UnityEngine;

public class BalikHareket : MonoBehaviour
{
    public float hiz = 3f;
    private Transform taka; // Gemiyi takip edeceðiz

    void Start()
    {
        // Sahnedeki "Taka" isimli objeyi buluyoruz
        GameObject g = GameObject.Find("Taka");
        if (g != null) taka = g.transform;
    }

    void Update()
    {
        // Balýðý sola yürüt
        transform.Translate(Vector3.left * hiz * Time.deltaTime);

        // YOK ETME MANTIÐI:
        // Eðer gemi bulunduysa ve balýk geminin 20 birim soluna düþtüyse yok et
        if (taka != null)
        {
            if (transform.position.x < taka.position.x - 20f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Eðer gemi bir þekilde bulunamazsa eski güvenli liman (çok uzak bir nokta)
            if (transform.position.x < -150f) Destroy(gameObject);
        }
    }
}