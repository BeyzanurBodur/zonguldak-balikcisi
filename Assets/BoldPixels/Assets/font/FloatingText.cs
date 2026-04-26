using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float yokOlmaSuresi = 1.0f; // 1 saniye sonra silinir
    public float yukariHiz = 200f;     // UI koordinatýnda yükselme hýzý
    public float sagaHiz = 50f;        // Hafif saða kayma hýzý

    void Start()
    {
        // ÖNEMLÝ: 1 saniye sonra objeyi sahneden tamamen siler
        Destroy(gameObject, yokOlmaSuresi);
    }

    void Update()
    {
        // UI objesini (RectTransform) hareket ettirmenin en saðlam yolu:
        transform.position += new Vector3(sagaHiz * Time.deltaTime, yukariHiz * Time.deltaTime, 0);
    }
}