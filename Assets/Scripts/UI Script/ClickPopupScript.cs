using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPopupScript : MonoBehaviour, IPointerClickHandler
{
    public GameObject objectYangMuncul;
  
    public void OnPointerClick(PointerEventData eventData)
    {
        if(objectYangMuncul == null)
        {
            Debug.Log("Masukkin dulu objek yang mau dimunculin");
            return;
        }

        objectYangMuncul.SetActive(true);
        Debug.Log($"Tampilan {objectYangMuncul} muncul");
    }
}
