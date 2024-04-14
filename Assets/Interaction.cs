using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject pressForInteractionUI;

    public Transform canvasPivot;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pressForInteractionUI.SetActive(false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        FacePlayer();
    }

    public void ShowPressForInteractionUI(bool toggle)
    {
        pressForInteractionUI.SetActive(toggle);
    }

    public virtual void OnPressedForInteraction()
    {
        //Override to run code for when Use key is pressed..
    }

    public virtual void OnEnterInteractionHitBox()
    {
        ShowPressForInteractionUI(true);
    }

    public virtual void OnLeaveInteractionHitBox()
    {
        ShowPressForInteractionUI(false);
    }

    private void FacePlayer()
    {
        Vector3 direction = GameManager.Instance.player.transform.position - Camera.main.transform.position;
        Quaternion lookRot = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0);
        canvasPivot.rotation = lookRot;
    }


}
