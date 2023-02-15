using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public enum BlockType
    {
        None = 0,
        PlayerFixed = 1,
        NoColor = 2,
        ColorBottle = 3,
        Special = 4,
        Bot = 5,
    }

    public class BlockDesc
    {
        public int template_id;
        public int basic_template_id;
        public string name;
        public BlockType block_type;
        public int ice_hp;
        public int arg0;
        public int arg1;
        public int arg2;
        public int arg3;

        public BlockDesc()
        {
        }

        public BlockDesc(BlockType blockType, int iceHp, int arg0, int arg1, int arg2, int arg3)
        {
            SetBlockData(blockType, iceHp, arg0, arg1, arg2, arg3);
        }

        public void SetBlockData(BlockType blockType, int iceHp, int arg0, int arg1, int arg2, int arg3)
        {
            block_type = blockType;
            ice_hp = iceHp;
            this.arg0 = arg0;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
        }
    }

    public class LevelCondition
    {
        public int UseStep;
        public int KillBotNum;
        public int AttackBotTimesOnce;
        public int TotalCleanRowNum;
        public int CleanRowNumOnce;
        public Dictionary<int, int> TotalCollectNums = new();
        public Dictionary<int, int> CollectNumOnce = new();
    }

    public class GenerateBlockConfig
    {
        public int GameType;
        public int GameIndex;
        public int TemplateId0 = -1;
        public int TemplateId1 = -1;
        public int TemplateId2 = -1;
        public int TemplateId3 = -1;

        public int GetValueByOrder(int order)
        {
            switch (order)
            {
                case 0:
                    return TemplateId0;
                case 1:
                    return TemplateId1;
                case 2:
                    return TemplateId2;
                case 3:
                    return TemplateId3;
            }

            return 0;
        }

        public void SetValueByOrder(int order, int value)
        {
            switch (order)
            {
                case 0:
                    TemplateId0 = value;
                    return;
                case 1:
                    TemplateId1 = value;
                    return;
                case 2:
                    TemplateId2 = value;
                    return;
                case 3:
                    TemplateId3 = value;
                    return;
            }
        }
    }

    public class TetrisLevelConfig
    {
        public LevelCondition PassCondition = new LevelCondition();
        public LevelCondition StarCondition = new LevelCondition();
        public Dictionary<int, GenerateBlockConfig> GenerateConfigs = new Dictionary<int, GenerateBlockConfig>();
        public Dictionary<int, Dictionary<int, int>> InitConfigs = new Dictionary<int, Dictionary<int, int>>();
        public int FullStarGoldCnt = 0;
    }

    public class SkillConfig
    {
        public int id;
        public string name;
    }

    public class SingleModeConfig
    {
        public Dictionary<int, BlockDesc> TemplateDescBind = new Dictionary<int, BlockDesc>();
        public Dictionary<int, TetrisLevelConfig> LevelConfigs = new Dictionary<int, TetrisLevelConfig>();
        public Dictionary<int, SkillConfig> SkillConfigs = new Dictionary<int, SkillConfig>();
    }
}