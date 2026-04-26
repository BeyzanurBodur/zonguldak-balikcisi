using UnityEngine;

public class KameraTakip : MonoBehaviour
{
    [Tooltip("Kameranýn kimi çekeceðini buraya sürükle")]
    public Transform hedef;

    [Tooltip("Kamera adama ne kadar yumuþak yetiþsin?")]
    public float yumusaklik = 5f;

    [Tooltip("Kameranýn uzaklýðý (Z mutlaka -10 kalmalý)")]
    public Vector3 ofset = new Vector3(0, 2, -10);

    void LateUpdate()
    {
        if (hedef != null)
        {
            Vector3 gidilecekYer = hedef.position + ofset;
            transform.position = Vector3.Lerp(transform.position, gidilecekYer, yumusaklik * Time.deltaTime);
        }
    }
}