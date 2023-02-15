using System.Collections;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SkillEditorScript : MonoBehaviour
{
    public UIEventCenter UIEventCenter;
    public Dropdown SelectTemplateDropdown;
    public InputField NameText;
    public InputField ClearRowsText;
    public InputField leftMoveRowsText;
    public InputField rightMoveRowsText;
    public InputField fireworkCountText;
    public InputField fireworkWidthText;
    public InputField fireworkHeightText;
    public Dictionary<int, SkillConfig> Templates = new();
    public SkillConfig CurrentTemplate;
    private int currentTemplateIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // SelectTemplateDropdown.options.Clear();
        // for (var index = 1; index < 50; index++) SelectTemplateDropdown.options.Add(new Dropdown.OptionData("" + index));
        SelectTemplateDropdown.onValueChanged.AddListener(this.onSelectTemplate);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadConfig(TetrisLevelConfig currentLevelConfig)
    {
        SelectTemplateDropdown.options.Clear();
        var bind = UIEventCenter.LevelSelectorScript.SingleModeConfig.SkillConfigs;
        Templates.Clear();
        // SelectTemplateDropdown.options.RemoveRange(1, SelectTemplateDropdown.options.Count - 1);
        foreach (var kv in bind)
        {
            Debug.Log("LoadConfig..." + kv.Key + ", " + kv.Value.name);
            var id = kv.Key;
            var blockDesc = kv.Value;
            Templates[id] = blockDesc;
            SelectTemplateDropdown.options.Add(
                new DropDownOptionSkillData(getOptionText(id, blockDesc), id));
        }

        onSelectTemplate(0);
    }
    private void onSelectTemplate(int opindex)
    {
        if (opindex == 0)
        {
            // return;
        }

        var selectTemplateOptionData = SelectTemplateDropdown.options[opindex] as DropDownOptionSkillData;
        if (selectTemplateOptionData == null)
        {
            return;
        }

        var templateIndex = selectTemplateOptionData.data;
        var ok = this.Templates.TryGetValue(templateIndex, out var blockDesc);
        if (!ok)
        {
            Debug.Log("没有找到对应模板");
            return;
        }

        selectTemplateOptionData.text = getOptionText(templateIndex,blockDesc);
        CurrentTemplate = blockDesc;
        currentTemplateIndex = opindex;
        NameText.text = blockDesc.name;
        ClearRowsText.text = blockDesc.clear_rows + "";
        leftMoveRowsText.text = blockDesc.left_rows + "";
        rightMoveRowsText.text = blockDesc.right_rows + "";
        fireworkCountText.text = blockDesc.fireworks_count + "";
        fireworkWidthText.text = blockDesc.fireworks_width + "";
        fireworkHeightText.text = blockDesc.fireworks_height + "";
    }
    
    public void ConfirmModifyEvent()
    {
        if (CurrentTemplate == null)
        {
            Debug.Log("当前目标为空");
            return;
        }

        ApplyEditorDataToDesc(CurrentTemplate);
        var optionText = getOptionText(currentTemplateIndex, CurrentTemplate);
        // SelectTemplateDropdown.options[currentTemplateIndex].text = optionText;
        // SelectTemplateDropdown.captionText.text = optionText;
    }


    private void ApplyEditorDataToDesc(SkillConfig blockDesc)
    {
        Debug.Log("ApplyEditorDataToDesc....."  + blockDesc.id + ", " + NameText.text);
        blockDesc.name = NameText.text;
        blockDesc.clear_rows = ParseUtil.Parse(ClearRowsText.text);
        blockDesc.left_rows = ParseUtil.Parse(leftMoveRowsText.text);
        blockDesc.right_rows = ParseUtil.Parse(rightMoveRowsText.text);
        blockDesc.fireworks_count = ParseUtil.Parse(fireworkCountText.text);
        blockDesc.fireworks_width = ParseUtil.Parse(fireworkWidthText.text);
        blockDesc.fireworks_height = ParseUtil.Parse(fireworkHeightText.text);
    }

    public void SaveConfig(TetrisLevelConfig tetrisLevelConfig)
    {
        LevelSelectorScript levelSelectorScript = UIEventCenter.LevelSelectorScript;
        var SingleModeConfig = levelSelectorScript.SingleModeConfig;
        foreach (var kv in UIEventCenter.SkillEditorScript.Templates)
        {
            var templateId = kv.Key;
            var desc = kv.Value;
            SingleModeConfig.SkillConfigs[templateId] = desc;
        }
    }

    private static string getOptionText(int id, SkillConfig blockDesc)
    {
        return $"{id}";
    }
}