using System.Collections;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class TaskEditorScript : MonoBehaviour
{
    public UIEventCenter UIEventCenter;
    public ConditionEditScript PassCondition;
    public ConditionEditScript StarCondition;
    public InputField FullGoldInput;
    public InputField SkillAwardInput;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadConfig(TetrisLevelConfig tetrisLevelConfig)
    {
        LevelSelectorScript levelSelectorScript = UIEventCenter.LevelSelectorScript;
        var SingleModeConfig = levelSelectorScript.SingleModeConfig;
        var ex = SingleModeConfig.LevelConfigs.TryGetValue(levelSelectorScript.currentLevel,
            out var currentLevelConfig);
        if (!ex)
        {
            return;
        }

        LoadConditionConfig(PassCondition, currentLevelConfig.PassCondition);
        LoadConditionConfig(StarCondition, currentLevelConfig.StarCondition);
        FullGoldInput.text = $"{currentLevelConfig.FullStarGoldCnt}";
        SkillAwardInput.text = DicToString(currentLevelConfig.SkillAwardCnt);
    }


    public void SaveConfig(TetrisLevelConfig tetrisLevelConfig)
    {
        LevelSelectorScript levelSelectorScript = UIEventCenter.LevelSelectorScript;
        var SingleModeConfig = levelSelectorScript.SingleModeConfig;
        var ex = SingleModeConfig.LevelConfigs.TryGetValue(levelSelectorScript.currentLevel,
            out var currentLevelConfig);
        if (!ex)
        {
            return;
        }

        SaveConditionConfig(PassCondition, currentLevelConfig.PassCondition);
        SaveConditionConfig(StarCondition, currentLevelConfig.StarCondition);
        currentLevelConfig.FullStarGoldCnt = ParseUtil.Parse(FullGoldInput.text);
        currentLevelConfig.SkillAwardCnt = StringToDic(SkillAwardInput.text);
    }


    private void LoadConditionConfig(ConditionEditScript conditionEdit, LevelCondition config)
    {
        conditionEdit.UseStep.text = config.UseStep.ToString();
        conditionEdit.KillBotNum.text = config.KillBotNum.ToString();
        conditionEdit.AttackBotTimesOnce.text = config.AttackBotTimesOnce.ToString();
        conditionEdit.TotalCleanRowNum.text = config.TotalCleanRowNum.ToString();
        conditionEdit.CleanRowNumOnce.text = config.CleanRowNumOnce.ToString();
        conditionEdit.TotalCollectNums.text = DicToString(config.TotalCollectNums);
        conditionEdit.CollectNumOnce.text = DicToString(config.CollectNumOnce);
    }


    private void SaveConditionConfig(ConditionEditScript edit, LevelCondition config)
    {
        config.UseStep = ParseUtil.Parse(edit.UseStep.text);
        config.KillBotNum = ParseUtil.Parse(edit.KillBotNum.text);
        config.AttackBotTimesOnce = ParseUtil.Parse(edit.AttackBotTimesOnce.text);
        config.TotalCleanRowNum = ParseUtil.Parse(edit.TotalCleanRowNum.text);
        config.CleanRowNumOnce = ParseUtil.Parse(edit.CleanRowNumOnce.text);
        config.TotalCollectNums = StringToDic(edit.TotalCollectNums.text);
        config.CollectNumOnce = StringToDic(edit.CollectNumOnce.text);
    }

    public string DicToString(Dictionary<int, int> dictionary)
    {
        var b = new StringBuilder();
        foreach (var kv in dictionary)
        {
            var typeId = kv.Key;
            var typeCnt = kv.Value;
            b.AppendFormat("{0}={1},", typeId, typeCnt);
        }

        return b.ToString();
    }

    public Dictionary<int, int> StringToDic(string s)
    {
        var dic = new Dictionary<int, int>();
        var kvs = s.Split(",");
        foreach (var kv in kvs)
        {
            if (kv.Trim().Length == 0)
            {
                continue;
            }

            var ss = kv.Split("=");
            var k = ParseUtil.Parse(ss[0]);
            var v = ParseUtil.Parse(ss[1]);
            dic[k] = v;
        }

        return dic;
    }
}