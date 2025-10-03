using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Fields
    [Header("General")]
    public PlayerMovement pm; //Player Movement
    public Transform camera; //Camera

    PlayerInput inputs;

    [Header("Health")]
    public int maxHealth = 20;
    public float timeUntilCanRegenHealth = 4;
    public float secondsForEachHeartRegen = 0.4f;
    public static int currentHealth = 20;
    float regenTimer = 0;
    float canRegenTimer = 0;

    [Header("Melee")]
    public int meleeDamage = 4;
    public float meleeAttackDelay = 0.5f;
    private float meleeAttackTimer = 0;
    public AbilityHitBox meleeHitBox;
    public Transform staffPivot;

    [Header("Misc")]
    public int spawnIdx = 0;

    private Interaction currentInteraction;

    #endregion

    #region Start, Awake, Update


    private void Awake()
    {
        GameManager.Instance.player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();

        inputs = GameManager.Instance.inputs;
        inputs.Main.Fire1.performed += AttemptMeleeAttack;
        inputs.Main.Interact.performed += OnInteractKey;

        transform.position = GameManager.Instance.lm.GetSpawnPoint(spawnIdx);
        currentHealth = maxHealth;
        meleeAttackTimer = meleeAttackDelay;
    }

    private void OnDestroy()
    {
        inputs.Main.Fire1.performed -= AttemptMeleeAttack;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!gm.gameActive)
        //    return;

        RegenerateHealth();
        IncrementMeleeTimer();
    }

    #endregion

    private void OnEnable()
    {
        //inputs.Enable();
        inputs = GameManager.Instance.inputs;
    }

    private void OnDisable()
    {
    }

    private void AttemptMeleeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MeleeAttack();
        }
    }

    private void OnInteractKey(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (currentInteraction)
            {
                currentInteraction.OnPressedForInteraction();
            }
        }
    }

    private void MeleeAttack()
    {
        if (meleeAttackTimer < meleeAttackDelay) return;
        staffPivot.transform.DOLocalRotateQuaternion(Quaternion.Euler(90, 0, 0), 0.1f).SetEase(Ease.Linear).OnComplete(DealMeleeDamage);
        meleeAttackTimer = 0;
    }

    private void DealMeleeDamage()
    {
        foreach(Enemy enemy in meleeHitBox.enemiesInTrigger)
        {

            if (enemy != null)
            {
                enemy.TakeDamage(meleeDamage);
            }
            
        }
        staffPivot.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f).SetDelay(0.1f).SetEase(Ease.Linear);
    }

    private void IncrementMeleeTimer()
    {
        if(meleeAttackTimer <= meleeAttackDelay)
        {
            meleeAttackTimer += Time.deltaTime;
        }
    }

    void RegenerateHealth()
    {
        if (currentHealth == maxHealth) return;


        if(canRegenTimer >= timeUntilCanRegenHealth)
        {
            if(regenTimer >= secondsForEachHeartRegen)
            {
                regenTimer -= secondsForEachHeartRegen;
                currentHealth = Mathf.Min(currentHealth+1, maxHealth);
            }
            else
            {
                regenTimer += Time.deltaTime;
            }
        }
        else
        {
            canRegenTimer += Time.deltaTime;
        }
    }

    void ResetCanRegenTimer()
    {
        canRegenTimer = 0;
    }

    public void TakeDamage(int damage)
    {
        ResetCanRegenTimer();
        currentHealth -= damage;
        if(currentHealth <= 0 )
        {
            Die();
        }
        else
        {
            EventManager.OnPlayerHealthUpdated(currentHealth);
        }
    }

    private void Die()
    {
        transform.position = GameManager.Instance.lm.GetSpawnPoint(spawnIdx);
        currentHealth = maxHealth;
        EventManager.OnPlayerDied();
        EventManager.OnPlayerHealthUpdated(currentHealth);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            other.GetComponentInParent<Collectable>().PickUp();
        }
        else if (other.CompareTag("KillTrigger"))
        {
            Die();
        }
        else if (other.CompareTag("Interaction"))
        {
            if(currentInteraction != null)
            {
                currentInteraction.OnLeaveInteractionHitBox();
            }
            Interaction interaction = other.GetComponent<Interaction>();
            currentInteraction = interaction;
            interaction.OnEnterInteractionHitBox();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interaction"))
        {
            Interaction interaction = other.GetComponent<Interaction>();
            interaction.OnLeaveInteractionHitBox();
            if(currentInteraction == interaction)
            {
                currentInteraction = null;
            }
        }
    }

}
