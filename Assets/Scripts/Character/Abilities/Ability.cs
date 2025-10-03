using UnityEngine;

public class Ability : MonoBehaviour
{
    public int idx = 0;

    protected Player player;
    protected PlayerMovement playerMovement;

    public Transform staffCrystal;

    protected bool isEnabled = false;

    protected bool AllowUse()
    {
        //if enough mana
        return true;
    }

    public virtual void Setup(Player player, PlayerMovement pm)
    {
        this.player = player;
        playerMovement = pm;
    }
    public virtual bool Activate()
    {
        return false;
    }
    public virtual void Deactivate()
    {
        ResetValues();
    }
    public virtual void SwitchOn()
    {
        isEnabled = true;
        ResetValues();
    }
    public virtual void SwitchOff()
    {
        isEnabled = false;
        ResetValues();
    }

    protected virtual void ResetValues()
    {
        
    }
}
