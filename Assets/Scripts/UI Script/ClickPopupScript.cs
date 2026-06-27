using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPopupScript : MonoBehaviour, IPointerClickHandler
{
    public GameObject objectYangMuncul;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PackageManager.Instance.currentState != CourierState.OnHub && gameObject.name == "AddressPinpoint") return;

        if (objectYangMuncul == null)
        {
            Debug.Log("[ClickPopup] Masukkin dulu objek yang mau dimunculin");
            return;
        }

        objectYangMuncul.SetActive(true);
        transform.parent.SetAsLastSibling();
        Time.timeScale = 0f; //ngepause pas modalnya muncul ntar diatur atur lagi aja klo misal gak perlu
        Debug.Log($"[ClickPopup] Tampilan {objectYangMuncul} muncul");

        AudioManager.instance.PlayAudio(SoundType.Menu);
    }
}
