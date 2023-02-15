using System;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CellGroupScript : MonoBehaviour
{
    public UIEventCenter UIEventCenter;
    private readonly int Height = 38;
    private readonly int Width = 12;

    public Dictionary<int, Dictionary<int, TetrisCellData>> CellMap = new();

    private GameObject cellPrefab;

    public void Start()
    {
    }

    // Start is called before the first frame update
    public void InitData()
    {
        cellPrefab = Resources.Load<GameObject>("Cell");

        for (var row = 0; row < Height; row++)
        {
            var rowTetrisCells = new Dictionary<int, TetrisCellData>();
            CellMap[row] = rowTetrisCells;
            for (var col = 0; col < Width; col++)
            {
                var tetrisCell = CreateTetrisCell(col, row);
                rowTetrisCells[col] = tetrisCell;
            }
        }
    }


    // Update is called once per frame
    private void Update()
    {
    }

    private TetrisCellData CreateTetrisCell(int x, int y)
    {
        var tetrisCell = Instantiate(cellPrefab);
        var cellData = tetrisCell.GetComponent<TetrisCellData>();
        var image = tetrisCell.GetComponent<Image>();
        var button = tetrisCell.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => UIEventCenter.InitMapEditorScript.OnCellClick(cellData));
            cellData.Image = image;
            cellData.X = x;
            cellData.Y = y;
            cellData.SetBlockData(0, BlockType.None);
        }

        tetrisCell.transform.SetParent(transform);
        return cellData;
    }
}