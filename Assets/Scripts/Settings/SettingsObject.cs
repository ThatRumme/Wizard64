using System;
// ReSharper disable InconsistentNaming

[Serializable]
public class SettingsObject
{
    public int SoundEffectsVolume;
    public int MusicVolume;
    public int Sensitivity;
    public int RenderDistance;
    public InputSetting InputSettings;
    public GraphicSetting GraphicSettings;

    //public int LeaderboardFilter;
}

[Serializable]
public class InputSetting
{
    //Movement
    public string Forward;
    public string Backwards;
    public string Right;
    public string Left;
    public string Jump;

    //Controls
    public string Fire1;
    public string RecenterCamera;
    public string SwitchAbilityForward;
    public string SwitchAbilityBackward;
    public string UseAbility;
    public string Interact;
}

public class GraphicSetting
{
    public int ResolutionHeight;
    public int ResolutionWidth;
    public bool FullScreen;
    public bool Vsync;
    public int FPS;
}
