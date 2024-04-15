using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogInteraction : Interaction
{
    public GameObject dialogUI;
    public TextMeshProUGUI dialogText;
    private NPC npc;

    protected override void Start()
    {
        base.Start();
        dialogUI.SetActive(false);
        npc = GetComponentInParent<NPC>();
    }

    public override void OnPressedForInteraction()
    {
        if (dialogUI.activeSelf)
        {
            OnLeaveInteractionHitBox();
            OnEnterInteractionHitBox();
        }
        else
        {
            dialogUI.SetActive(true);
            ShowPressForInteractionUI(false);
            if(npc != null)
            {
                npc.RotateToFacePlayer();
            }
        }
        
    }

    public override void OnLeaveInteractionHitBox()
    {
        base.OnLeaveInteractionHitBox();
        dialogUI.SetActive(false);
    }

}
