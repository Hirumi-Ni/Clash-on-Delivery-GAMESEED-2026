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

    void Start()
    {
        currentPage = 1;
        targetPos = slideContentRect.localPosition;
        Time.timeScale = 0;
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
        Time.timeScale = 1;
        Destroy(gameObject, .1f);
    }

    private void MovePage()
    {
        slideContentRect.DOAnchorPos(targetPos,slideDuration,false).SetUpdate(true);
    }


}
