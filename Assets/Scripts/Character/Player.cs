using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Fields
    [Header("General")]
    public PlayerMovement pm; //Player Movement
    public Transform camera; //Camera

    PlayerInput inputs;

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

    }

    // Update is called once per frame
    void Update()
    {
        //if (!gm.gameActive)
        //    return;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            Debug.Log("Pick up???");
            other.GetComponentInParent<Collectable>().PickUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

}
