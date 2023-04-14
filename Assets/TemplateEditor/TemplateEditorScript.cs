using System;
using System.Collections.Generic;
using DefaultNamespace;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TemplateEditorScript : MonoBehaviour
{
    public Dropdown SelectTemplateDropdown;
    public InputField NameText;
    public InputField BasicTemplateIdText;
    public Dropdown BlockTypeText;
    public InputField IceHpText;
    public InputField Arg0Text;
    public InputField Arg1Text;
    public InputField Arg2Text;
    public InputField Arg3Text;
    public UIEventCenter UIEventCenter;
    public Dictionary<int, BlockDesc> Templates = new();
    public BlockDesc CurrentTemplate;
    public bool MoveMode = false;
    private int currentTemplateIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var b in Enum.GetValues(typeof(BlockType)))
        {
            var blockType = (BlockType)b;
            BlockTypeText.options.Add(new DropDownOptionWithData(blockType.ToString(), (int)blockType));
        }

        SelectTemplateDropdown.onValueChanged.AddListener(this.onSelectTemplate);
    }


    public void CreateTemplateEvent()
    {
        var newTemplateId = this.Templates.Count + 1;
        var blockDesc = new BlockDesc();
        blockDesc.template_id = newTemplateId;
        Templates[newTemplateId] = blockDesc;
        ApplyEditorDataToDesc(blockDesc);
        SelectTemplateDropdown.options.Add(
            new DropDownOptionWithData($"{newTemplateId}{blockDesc.name}", newTemplateId));
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
        SelectTemplateDropdown.options[currentTemplateIndex].text =
            optionText;
        SelectTemplateDropdown.captionText.text = optionText;
    }

    public void DeleteTemplateEvent()
    {
        if (CurrentTemplate == null)
        {
            Debug.Log("当前无选中模板");
            return;
        }

        Templates.Remove(CurrentTemplate.template_id);
        var deleteIndex = -1;
        for (var index = 0; index < SelectTemplateDropdown.options.Count; index++)
        {
            var selectOptionData = SelectTemplateDropdown.options[index] as DropDownOptionWithData;
            if (selectOptionData == null)
            {
                continue;
            }

            if (CurrentTemplate.template_id == selectOptionData.data)
            {
                deleteIndex = index;
                break;
            }
        }

        if (deleteIndex != -1)
        {
            SelectTemplateDropdown.options.RemoveAt(deleteIndex);
        }

        SelectTemplateDropdown.value = 0;
    }


    private void ApplyEditorDataToDesc(BlockDesc blockDesc)
    {
        blockDesc.name = NameText.text;
        blockDesc.basic_template_id = ParseUtil.Parse(BasicTemplateIdText.text);
        var optionData = BlockTypeText.options[BlockTypeText.value];
        if (optionData is DropDownOptionWithData optionWithData)
        {
            blockDesc.block_type = (BlockType)optionWithData.data;
        }

        blockDesc.ice_hp = ParseUtil.Parse(IceHpText.text);
        blockDesc.arg0 = ParseUtil.Parse(Arg0Text.text);
        blockDesc.arg1 = ParseUtil.Parse(Arg1Text.text);
        blockDesc.arg2 = ParseUtil.Parse(Arg2Text.text);
        blockDesc.arg3 = ParseUtil.Parse(Arg3Text.text);
    }

    private void onSelectTemplate(int opindex)
    {
        if (opindex == 0)
        {
            return;
        }

        var selectTemplateOptionData = SelectTemplateDropdown.options[opindex] as DropDownOptionWithData;
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

        selectTemplateOptionData.text = getOptionText(templateIndex, blockDesc);
        CurrentTemplate = blockDesc;
        currentTemplateIndex = templateIndex;
        NameText.text = blockDesc.name;
        for (var index = 0; index < BlockTypeText.options.Count; index++)
        {
            var opData = BlockTypeText.options[index] as DropDownOptionWithData;
            if (opData == null)
            {
                continue;
            }

            if (opData!.data == (int)blockDesc.block_type)
            {
                BlockTypeText.value = index;
            }
        }

        BasicTemplateIdText.text = blockDesc.basic_template_id.ToString();
        IceHpText.text = blockDesc.ice_hp.ToString();
        Arg0Text.text = blockDesc.arg0.ToString();
        Arg1Text.text = blockDesc.arg1.ToString();
        Arg2Text.text = blockDesc.arg2.ToString();
        Arg3Text.text = blockDesc.arg3.ToString();
    }


    public void SaveConfig(TetrisLevelConfig tetrisLevelConfig)
    {
        LevelSelectorScript levelSelectorScript = UIEventCenter.LevelSelectorScript;
        var SingleModeConfig = levelSelectorScript.SingleModeConfig;
        foreach (var kv in UIEventCenter.TemplateEditorScript.Templates)
        {
            var templateId = kv.Key;
            var desc = kv.Value;
            SingleModeConfig.TemplateDescBind[templateId] = desc;
        }
    }

    public void LoadConfig(TetrisLevelConfig currentLevelConfig)
    {
        var bind = UIEventCenter.LevelSelectorScript.SingleModeConfig.TemplateDescBind;
        Templates.Clear();
        SelectTemplateDropdown.options.RemoveRange(1, SelectTemplateDropdown.options.Count - 1);
        foreach (var kv in bind)
        {
            var id = kv.Key;
            var blockDesc = kv.Value;
            Templates[id] = blockDesc;
            SelectTemplateDropdown.options.Add(
                new DropDownOptionWithData(getOptionText(id, blockDesc), id));
        }

        SelectTemplateDropdown.value = 0;
        onSelectTemplate(0);
    }

    private static string getOptionText(int id, BlockDesc blockDesc)
    {
        return $"{id}{blockDesc.name}";
    }
}