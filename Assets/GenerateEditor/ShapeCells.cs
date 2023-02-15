using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShapeCells : MonoBehaviour
{
    public bool back;
    public UIEventCenter UIEventCenter;
    private readonly int Height = 4;
    private readonly int Width = 4;

    public Dictionary<int, Dictionary<int, TetrisCellData>> CellMap = new();

    private GameObject cellPrefab;

    public void Start()
    {
        InitData();
    }

    // Start is called before the first frame update
    public void InitData()
    {
        if (back)
            cellPrefab = Resources.Load<GameObject>("BackCell");
        else
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
            button.onClick.AddListener(() => UIEventCenter.GenerateBlockEditorScript.OnCellClick(cellData));
            cellData.Image = image;
            cellData.X = x;
            cellData.Y = y;
            cellData.SetBlockData(0, BlockType.None);
        }

        tetrisCell.transform.SetParent(transform);
        return cellData;
    }
}