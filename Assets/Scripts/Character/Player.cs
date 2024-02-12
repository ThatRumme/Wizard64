using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Fields
    [Header("General")]
    public PlayerMovement pm; //Player Movement
    public Transform camera;  //Camera
    public Animator  _animationContoller;
    
    
    PlayerInput inputs;

    #endregion

    #region Start, Awake, Update


    private void Awake()
    {
        

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
    
}
