using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
 
public class OptionMenu : MonoBehaviour
{
        
    #region Fields
        
    [Header("Audio Settings")] public OptionSlider soundSlider;
    public OptionSlider musicSlider;
    public AudioMixer audioMixer;

    [Header("Game Settings")]
    public OptionSlider sensitivitySlider;
    public OptionSlider fovSlider;
    public TMP_Dropdown fpsLimit;
    public GameObject[] checkboxes;
    private bool showTimer;
    public TMP_Dropdown renderDistance;

    [Header("Graphics Settings")] 
    public TMP_Dropdown resolution;
    public bool fullScreen;
    //public Dropdown textureQuality;
    //public Dropdown antialiasing;
    private bool vsync;        

    public GameObject[] graphicsCheckboxes;        

    //[Header("Save / Load")] public static float[]
    //    settings = new float[8]; //0 = sound, 1 = music, 2 = ambient, 3 = timer, 4 = FPS

    [Header("Sections")] public GameObject gameSection;
    public GameObject keybindSection;
        

    [Header("Events")] public UnityEvent OnMenuClose;

    [Header("KeyMap UI")]
    public GameObject duplicateWarning;
    public TextMeshProUGUI forwardKeyUI;
    public TextMeshProUGUI backwardKeyUI;
    public TextMeshProUGUI leftKeyUI;
    public TextMeshProUGUI rightKeyUI;
    public TextMeshProUGUI jumpKeyUI;
    public TextMeshProUGUI fireKeyUI;
    public TextMeshProUGUI recenterCameraKeyUI;
    public TextMeshProUGUI switchAbilityForwardKeyUI;
    public TextMeshProUGUI switchAbilityBackwardKeyUI;
    public TextMeshProUGUI useAbilityKeyUI;
    public TextMeshProUGUI interactKeyUI;

    PlayerInput inputs;

