using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPauseMenu : MonoBehaviour
{

    public TextMeshProUGUI runeCountText;
    public TextMeshProUGUI crystalCountText;

    private void Awake()
    {
        GameManager.Instance.pauseUI = this;
        EventManager.CollectablesUpdated += OnCollectableUpdated;
        
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        EventManager.CollectablesUpdated -= OnCollectableUpdated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollectableUpdated()
    {
        runeCountText.SetText(UICollectables.runeCount.ToString());
        crystalCountText.SetText(UICollectables.crystalCount.ToString());
    }


    public void QuitButtonPressed()
    {
        Application.Quit();
    }
    public void ResumeButton()
    {
        Debug.Log("RESUME?");
        GameManager.Instance.Resume();
    }
}
