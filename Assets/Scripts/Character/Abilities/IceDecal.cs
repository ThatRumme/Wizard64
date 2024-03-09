using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDecal : MonoBehaviour
{
    // Start is called before the first frame update

    public float animateDuration = 1.0f;
    public float fullDuration = 10;
    public float startScale = 0.1f;
    public float endScale = 3f;
    void Start()
    {
        AnimateIn();
    }


    void AnimateIn()
    {
        void TweenOnComplete()
        {
            StartCoroutine(WaitAnimateOut());
        }

        transform.position += (Vector3.up * (endScale / 2));
        transform.localScale = new Vector3(startScale, startScale, endScale);
        transform.DOScaleX(endScale, animateDuration).SetEase(Ease.OutCubic);
        transform.DOScaleY(endScale, animateDuration).SetEase(Ease.OutCubic).OnComplete(TweenOnComplete);
    }

    void AnimateOut()
    {
        void TweenOnComplete()
        {
            Destroy(this.gameObject);
        }

        transform.DOScaleX(0, animateDuration).SetEase(Ease.InCubic);
        transform.DOScaleY(0, animateDuration).SetEase(Ease.InCubic).OnComplete(TweenOnComplete);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitAnimateOut()
    {
        yield return new WaitForSeconds(fullDuration - (animateDuration*2));
        AnimateOut();

    }
}
