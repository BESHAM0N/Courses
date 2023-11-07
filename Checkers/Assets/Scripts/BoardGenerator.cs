using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClickHandler))]
public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private int _rows;
    [SerializeField] private int _cols;
    [SerializeField] private CellComponent _cellPrefab;
    [SerializeField] private CheckerComponent _checkerPrefab;
    private readonly List<CheckerComponent> _checkers = new ();

    private ClickHandler _clickHandler;

    public void Awake()
    {
        _clickHandler = GetComponent<ClickHandler>();
        Generation();
    }

    private void Generation()
    {
        var cells = new CellComponent[_rows, _cols];
        var lastColor = ColorType.White;

        for (int i = 0; i < _rows; i++)
        {
            float lastPosition = 0f;
           
            lastColor = lastColor == ColorType.White ? ColorType.Black : ColorType.White;

            for (int j = 0; j < _cols; j++)
            {
                var cell = Instantiate(
                    _cellPrefab,
                    new Vector3(i, 0, lastPosition),
                    Quaternion.identity,
                    transform);

                lastPosition += cell.transform.localScale.x;
                lastColor = lastColor == ColorType.White ? ColorType.Black : ColorType.White;
                cell.SetDefaultMaterial(lastColor == ColorType.Black ? cell.BlackMaterial : cell.WhiteMaterial);

                cells[i, j] = cell;
                cell.Coordinate = new Coordinate(i, j);

                if (lastColor != ColorType.Black)
                    continue;

                switch (j)
                {
                    case < 3:
                    {
                        GenerationChecker(cell, ColorType.Black);
                        break;
                    }
                    case > 4:
                    {
                        GenerationChecker(cell, ColorType.White);
                        break;
                    }
                }
            }
        }
        _clickHandler.Init(cells, _checkers);
    }

    private void GenerationChecker(CellComponent cell, ColorType color)
    {
        var checker = Instantiate(
            _checkerPrefab,
            cell.transform.position,
            Quaternion.identity,
            transform);
        checker.SetDefaultMaterial(color == ColorType.Black ? checker.BlackMaterial : checker.WhiteMaterial);
        checker.Pair = cell;
        cell.Pair = checker;
        checker.Color = color;
        checker.Coordinate = cell.Coordinate;
        _checkers.Add(checker);
    }
}