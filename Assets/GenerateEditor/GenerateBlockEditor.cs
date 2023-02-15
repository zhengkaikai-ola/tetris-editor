using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class GenerateBlockEditor : MonoBehaviour
{
    public ShapeCells ShapeCells;
    public Dropdown BlockTypeDropDown;
    public Dropdown IndexDropDown;
    public UIEventCenter UIEventCenter;
    private TetrisLevelConfig CurrentLevelConfig;
    private int currentIndex;
    private GameTypeEnum currGameType;

    // Start is called before the first frame update
    private void Start()
    {
        foreach (var kv in SingleTetrisShapes.HardBlockTypeToShapes)
        {
            var GameTypeEnum = kv.Key;

            var op = new Dropdown.OptionData(GameTypeEnum.ToString());

            BlockTypeDropDown.options.Add(op);
        }

        BlockTypeDropDown.onValueChanged.AddListener(onBlockTypeChanged);
        for (var index = 1; index < 40; index++)
        {
            IndexDropDown.options.Add(new Dropdown.OptionData(index.ToString()));
        }

        IndexDropDown.onValueChanged.AddListener(onIndexChanged);
    }

    public void onBlockTypeChanged(int index)
    {
        if (index == 0)
        {
            Debug.Log("Block Type Not Set");
            return;
        }

        var ok = GameTypeEnum.TryParse(BlockTypeDropDown.captionText.text, out GameTypeEnum gtype);
        if (!ok)
        {
            Debug.LogFormat("Parse Block Type Error {0}", BlockTypeDropDown.captionText.text);
            return;
        }

        //ShapeCells数据重置
        currGameType = gtype;
        rotationIndex = 0;
        ResetShapeCells(currGameType, rotationIndex);

        var allpos = SingleTetrisShapes.HardBlockTypeToShapes[currGameType][rotationIndex];
        for (var order = 0; order < allpos.Length; order++)
        {
            var pos = allpos[order];
            var cell = ShapeCells.CellMap[Mathf.RoundToInt(pos.y)][Mathf.RoundToInt(pos.x)];
            cell.SetBlockData(0, BlockType.PlayerFixed);
        }
    }

    public void onIndexChanged(int index)
    {
        if (index == 0) return;

        if (currentIndex == index) return;

        currentIndex = index;
        loadIndexConfig(currentIndex);
    }

    public void OnCellClick(TetrisCellData cellData)
    {
        Debug.LogFormat("{0},{1}", cellData.X, cellData.Y);
        var currentTemplate = UIEventCenter.TemplateEditorScript.CurrentTemplate;
        if (currentTemplate == null) return;
        if (!cellValid(cellData.X, cellData.Y, currGameType, rotationIndex))
        {
            Debug.Log("此处无法指定数据");
            return;
        }

        if (currentTemplate.block_type == BlockType.Bot)
        {
            for (var deltax = 0; deltax < currentTemplate.arg0; deltax++)
            {
                for (var deltay = 0; deltay < currentTemplate.arg1; deltay++)
                {
                    var x = cellData.X + deltax;
                    var y = cellData.Y + deltay;
                    if (!cellValid(x, y, currGameType, rotationIndex))
                    {
                        Debug.Log("此处不能放机器人");
                        return;
                    }
                }
            }

            cellData.SetBlockData(currentTemplate.template_id, currentTemplate.block_type);
            // for (var deltax = 0; deltax < currentTemplate.arg0; deltax++)
            // {
            //     for (var deltay = 0; deltay < currentTemplate.arg1; deltay++)
            //     {
            //         var x = cellData.X + deltax;
            //         var y = cellData.Y + deltay;
            //
            //         if (ShapeCells.CellMap.TryGetValue(y, out var cs))
            //         {
            //             if (cs.TryGetValue(x, out var desc))
            //             {
            //                 desc.SetBlockData(currentTemplate.template_id, currentTemplate.block_type);
            //             }
            //         }
            //     }
            // }
        }
        else
        {
            cellData.SetBlockData(currentTemplate.template_id, currentTemplate.block_type);
        }
    }

    private int rotationIndex = 0;

    public void Rotate()
    {
        var ok = GameTypeEnum.TryParse(BlockTypeDropDown.captionText.text, out GameTypeEnum gtype);
        if (!ok)
        {
            Debug.LogFormat("Parse Block Type Error {0}", BlockTypeDropDown.captionText.text);
            return;
        }

        var typeCnt = SingleTetrisShapes.HardBlockTypeToShapes[currGameType].Length;
        currGameType = gtype;
        rotationIndex++;
        rotationIndex %= typeCnt;
        ResetShapeCells(currGameType, rotationIndex);
        var allpos = SingleTetrisShapes.HardBlockTypeToShapes[currGameType][rotationIndex];
        for (var order = 0; order < allpos.Length; order++)
        {
            var pos = allpos[order];
            var cell = ShapeCells.CellMap[Mathf.RoundToInt(pos.y)][Mathf.RoundToInt(pos.x)];
            cell.SetBlockData(0, BlockType.PlayerFixed);
        }
    }

    public void DeleteEvent()
    {
        CurrentLevelConfig.GenerateConfigs.Remove(currentIndex);
        ResetShapeCells(GameTypeEnum.None, 0);
    }

    private void loadIndexConfig(int index)
    {
        if (CurrentLevelConfig == null) return;

        var generateBlockConfig =
            CurrentLevelConfig.GenerateConfigs.GetValueOrDefault(index, new GenerateBlockConfig());
        CurrentLevelConfig.GenerateConfigs[index] = generateBlockConfig;
        var recordBlockType = (GameTypeEnum)generateBlockConfig.GameType;
        ResetShapeCells(recordBlockType, generateBlockConfig.GameIndex);
        SetCellDataFromConfig(generateBlockConfig);
    }

    private void SetCellDataFromConfig(GenerateBlockConfig generateBlockConfig)
    {
        if (generateBlockConfig.GameType == (int)GameTypeEnum.None)
        {
            return;
        }

        var allpos =
            SingleTetrisShapes.HardBlockTypeToShapes[(GameTypeEnum)generateBlockConfig.GameType][
                generateBlockConfig.GameIndex];
        for (var order = 0; order < allpos.Length; order++)
        {
            var pos = allpos[order];
            var typeId = generateBlockConfig.GetValueByOrder(order);
            var cell = ShapeCells.CellMap[Mathf.RoundToInt(pos.y)][Mathf.RoundToInt(pos.x)];
            var ok = UIEventCenter.LevelSelectorScript.SingleModeConfig.TemplateDescBind.TryGetValue(typeId,
                out var desc);
            if (!ok)
            {
                continue;
            }

            cell.SetBlockData(desc.template_id, desc.block_type);
        }
    }

    private void ResetShapeCells(GameTypeEnum gameTypeEnum, int index)
    {
        foreach (var kv in ShapeCells.CellMap)
        {
            var y = kv.Key;
            var rmap = kv.Value;
            foreach (var kv2 in rmap)
            {
                var x = kv2.Key;
                var cellData = kv2.Value;
                if (!cellValid(x, y, gameTypeEnum, index))
                {
                    cellData.SetBlockData(0, BlockType.None);
                }
                else
                {
                    cellData.SetBlockData(0, BlockType.PlayerFixed);
                }
            }
        }
    }

    private void saveIndexConfig(GenerateBlockConfig generateBlockConfig)
    {
        if (currGameType == GameTypeEnum.None)
        {
            return;
        }

        generateBlockConfig.GameType = (int)currGameType;
        generateBlockConfig.GameIndex = rotationIndex;

        var shapes = new SingleTetrisShapes();
        var allpos = SingleTetrisShapes.HardBlockTypeToShapes[currGameType][generateBlockConfig.GameIndex];
        for (var order = 0; order < allpos.Length; order++)
        {
            var pos = allpos[order];
            var x = Mathf.RoundToInt(pos.x);
            var y = Mathf.RoundToInt(pos.y);
            var cell = ShapeCells.CellMap[y][x];
            if (cell.BlockType != BlockType.None) generateBlockConfig.SetValueByOrder(order, cell.typeId);
        }
    }


    public void LoadConfig(TetrisLevelConfig currentLevelConfig)
    {
        CurrentLevelConfig = currentLevelConfig;
        loadIndexConfig(currentIndex);
    }

    public void SaveConfig(TetrisLevelConfig tetrisLevelConfig)
    {
        if (currentIndex == 0)
        {
            return;
        }

        var generateBlockConfig =
            tetrisLevelConfig.GenerateConfigs.GetValueOrDefault(currentIndex, new GenerateBlockConfig());
        tetrisLevelConfig.GenerateConfigs[currentIndex] = generateBlockConfig;
        saveIndexConfig(generateBlockConfig);
    }

    public bool cellValid(int x, int y, GameTypeEnum gameTypeEnum, int index)
    {
        if (gameTypeEnum == GameTypeEnum.None)
        {
            return false;
        }

        var allpos = SingleTetrisShapes.HardBlockTypeToShapes[gameTypeEnum][rotationIndex];
        var find = false;
        foreach (var pos in allpos)
        {
            if (Math.Abs(x - pos.x) < float.Epsilon && Math.Abs(y - pos.y) < float.Epsilon)
            {
                return true;
            }
        }

        return false;
    }
}