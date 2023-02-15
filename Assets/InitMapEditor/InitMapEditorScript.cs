using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class InitMapEditorScript : MonoBehaviour
{
    //预加载prefab
    private GameObject initMapPrefab;
    private CellGroupScript CellGroupScript;
    private GameObject CurrentMapObj;

    public UIEventCenter UIEventCenter;

    // Start is called before the first frame update
    void Start()
    {
        initMapPrefab = Resources.Load<GameObject>("InitMap");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DestroyOldAndLoadNewLevel(TetrisLevelConfig currentLevelConfig)
    {
        if (initMapPrefab == null)
        {
            initMapPrefab = Resources.Load<GameObject>("InitMap");
        }

        if (CurrentMapObj != null)
        {
            GameObject.DestroyImmediate(CurrentMapObj);
        }

        var initMapObj = Instantiate(initMapPrefab);
        initMapObj.transform.SetParent(this.transform);
        initMapObj.transform.localPosition = new Vector3(-55, -26, 0);
        var scripts = initMapObj.GetComponentsInChildren<CellGroupScript>();
        foreach (var s in scripts)
        {
            s.UIEventCenter = UIEventCenter;
            CellGroupScript = s;
            CurrentMapObj = initMapObj;
            s.InitData();
            LoadLevelDataFromConfig();
        }
    }

    public void OnCellClick(TetrisCellData cellData)
    {
        var currentTemplate = UIEventCenter.TemplateEditorScript.CurrentTemplate;
        if (currentTemplate == null)
        {
            Debug.Log("当前没有选中模板");
            return;
        }
        // if (currentTemplate.block_type == BlockType.Bot)
        // {
        //     for (var deltax = 0; deltax < currentTemplate.arg0; deltax++)
        //     {
        //         for (var deltay = 0; deltay < currentTemplate.arg1; deltay++)
        //         {
        //             var x = cellData.X + deltax;
        //             var y = cellData.Y + deltay;
        //             if (CellGroupScript.CellMap.TryGetValue(y, out var cs))
        //             {
        //                 if (cs.TryGetValue(x, out var desc))
        //                 {
        //                     desc.SetBlockData(currentTemplate.template_id, currentTemplate.block_type);
        //                 }
        //             }
        //         }
        //     }
        // }
        // else
        // {
            cellData.SetBlockData(currentTemplate.template_id, currentTemplate.block_type);
        // }
    }

    public void LoadLevelDataFromConfig()
    {
        LevelSelectorScript levelSelectorScript = UIEventCenter.LevelSelectorScript;
        var SingleModeConfig = levelSelectorScript.SingleModeConfig;
        var ex = SingleModeConfig.LevelConfigs.TryGetValue(levelSelectorScript.currentLevel,
            out var currentLevelConfig);
        if (!ex)
        {
            return;
        }

        foreach (var kv in currentLevelConfig.InitConfigs)
        {
            var y = kv.Key;
            var xmap = kv.Value;
            foreach (var kv2 in xmap)
            {
                var x = kv2.Key;
                var typeId = kv2.Value;
                var tetrisCellData = UIEventCenter.InitMapEditorScript.CellGroupScript.CellMap[y][x];
                var ex2 = SingleModeConfig.TemplateDescBind.TryGetValue(typeId, out var cellConfig);
                if (!ex2)
                {
                    continue;
                }

                tetrisCellData.SetBlockData(typeId, cellConfig.block_type);
            }
        }
    }


    public void SaveConfig(TetrisLevelConfig tetrisLevelConfig)
    {
        LevelSelectorScript levelSelectorScript = UIEventCenter.LevelSelectorScript;
        var SingleModeConfig = levelSelectorScript.SingleModeConfig;
        var levelConfig =
            SingleModeConfig.LevelConfigs.GetValueOrDefault(levelSelectorScript.currentLevel, new TetrisLevelConfig());
        SingleModeConfig.LevelConfigs[levelSelectorScript.currentLevel] = levelConfig;
        levelConfig.InitConfigs.Clear();
        foreach (var lineDatas in UIEventCenter.InitMapEditorScript.CellGroupScript.CellMap.Values)
        {
            foreach (var tetrisCellData in lineDatas.Values)
            {
 

                if (tetrisCellData.BlockType != BlockType.None)
                {
                    var lmap = levelConfig.InitConfigs.GetValueOrDefault(tetrisCellData.Y, new Dictionary<int, int>());
                    levelConfig.InitConfigs[tetrisCellData.Y] = lmap;
                    lmap[tetrisCellData.X] = tetrisCellData.typeId;
                }
            }
        }
    }
}