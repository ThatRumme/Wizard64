using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

    public class GameManager : Singleton<GameManager> {

        [Header("Debug")]
        public bool gameActive;

        [Header("UI")]
        public GameObject gameUI;
        public GameObject endUI;
        public GameObject pauseUI;
        public GameObject optionUI;
        public GameObject mainmenuUI;
        public bool inMainMenu = true;
        public int currentUI;

        // Misc
        public PlayerInput inputs;

        private void Awake()
        {
            inputs = InputManager.Instance.inputs;
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

            //gameActive = !gameActive;
        }
        
        /// <summary>
        /// Resume the game
        /// </summary>
        public void Resume() {
          
        }


        /// <summary>
        /// Quit the game
        /// </summary>
        public void Exit() {
            Application.Quit();
        }
    }

