using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class SettingsManager {
    private static SettingsObject CurrentSettings;

    public static SettingsObject GetSettings {
        get {
            if (CurrentSettings == null) {
                SettingsRead();
            }

            return CurrentSettings;
        }
    }

    public static void SetSettings(SettingsObject settings) {
        CurrentSettings = settings;
    }

    private static void CreateDefaults() {

        Debug.Log("[SettingsManager] Creating default settings...");

        PlayerInput defaultInput = new PlayerInput();
        CurrentSettings = new SettingsObject {
            Sensitivity = 500,
            SoundEffectsVolume = 70,
            MusicVolume = 50,
            RenderDistance = 250,
            InputSettings = new InputSetting {
                Forward = defaultInput.Main.Forward.bindings[0].path,
                Backwards = defaultInput.Main.Backwards.bindings[0].path,
                Left = defaultInput.Main.Left.bindings[0].path,
                Right = defaultInput.Main.Right.bindings[0].path,
                Jump = defaultInput.Main.Jump.bindings[0].path,
                Fire1 = defaultInput.Main.Fire1.bindings[0].path,
                RecenterCamera = defaultInput.Main.RecenterCamera.bindings[0].path,
                SwitchAbilityForward = defaultInput.Main.SwitchAbilityForward.bindings[0].path,
                SwitchAbilityBackward = defaultInput.Main.SwitchAbilityBackward.bindings[0].path,
                UseAbility = defaultInput.Main.UseAbility.bindings[0].path,
                Interact = defaultInput.Main.Interact.bindings[0].path,
            },

        GraphicSettings = new GraphicSetting {
            //Bloom = 20,
            ResolutionWidth = 1920,
            ResolutionHeight = 1080,
            FullScreen = true,
            //Antialiasing = 1,
            //Filtering = true,
            //TextureQuality = 4,
            Vsync = false,
            FPS = 300,
            }
        };

        SettingsWrite();
        Debug.Log("[SettingsManager] Created new default settings");
    }

    /// <summary>
    /// Read settings from XML file
    /// </summary>
    public static void SettingsRead() {
        string path = Application.persistentDataPath + "\\settings.xml";
        Debug.Log("Reading Settings from: " + path);

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingsObject));
        if (!File.Exists(path)) {
            CreateDefaults();
            return;
        }

        SettingsObject settingData;
        try {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                settingData = xmlSerializer.Deserialize(fs) as SettingsObject;
            }
        } catch {
            settingData = null;
        }

        if (settingData == null) {
            CreateDefaults();
            return;
        }

        CurrentSettings = settingData;
    }

    /// <summary>
    /// Write current settings to a XML file
    /// </summary>
    public static void SettingsWrite() {
        string path = Application.persistentDataPath + "\\settings.xml";
        Debug.Log("Writing settings file: " + path);

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingsObject));
        using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write)) {
            xmlSerializer.Serialize(fs, GetSettings);
        }
    }

    public static void RebindKeys() {

        PlayerInput inputs = GameManager.Instance.inputs;

        //Rebind Keys to match settings
        try {
            //Make sure input is disabled when rebinding
            inputs.Disable();

            inputs.Main.Forward.ApplyBindingOverride(0, GetSettings.InputSettings.Forward);
            inputs.Main.Left.ApplyBindingOverride(0, GetSettings.InputSettings.Left);
            inputs.Main.Backwards.ApplyBindingOverride(0, GetSettings.InputSettings.Backwards);
            inputs.Main.Right.ApplyBindingOverride(0, GetSettings.InputSettings.Right);
            inputs.Main.Jump.ApplyBindingOverride(0, GetSettings.InputSettings.Jump);
            inputs.Main.Fire1.ApplyBindingOverride(0, GetSettings.InputSettings.Fire1);
            inputs.Main.RecenterCamera.ApplyBindingOverride(0, GetSettings.InputSettings.RecenterCamera);
            inputs.Main.SwitchAbilityForward.ApplyBindingOverride(0, GetSettings.InputSettings.SwitchAbilityForward);
            inputs.Main.SwitchAbilityBackward.ApplyBindingOverride(0, GetSettings.InputSettings.SwitchAbilityBackward);
            inputs.Main.UseAbility.ApplyBindingOverride(0, GetSettings.InputSettings.UseAbility);
            inputs.Main.Interact.ApplyBindingOverride(0, GetSettings.InputSettings.Interact);
            inputs.Enable();
        } catch (Exception e) {
            Debug.LogWarning("Failed to overwrite bindings! - " + e);
        }
    }

    public static void SetLeaderboardPreference(int value) {
        //CurrentSettings.LeaderboardFilter = value;
        SettingsWrite();
    }

    public static void ApplySettings()
    {
        Debug.Log("[SettingsMananger] LOADING AND APPLYING SETTINGS");

        SettingsObject settings = GetSettings;
        //SoundManager.Instance.SetAudioLevels(settings.SoundEffectsVolume, settings.MusicVolume); //TODO: Set sound level
        GraphicsSettings.Reload();
        MouseLook.sensitivity = settings.Sensitivity * 0.01f;
        Camera.main.farClipPlane = settings.RenderDistance;
        RebindKeys();

    }

}

public class GraphicsSettings {
    private static void SetAnisotropicFiltering(bool value) {
        if (value) {
            if (QualitySettings.anisotropicFiltering != AnisotropicFiltering.ForceEnable)
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        } else {
            if (QualitySettings.anisotropicFiltering != AnisotropicFiltering.Disable)
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        }

        //QualitySettings.anisotropicFiltering = value ? AnisotropicFiltering.ForceEnable : AnisotropicFiltering.Disable;
    }

    private static void SetVsync(bool value) {

        if (QualitySettings.vSyncCount != Convert.ToInt16(value)) {
            QualitySettings.vSyncCount = Convert.ToInt16(value);
        }

    }

    private static void SetResolution(int width, int height, bool fullScreen) {

        if (Screen.width != width || Screen.height != height ||
            Screen.fullScreen != fullScreen)
        {
            Screen.SetResolution(width, height, fullScreen);
        }

    }

    private static void SetTextureQuality(int value) {
        // In the quality settings 0 is full quality textures, while 3 is the lowest.

        int quality = 3 - value;
        if (QualitySettings.globalTextureMipmapLimit != quality) {
            QualitySettings.globalTextureMipmapLimit = quality;
        }

    }

    private static void SetFpsLimit(int value) {
        if (Application.targetFrameRate != value) {
            Application.targetFrameRate = value;
        }
    }

    public static void Reload() {
        SettingsObject settings = SettingsManager.GetSettings;
        GraphicSetting gs = settings.GraphicSettings;

        Debug.Log("RESOLUTION: " + gs.ResolutionWidth + " x " + gs.ResolutionHeight);

        SetResolution(gs.ResolutionWidth, gs.ResolutionHeight, gs.FullScreen);
        SetVsync(gs.Vsync);
        //SetTextureQuality(gs.TextureQuality);
        //SetAnisotropicFiltering(gs.Filtering);
        SetFpsLimit(gs.FPS);
    }

   

    

}

