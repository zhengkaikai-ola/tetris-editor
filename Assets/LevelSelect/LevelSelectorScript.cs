using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelSelectorScript : MonoBehaviour
{
    public Dropdown SelectModeDropDown;
    public InputField FilePathInputField;
    public Dropdown LevelDropdown;
    public SingleModeConfig SingleModeConfig = new SingleModeConfig();
    public UIEventCenter UIEventCenter;
    private string configDir = "";
    public int currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        LevelDropdown.options.Clear();
        for (var index = 1; index < 50; index++) LevelDropdown.options.Add(new Dropdown.OptionData("" + index));
        LevelDropdown.onValueChanged.AddListener(onLevelChanged);
        onEditModeChange();
        OnFilePathChanged();
        Load();
        onLevelChanged(0);
    }

    public void Load()
    {
        string content = "";
        try
        {
            content = File.ReadAllText(configDir + "/config.json");
        }
        catch (Exception)
        {
            Debug.LogFormat("Read Config Error ,Path {0}", configDir + "/config.json");
        }

        if (content.Length == 0)
        {
            return;
        }

        var config = JsonConvert.DeserializeObject<SingleModeConfig>(content);
        SingleModeConfig = config;
    }

    public void Save()
    {
        var levelConfig = SingleModeConfig.LevelConfigs.GetValueOrDefault(currentLevel,
            new TetrisLevelConfig());
        SingleModeConfig.LevelConfigs[currentLevel] = levelConfig;
        UIEventCenter.InitMapEditorScript.SaveConfig(levelConfig);
        UIEventCenter.TemplateEditorScript.SaveConfig(levelConfig);
        UIEventCenter.GenerateBlockEditorScript.SaveConfig(levelConfig);
        UIEventCenter.TaskEditorScript.SaveConfig(levelConfig);
        var jsonConfig = JsonConvert.SerializeObject(SingleModeConfig);
        File.WriteAllText(configDir + "/config.json", jsonConfig);
    }

    private void onLevelChanged(int index)
    {
        var lastLevel = currentLevel;
        currentLevel = index + 1;
        if (currentLevel != lastLevel)
        {
            var levelConfig = SingleModeConfig.LevelConfigs.GetValueOrDefault(currentLevel,
                new TetrisLevelConfig());
            SingleModeConfig.LevelConfigs[currentLevel] = levelConfig;
            UIEventCenter.InitMapEditorScript.DestroyOldAndLoadNewLevel(levelConfig);
            UIEventCenter.TemplateEditorScript.LoadConfig(levelConfig);
            UIEventCenter.GenerateBlockEditorScript.LoadConfig(levelConfig);
            UIEventCenter.TaskEditorScript.LoadConfig(levelConfig);
        }
    }

    public void Close()
    {
        Application.Quit(0);
    }

    public void OnFilePathChanged()
    {
        if (FilePathInputField.text.Length != 0) configDir = FilePathInputField.text;
    }

    public void onEditModeChange()
    {
        switch (SelectModeDropDown.value)
        {
            case 0:
                UIEventCenter.InitMapEditorScript.transform.gameObject.SetActive(true);
                UIEventCenter.TaskEditorScript.transform.gameObject.SetActive(false);
                UIEventCenter.GenerateBlockEditorScript.transform.gameObject.SetActive(false);
                UIEventCenter.TemplateEditorScript.transform.gameObject.SetActive(true);
                return;
            case 1:
                UIEventCenter.InitMapEditorScript.transform.gameObject.SetActive(false);
                UIEventCenter.TaskEditorScript.transform.gameObject.SetActive(true);
                UIEventCenter.GenerateBlockEditorScript.transform.gameObject.SetActive(false);
                UIEventCenter.TemplateEditorScript.transform.gameObject.SetActive(true);
                return;
            case 2:
                UIEventCenter.InitMapEditorScript.transform.gameObject.SetActive(false);
                UIEventCenter.TaskEditorScript.transform.gameObject.SetActive(false);
                UIEventCenter.GenerateBlockEditorScript.transform.gameObject.SetActive(true);
                UIEventCenter.TemplateEditorScript.transform.gameObject.SetActive(true);
                return;
        }
    }
}