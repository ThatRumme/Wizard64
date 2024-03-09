using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHandler : MonoBehaviour
{
    private PlayerInput inputs;
    private Player player;
    private PlayerMovement playerMovement;

    public Ability[] abilities;
    private int currentAbilityIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
        playerMovement = GetComponentInParent<PlayerMovement>();

        inputs = GameManager.Instance.inputs;
        inputs.Main.SwitchAbilityForward.performed += NextAbilityInput;
        inputs.Main.SwitchAbilityBackward.performed += PrevAbilityInput;
        inputs.Main.UseAbility.performed += UseAbility;
        inputs.Main.UseAbility.canceled += UseAbility;

        
        foreach(Ability ability in abilities)
        {
            ability.Setup(player, playerMovement);
        }

    }

    private void OnDestroy()
    {
        inputs.Main.SwitchAbilityForward.performed -= NextAbilityInput;
        inputs.Main.SwitchAbilityBackward.performed -= PrevAbilityInput;
        inputs.Main.UseAbility.performed -= UseAbility;
        inputs.Main.UseAbility.canceled -= UseAbility;

    }


    // Update is called once per frame
    void Update()
    {
        
    }


    void NextAbilityInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            abilities[currentAbilityIdx].SwitchOff();
            if (currentAbilityIdx == abilities.Length-1)
            {
                currentAbilityIdx = 0;
            }
            else
            {
                currentAbilityIdx++;
            }
            abilities[currentAbilityIdx].SwitchOn();
            EventManager.OnAbilitySwitched(currentAbilityIdx);

        }
    }

    void PrevAbilityInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            abilities[currentAbilityIdx].SwitchOff();
            if (currentAbilityIdx == 0)
            {
                currentAbilityIdx = abilities.Length - 1;
            }
            else
            {
                currentAbilityIdx--;
            }
            abilities[currentAbilityIdx].SwitchOn();
            EventManager.OnAbilitySwitched(currentAbilityIdx);
        }
    }

    void UseAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            abilities[currentAbilityIdx].Activate();
        }
        else if (context.canceled)
        {
            abilities[currentAbilityIdx].Deactivate();
        }
    }
}
