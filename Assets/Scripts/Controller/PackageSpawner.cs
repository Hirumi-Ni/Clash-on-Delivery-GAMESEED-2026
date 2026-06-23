using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    [Header("Pengaturan Prefab & Lokasi")]
    [SerializeField] private GameObject paketPrefab;
    [SerializeField] private GameObject canvasParent; // Parent untuk UI paket agar tetap rapi di Hierarchy

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
        GameObject paketBaru = Instantiate(paketPrefab, new Vector3(1920 / 2, 1080 / 2, 0), Quaternion.identity);

        // Jadikan child dari titik spawn agar Hierarchy Unity tetap rapi
        paketBaru.transform.SetParent(canvasParent.transform, true);

        AddressUIScript uiScript = paketBaru.GetComponent<AddressUIScript>();
        PackageController paketController = paketBaru.GetComponent<PackageController>();
        if (uiScript != null && paketController != null)
        {
            uiScript.SetupAddress(dataPaket);
            paketController.SetupData(dataPaket);
        }
        else
        {
            Debug.LogError("[PaketSpawner] Prefab tidak memiliki script AddressUIScript!");
        }

        Transform pinPoint = paketBaru.transform.GetChild(0);
        Transform completeButton = paketBaru.transform.GetChild(1);
        if (pinPoint != null && completeButton != null)
        {
            pinPoint.localPosition = dataPaket.addressLocationTransform; // Set posisi child sesuai data dari SOAddress
            completeButton.localPosition = dataPaket.addressLocationTransform; // Set posisi child sesuai data dari SOAddress
        }
        else
        {
            Debug.LogError("[PaketSpawner] Prefab tidak memiliki child untuk pin point!");
        }

        Debug.Log($"[PaketSpawner] Sukses mencetak paket untuk: {dataPaket.addressPerson}");
    }
}