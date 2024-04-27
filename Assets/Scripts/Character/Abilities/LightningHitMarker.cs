using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningHitMarker : MonoBehaviour
{
    // Start is called before the first frame update


    public LightningBeam[] lightningbeams;

    public float range = 2;

    public float duration = 1.5f;
    public float startScale;
    public float endScale;
    float currentScale;


    public GameObject[] lbEndTransforms;

    public MeshRenderer[] coreMeshes;

    void Start()
    {

        void TweenOnComplete()
        {
            Destroy(this.gameObject);
        }

        DOTween.To(() => currentScale, x => currentScale = x, endScale, duration).SetEase(Ease.OutQuint).OnComplete(TweenOnComplete);

        for (int i = 0; i < lbEndTransforms.Length; i++)
        {
            lightningbeams[i].Setup(transform, lbEndTransforms[i], lbEndTransforms[i].transform.position, false, Vector3.zero);
        }

        for (int i = 0; i < coreMeshes.Length; i++)
        {
            Color currentColor = coreMeshes[i].material.color;
            Color newColor = currentColor;
            newColor.a = 0;
            coreMeshes[i].material.DOColor(newColor, duration).SetEase(Ease.InCubic);
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(1, 1, 1) * currentScale;
    }
}
