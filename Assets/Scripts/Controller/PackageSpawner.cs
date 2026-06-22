using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    [Header("Pengaturan Prefab & Lokasi")]
    [SerializeField] private GameObject paketPrefab;

    private void OnEnable()
    {
        EventHandler.OnRequestSpawn += SpawnPaket;
    }

    private void OnDisable()
    {
        EventHandler.OnRequestSpawn -= SpawnPaket;
    }

    private void SpawnPaket(SOAddress dataPaket)
    {
        // Spawn paket berdasarkan data yang dikirim oleh PaketManager
        GameObject paketBaru = Instantiate(paketPrefab, dataPaket.addressLocationTransform.position, Quaternion.identity);

        // Jadikan child dari titik spawn agar Hierarchy Unity tetap rapi
        paketBaru.transform.SetParent(dataPaket.addressLocationTransform);

        // Suntikkan data SOAddress ke dalam Paket tersebut
        PaketController controller = paketBaru.GetComponent<PaketController>();
        if (controller != null)
        {
            controller.SetupData(dataPaket);
        }
        else
        {
            Debug.LogError("[PaketSpawner] Prefab tidak memiliki script PaketController!");
        }

        Debug.Log($"[PaketSpawner] Sukses mencetak paket untuk: {dataPaket.addressPerson}");
    }
}