    // Private Vars
    private readonly XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingsObject));
    public static SettingsObject settingsObj;

    // Private Bools

    private bool showSpeedRunUI, useProfanityFilter;
    private bool isApplicationQuitting;
    public static bool useScrollJump;


    [Header("InputRemappingUI")]
    public GameObject TimeoutUI;
    public float inputTimeout;
    private string remapNextFrame = "";

    [Header("Other")]
    public Color markedSectionColor;
    public Text[] optionSectionButtons;

    public GameObject firstSelected;
    #endregion

    #region Unity Event Functions

    public void Awake() {
        //InputManager.GetInput().Menu.Escape.performed += CloseOptions;
    }

    public void OnDestroy() {
        //InputManager.GetInput().Menu.Escape.performed -= CloseOptions;
    }

    private void OnEnable()
    {
        Setup();
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        inputs = GameManager.Instance.inputs;
        ChangeSection(0);
        SettingsManager.SettingsRead();
        ApplySettings();
    }

    private void Update() {
        if (remapNextFrame != "")
        {
            UpdateKeybind(remapNextFrame);
        }
    }

    private void OnDisable()
    {
        //If we are quiting from the menu, don't re-enable
        if (isApplicationQuitting)
        {
            return;
        }

        inputs.Enable();
    }

    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }

    #endregion

    public void UpdateSettings()
    {

        Debug.Log("[OPTIONS] Updating settings...");

        var menu = 1;

        int width;
        int height;
        int fps;
        int renderDist;

        if (int.TryParse(resolution.options[resolution.value].text.Split('x')[0], out width)) { }

        if (int.TryParse(resolution.options[resolution.value].text.Split('x')[1], out height)) { }

        if (int.TryParse(fpsLimit.options[fpsLimit.value].text, out fps)) { }

        if (int.TryParse(renderDistance.options[renderDistance.value].text, out renderDist)) { }


        SettingsManager.SetSettings(new SettingsObject
        {
            SoundEffectsVolume = (int)soundSlider.slider.value,
            MusicVolume = (int)musicSlider.slider.value,
            Sensitivity = (int)sensitivitySlider.slider.value,
            //ShowVelocity = showVel,
            RenderDistance = renderDist,
            //ShowInput = showInput,
            //ShowSpeedRunUI = showSpeedRunUI,
            //UseProfanityFilter = useProfanityFilter,
            //UseScrollJump = OptionMenu.useScrollJump,
            //LeaderboardFilter = SettingsManager.GetSettings.LeaderboardFilter,
            InputSettings = new InputSetting
            {
                Forward = inputs.Main.Forward.bindings[0].overridePath ?? inputs.Main.Forward.bindings[0].path,
                Backwards = inputs.Main.Backwards.bindings[0].overridePath ?? inputs.Main.Backwards.bindings[0].path,
                Left = inputs.Main.Left.bindings[0].overridePath ?? inputs.Main.Left.bindings[0].path,
                Right = inputs.Main.Right.bindings[0].overridePath ?? inputs.Main.Right.bindings[0].path,
                Jump = inputs.Main.Jump.bindings[0].overridePath ?? inputs.Main.Jump.bindings[0].path,
                Fire1 = inputs.Main.Fire1.bindings[0].overridePath ?? inputs.Main.Fire1.bindings[0].path,
                SwitchAbilityBackward = inputs.Main.SwitchAbilityBackward.bindings[0].overridePath ?? inputs.Main.SwitchAbilityBackward.bindings[0].path,
                SwitchAbilityForward = inputs.Main.SwitchAbilityForward.bindings[0].overridePath ?? inputs.Main.SwitchAbilityForward.bindings[0].path,
                RecenterCamera = inputs.Main.RecenterCamera.bindings[0].overridePath ?? inputs.Main.RecenterCamera.bindings[0].path,
                UseAbility = inputs.Main.UseAbility.bindings[0].overridePath ?? inputs.Main.UseAbility.bindings[0].path,
                Interact = inputs.Main.Interact.bindings[0].overridePath ?? inputs.Main.Interact.bindings[0].path,
            },
            GraphicSettings = new GraphicSetting
            {
                
                ResolutionWidth = width,
                ResolutionHeight = height,
                FullScreen = fullScreen,
                //Bloom = bloom.value * 10,
                //ScreenSpaceReflections = screenSpaceReflections.value,
                //AmbientOcclusion = ambientOcclusion,
                //Antialiasing = antialiasing.value,
                //Filtering = Convert.ToBoolean(filtering.value),
                //TextureQuality = textureQuality.value,
                Vsync = vsync,
                FPS = fps
            }                
        });

        SettingsManager.SettingsWrite();
        ApplySettings();
    }

    public void ApplySettings() {
            
        SettingsObject curSettings = SettingsManager.GetSettings;
        float sensitivity = Mathf.Clamp(curSettings.Sensitivity, 0, 2000);

        //Set audio settings
        float sfx = Mathf.Clamp(curSettings.SoundEffectsVolume, 0, 100);
        float music = Mathf.Clamp(curSettings.MusicVolume, 0, 100);

        SettingsManager.ApplySettings();

        //Set UI Slider & Checkbox values
        UpdateUISliders(sfx, music, sensitivity);
        UpdateOptionCheckBoxes(
            //curSettings.ShowVelocity,
                
        //curSettings.ShowInput;
        );
        UpdateBoolCheckBoxes(
            
            );
        UpdateGraphicsCheckBoxes(
        curSettings.GraphicSettings.FullScreen,
            curSettings.GraphicSettings.Vsync);

        bool fpsbuiltin = false;
        for (int index = 0; index < fpsLimit.options.Count; index++)
        {
            var option = fpsLimit.options[index];
            if (option.text != curSettings.GraphicSettings.FPS.ToString()) {
                continue;
            }

            fpsLimit.value = index;
            fpsbuiltin = true;
        }
            
        if(!fpsbuiltin)
        {
            fpsLimit.AddOptions(new List<string> {
                curSettings.GraphicSettings.FPS.ToString()
            });
        }
            
            
        //Setup Graphics
        var gs = SettingsManager.GetSettings.GraphicSettings;
        //GraphicsSettings.Reload();
            
            
        //Setup Graphics Display
        bool builtin = false;
        resolution.ClearOptions();
        foreach (var res in Screen.resolutions.Reverse()) {
            bool alreadyAdded = false;
            foreach (var option in resolution.options) {
                if (option.text ==  String.Format("{0}x{1}", res.width, res.height)){
                    alreadyAdded = true;
                    break;
                }
            }
                
            if(alreadyAdded)
                continue;
                
            resolution.AddOptions(new List<string> {
                String.Format("{0}x{1}", res.width, res.height)
            });  
        }

        for (int index = 0; index < resolution.options.Count; index++)
        {
            var option = resolution.options[index];
            if (!option.text.Contains(curSettings.GraphicSettings.ResolutionWidth.ToString())) {
                continue;
            }

            resolution.value = index;
            builtin = true;
        }
            
        if(!builtin)
        {
            resolution.AddOptions(new List<string> {
                String.Format("{0}x{1}", gs.ResolutionWidth, gs.ResolutionHeight)
            });
        }

        bool renderDistBuiltin = false;
        for (int index = 0; index < fpsLimit.options.Count; index++)
        {
            var renderDist = renderDistance.options[index];
            if (renderDist.text != curSettings.RenderDistance.ToString())
            {
                continue;
            }

            renderDistance.value = index;
            renderDistBuiltin = true;
        }

        if (!renderDistBuiltin)
        {
            renderDistance.AddOptions(new List<string> {
                curSettings.RenderDistance.ToString()
            });
        }

        fullScreen = curSettings.GraphicSettings.FullScreen;
        vsync = curSettings.GraphicSettings.Vsync;
        //bloom.value = gs.Bloom / 10;
        //screenSpaceReflections.value = gs.ScreenSpaceReflections;
        //textureQuality.value = gs.TextureQuality;
        //antialiasing.value = gs.Antialiasing;
        //filtering.value = Convert.ToInt16(gs.Filtering);

        // Rebinds Keys
        SettingsManager.RebindKeys();

        // Update UI KeyMap Display
        UpdateKeyDisplay(forwardKeyUI, inputs.Main.Forward.controls[0].displayName);
        UpdateKeyDisplay(backwardKeyUI, inputs.Main.Backwards.controls[0].displayName);
        UpdateKeyDisplay(leftKeyUI, inputs.Main.Left.controls[0].displayName);
        UpdateKeyDisplay(rightKeyUI, inputs.Main.Right.controls[0].displayName);
        UpdateKeyDisplay(jumpKeyUI, inputs.Main.Jump.controls[0].displayName);
        UpdateKeyDisplay(fireKeyUI, inputs.Main.Fire1.controls[0].displayName);
        UpdateKeyDisplay(recenterCameraKeyUI, inputs.Main.RecenterCamera.controls[0].displayName);
        UpdateKeyDisplay(switchAbilityForwardKeyUI, inputs.Main.SwitchAbilityForward.controls[0].displayName);
        UpdateKeyDisplay(switchAbilityBackwardKeyUI, inputs.Main.SwitchAbilityBackward.controls[0].displayName);
        UpdateKeyDisplay(useAbilityKeyUI, inputs.Main.UseAbility.controls[0].displayName);
        UpdateKeyDisplay(interactKeyUI, inputs.Main.Interact.controls[0].displayName);

        //CheckForDuplicate();
        //EventManager.RaiseOnSettingsUpdated();
        EventManager.OnSettingsApplied();
    }

    private void UpdateBoolCheckBoxes(params bool[] checkbox) {
        //for (int i = 0; i < checkbox.Length; i++)
        //{
        //    string sText = checkbox[i] ? "Enabled" : "Disabled";
        //    checkboxes[i].transform.GetChild(1).GetComponentInChildren<Text>().text = sText;
        //}
    }
        
    private void UpdateOptionCheckBoxes(params int[] checkbox) {
        for (int i = 0; i < checkbox.Length; i++)
        {
            string sText = checkbox[i] == 1 ? "Enabled" : "Disabled";
            checkboxes[i].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = sText;
        }
    }

    private void UpdateGraphicsCheckBoxes(params bool[] checkbox) {
        for (int i = 0; i < checkbox.Length; i++)
        {
            string sText = checkbox[i] ? "Enabled" : "Disabled";
            graphicsCheckboxes[i].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = sText;
        }
    }
        
    private void UpdateUISliders(float sfx, float music, float sensitivity) {
        soundSlider.slider.value = sfx;
        musicSlider.slider.value = music;
        sensitivitySlider.slider.value = sensitivity;
    }

        
    public void UpdateCheckBox(int i) {
        var curSettings = SettingsManager.GetSettings;
        string sText = "INVALID";
        switch (i)
        {
            case 1:
                curSettings.GraphicSettings.FullScreen = fullScreen = !curSettings.GraphicSettings.FullScreen;
                sText = curSettings.GraphicSettings.FullScreen ? "Enabled" : "Disabled";
                break;
            case 2:
                curSettings.GraphicSettings.Vsync = vsync = !curSettings.GraphicSettings.Vsync;
                sText = curSettings.GraphicSettings.Vsync ? "Enabled" : "Disabled";
                break;
        }

        if (i < checkboxes.Length){
            checkboxes[i].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = sText;
            Debug.Log("Checkbox: " + i);
        } else {
            graphicsCheckboxes[i - checkboxes.Length].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = sText;
            Debug.Log("Graphic checkbox: " + (i - checkboxes.Length));
        }
    }
        

    public void ChangeSection(int section)
    {
        gameSection.SetActive(section == 0);       
        keybindSection.SetActive(section == 1);
    }

    #region Input Rebinding

    public void UpdateKeybind(string kb)
    {

        // Make sure input is disabled
        if (remapNextFrame == "")
        {
            inputs.Main.Disable();
            remapNextFrame = kb;
            return;
        }

        //MusicManager.PlayButtonSound();

        var display = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
        TimeoutUI.SetActive(true);
        StartCoroutine(nameof(OptionMenu.InputTimeout));

        using (new InputActionRebindingExtensions.RebindingOperation())
        {
            StringToInputAction(remapNextFrame).PerformInteractiveRebinding()
                .WithTimeout(inputTimeout)
                .OnCancel(result => { TimeoutUI.SetActive(false); })
                .WithTargetBinding(0)
                .WithControlsExcluding("<Mouse>/leftButton")
                .WithControlsExcluding("<Pointer>/press")
                .OnComplete(result =>
                {
                    UpdateKeyDisplay(display, result.action.controls[0].displayName);
                    //CheckForDuplicate();
                    TimeoutUI.SetActive(false);
                })
                .Start();
            EventSystem.current.SetSelectedGameObject(null);
            remapNextFrame = "";
        }
    }

    private IEnumerator InputTimeout()
    {
        Image timeoutbar = TimeoutUI.transform.GetChild(1).GetComponent<Image>();
        float timer = inputTimeout;

        while (TimeoutUI.activeInHierarchy)
        {
            timer -= Time.deltaTime;
            timeoutbar.fillAmount = timer / inputTimeout;
            yield return null;
        }
    }

    private void CheckForDuplicate()
    {
       

        IEnumerable<InputAction> duplicatekeys = new List<InputAction>();
        foreach (var input in inputs)
        {
            duplicatekeys = duplicatekeys.Concat(inputs.Where(i => i != input &&
                i.bindings[0].overridePath == input.bindings[0].overridePath && i.bindings[0].overridePath != null));

        }

        IEnumerable<InputAction> inputActions = duplicatekeys as InputAction[] ?? duplicatekeys.ToArray();
        duplicateWarning.SetActive(inputActions.Count() > 1);
    }

    public void UpdateKeyDisplay(TextMeshProUGUI display, string text)
    {
        if (display == null)
        {
            return;
        }

        display.text = Regex.Replace(text, "^(<.*>)\\/", "");
    }

    private InputAction StringToInputAction(string input)
    {
        switch (input)
        {
            case "Forward": return inputs.Main.Forward;
            case "Backwards": return inputs.Main.Backwards;
            case "Right": return inputs.Main.Right;
            case "Left": return inputs.Main.Left;
            case "Jump": return inputs.Main.Jump;
            case "Fire": return inputs.Main.Fire1;
            case "RecenterCamera": return inputs.Main.RecenterCamera;
            case "SwitchAbilityForward": return inputs.Main.SwitchAbilityForward;
            case "SwitchAbilityBackward": return inputs.Main.SwitchAbilityBackward;
            case "UseAbility": return inputs.Main.UseAbility;
            case "Interact": return inputs.Main.Interact;
            default: return null;
        }
    }

    #endregion

    private void CloseOptions(InputAction.CallbackContext obj)
    {
        CloseOptions();
    }

    public void CloseOptions()
    {
        Debug.Log(GameManager.Instance.inMainMenu);

        //if (gameObject.activeInHierarchy)
        //{
        //    if (GameManager.Instance.inMainMenu)
        //        GameManager.Instance.OpenMenu(5);
        //    else
        //        GameManager.Instance.OpenMenu(2);
        //}
            
    }
        
}

