using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class SingleTetrisShapes
    {
        // ShapeO 田字型
        static Vector2[] ShapeO = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(1, 1),
        };

// ShapeJ0 J 字型 四个方向

        private static Vector2[] ShapeJ0 = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(1, 2),
            new Vector2(0, 2),
        };

        private static Vector2[] ShapeJ1 = new Vector2[]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(2, 1),
            new Vector2(2, 2),
        };

        static Vector2[] ShapeJ2 = new Vector2[]
        {
            new Vector2(1, 2),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(2, 0),
        };

        static Vector2[] ShapeJ3 = new Vector2[]
        {
            new Vector2(2, 1),
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(0, 0),
        };

        static Vector2[][] ShapeJList = { ShapeJ0, ShapeJ1, ShapeJ2, ShapeJ3 };

        // ShapeL0 L字型 四个方向
        static Vector2[] ShapeL0 = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(1, 2),
            new Vector2(2, 2),
        };

        static Vector2[] ShapeL1 = new Vector2[]
        {
            new Vector2(0, 2),
            new Vector2(1, 2),
            new Vector2(2, 2),
            new Vector2(2, 1),
        };

        static Vector2[] ShapeL2 = new Vector2[]
        {
            new Vector2(1, 2),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0),
        };

        static Vector2[] ShapeL3 = new Vector2[]
        {
            new Vector2(2, 1),
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(0, 2),
        };

        static Vector2[][] ShapeLList =
        {
            ShapeL0, ShapeL1,
            ShapeL2, ShapeL3
        };

        // ShapeT0 T 字型 四个方向
        static Vector2[] ShapeT0 = new Vector2[]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(2, 1),
            new Vector2(1, 2),
        };

        static Vector2[] ShapeT1 = new Vector2[]
        {
            new Vector2(1, 2),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(2, 1),
        };

        static Vector2[] ShapeT2 = new Vector2[]
        {
            new Vector2(2, 1),
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(1, 0),
        };

        static Vector2[] ShapeT3 = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(1, 2),
            new Vector2(0, 1),
        };

        static Vector2[][] ShapeTList = { ShapeT0, ShapeT1, ShapeT2, ShapeT3 };

        // ShapeS0 S 字型 两个方向
        static Vector2[] ShapeS0 = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 2),
        };

        static Vector2[] ShapeS1 = new Vector2[]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(2, 0),
        };

        private static Vector2[] ShapeS2 = new Vector2[]
        {
            new Vector2( 1,  2),
            new Vector2( 1,  1),
            new Vector2( 0,  1),
            new Vector2( 0,  0),
        };

        private static Vector2[] ShapeS3 = new Vector2[]
        {
            new Vector2( 2,  0),
            new Vector2( 1,  0),
            new Vector2( 1,  1),
            new Vector2( 0,  1),
        };
        static Vector2[][] ShapeSList = { ShapeS0, ShapeS1 ,ShapeS2,ShapeS3};

        // ShapeZ0 Z 字型 两个方向
        static Vector2[] ShapeZ0 = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(2, 1),
        };

        static Vector2[] ShapeZ1 = new Vector2[]
        {
            new Vector2(0, 2),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
        };
        static Vector2[] ShapeZ2 = new Vector2[]
        {
            new Vector2(2, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0),
        };
        static Vector2[] ShapeZ3 = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(0, 2),
        };

        static Vector2[][] ShapeZList = { ShapeZ0, ShapeZ1,ShapeZ2,ShapeZ3 };

        // ShapeI0 I 字型 两个方向
        static Vector2[] ShapeI0 = new Vector2[]
        {
            new Vector2(2, 0),
            new Vector2(2, 1),
            new Vector2(2, 2),
            new Vector2(2, 3),
        };

        static Vector2[] ShapeI1 = new Vector2[]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(2, 1),
            new Vector2(3, 1),
        };

        static Vector2[][] ShapeIList = { ShapeI0, ShapeI1 };

        public static Dictionary<GameTypeEnum, Vector2[][]> HardBlockTypeToShapes = new();

        static SingleTetrisShapes()
        {
            HardBlockTypeToShapes[GameTypeEnum.Tian] = new[] { ShapeO };
            HardBlockTypeToShapes[GameTypeEnum.J] = ShapeJList;
            HardBlockTypeToShapes[GameTypeEnum.L] = ShapeLList;
            HardBlockTypeToShapes[GameTypeEnum.T] = ShapeTList;
            HardBlockTypeToShapes[GameTypeEnum.S] = ShapeSList;
            HardBlockTypeToShapes[GameTypeEnum.Z] = ShapeZList;
            HardBlockTypeToShapes[GameTypeEnum.I] = ShapeIList;
        }
    }

    public enum GameTypeEnum
    {
        None = 0, // 空类型
        Tian = 1, // 田字形
        J = 2, // J 字形
        L = 3, // L 字形
        T = 4, // T 字形
        S = 5, // S 字形
        Z = 6, // Z 字形
        I = 7, // I 字形
    }
}