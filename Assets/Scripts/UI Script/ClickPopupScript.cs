using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPopupScript : MonoBehaviour, IPointerClickHandler
{
    public GameObject objectYangMuncul;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (objectYangMuncul == null)
        {
            Debug.Log("[ClickPopup] Masukkin dulu objek yang mau dimunculin");
            return;
        }

        objectYangMuncul.SetActive(true);
        Time.timeScale = 0f; //ngepause pas modalnya muncul ntar diatur atur lagi aja klo misal gak perlu
        Debug.Log($"[ClickPopup] Tampilan {objectYangMuncul} muncul");
    }
}
