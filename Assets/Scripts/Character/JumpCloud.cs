using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCloud : MonoBehaviour
{
    // Start is called before the first frame update

    public float endScale;

    public float animInDur;
    public float animOutDur;
    public float delayBeforeAnimOut;

    private Player player;


    bool hasAnimatedIn = false;

    void Start()
    {
        player = GameManager.Instance.player;
        transform.localScale = Vector3.zero;
        AnimateIn();
        StartCoroutine(DelayForAnimatedInBool());
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasAnimatedIn)
        {
            transform.position = player.transform.position + new Vector3(0, -1.2f, 0);
        }
       
    }

    void AnimateIn()
    {
        Vector3 finalScale = Vector3.one * endScale;
        transform.DOScale(finalScale, animInDur).SetEase(Ease.OutBack).OnComplete(OnDoneAnimatingIn);
    }


    void AnimateOut()
    {
        transform.DOScale(Vector3.zero, animOutDur).SetEase(Ease.InBack).OnComplete(OnDoneAnimatingOut);
    }

    private void OnDoneAnimatingIn()
    {
       
        StartCoroutine(DelayAnimOut());
    }

    private void OnDoneAnimatingOut()
    {
        Destroy(this.gameObject);
    }

    IEnumerator DelayAnimOut()
    {
        yield return new WaitForSeconds(delayBeforeAnimOut);
        AnimateOut();
    }

    IEnumerator DelayForAnimatedInBool()
    {
        yield return new WaitForSeconds(animInDur*0.5f);
        hasAnimatedIn = true;
    }
}
