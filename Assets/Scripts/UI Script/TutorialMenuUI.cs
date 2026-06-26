using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialMenuUI : MonoBehaviour
{
    [Header("UI Setup")]
    [SerializeField] int maxPage;
    [SerializeField] Vector3 currentPos;
    [SerializeField] RectTransform slideContentRect;
    [SerializeField] float slideDuration;

    private int currentPage;
    Vector3 targetPos;

    void Awake()
    {
        currentPage = 1;
        targetPos = slideContentRect.localPosition;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += currentPos;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= currentPos;
            MovePage();
        }
    }

    public void CloseTutorialMenu()
    {
        Destroy(gameObject, .1f);
    }

    private void MovePage()
    {
        slideContentRect.DOAnchorPos(targetPos,slideDuration,false);
    }


}
