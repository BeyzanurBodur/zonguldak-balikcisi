using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject balikPrefab;
    public GameObject yosunPrefab;
    public GameObject buyukBalikPrefab; // PALAMUT PREFABI BURAYA

    public float spawnAraligi = 2f;
    public float yosunSpawnAraligi = 3f;
    public float buyukBalikSpawnAraligi = 6f; // Palamutlar daha nadir gelir

    public float minY = -2f;
    public float maxY = -4f;
    public float spawnX = 10f;

    void Start()
    {
        // Temel balýk ve yosun döngüsü
        InvokeRepeating("BalikYarat", 1f, spawnAraligi);
        InvokeRepeating("YosunYarat", 2f, yosunSpawnAraligi);

        // Palamut döngüsü (Arka planda çalýþýr ama sadece Level 3'te ekrana basar)
        InvokeRepeating("BuyukBalikYarat", 3f, buyukBalikSpawnAraligi);
    }

    void BalikYarat()
    {
        float rastgeleY = Random.Range(minY, maxY);
        Vector3 spawnPozisyonu = new Vector3(transform.position.x, rastgeleY, 0);
        Instantiate(balikPrefab, spawnPozisyonu, Quaternion.identity);
    }

    void YosunYarat()
    {
        if (LevelManager.instance != null && LevelManager.instance.gecerliLevel >= 2)
        {
            float rastgeleY = Random.Range(minY, maxY);
            Vector3 spawnPozisyonu = new Vector3(transform.position.x, rastgeleY, 0);
            Instantiate(yosunPrefab, spawnPozisyonu, Quaternion.identity);
        }
    }

    void BuyukBalikYarat()
    {
        // SADECE LEVEL 3'TE (veya sonrasýnda) ÇIKAR
        if (LevelManager.instance != null && LevelManager.instance.gecerliLevel >= 3)
        {
            float rastgeleY = Random.Range(minY, maxY);
            Vector3 spawnPozisyonu = new Vector3(transform.position.x, rastgeleY, 0);
            Instantiate(buyukBalikPrefab, spawnPozisyonu, Quaternion.identity);
        }
    }
}