using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager> {

    [Header("Debug")]
    public bool gameActive;

    [Header("UI")]
    public UIPauseMenu pauseUI;

    // Misc
    public PlayerInput inputs;

    public Player player;

    public LevelManager lm;

    private void Awake()
    {
        inputs = InputManager.Instance.inputs;
        SaveGame.Instance.LoadData();
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Main.Exit.performed += EscapePress;
    }

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Main.Exit.performed -= EscapePress;
}

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
}

    private void Update()
    {
        if (!gameActive)
            return;
    }

    public void SetLevelManager(LevelManager lm)
    {
        this.lm = lm;
    }


    private void EscapePress(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Pause();
        }
    }
        
    private void RestartPress(InputAction.CallbackContext context)
    {

    }


    /// <summary>
    /// Pauses the game
    /// </summary>
    private void Pause() {

        Debug.Log("[GameManager] Pause");

        gameActive = !gameActive;

        if (pauseUI)
        {
            pauseUI.gameObject.SetActive(!gameActive);
        }
    }
        
    /// <summary>
    /// Resume the game
    /// </summary>
    public void Resume() {

        gameActive = true;

        if (pauseUI)
        {
            pauseUI.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// Quit the game
    /// </summary>
    public void Exit() {
        Application.Quit();
    }
}

