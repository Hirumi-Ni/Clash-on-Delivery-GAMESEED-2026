using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class TrailAnimate : MonoBehaviour
{
    [SerializeField] private SplineAnimate splineAnimate;
    private Tween splineTween;
    private int duration;

    void OnEnable()
    {
        EventHandler.OnStartReturnToHub += ReturnHub;
    }

    void OnDisable()
    {
        EventHandler.OnStartReturnToHub -= ReturnHub;
    }

    public void SetupTrail(int splineDuration)
    {
        duration = splineDuration/10;
        splineAnimate.NormalizedTime = 0f;
        splineAnimate.Duration = duration; 
        Debug.Log($"{splineAnimate.Duration} ms");
        splineTween = DOTween.To(() => splineAnimate.NormalizedTime, x => splineAnimate.NormalizedTime = x, 1f, duration).SetEase(Ease.Linear).SetAutoKill(false).Pause();
        PlayForward();
    }
    
    public void ReturnHub(int _)
    {
        PlayBackward();
    }

    [ContextMenu("AnimateForward")]
    public void PlayForward()
    {
        splineTween.PlayForward();
    }

    [ContextMenu("AnimateBackward")]
    public void PlayBackward()
    {
        splineTween.PlayBackwards();
        Destroy(gameObject, duration+1);
    }
}