using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public List<BaseClickComponent> DestroyChecker { get; private set; } = new ();
    
    private readonly CellComponent[,] _cells;
    private readonly GameSide _gameSide;
    private BaseClickComponent _currentCells;

    private const int MAX_INDEX = 1;
    private const int MIN_INDEX = -1;

    public PathCreator(CellComponent[,] cells, GameSide gameSide)
    {
        _cells = cells;
        _gameSide = gameSide;
    }

    [ItemCanBeNull]
    public List<CellComponent> FindFreeCells(BaseClickComponent cell)
    {
        _currentCells = cell;
        var pairs = new List<CellComponent>();

        MoveChecker(pairs, _gameSide.CurrentSide == ColorType.Black ? MAX_INDEX : MIN_INDEX, MAX_INDEX);
        return pairs;
    }

    private void MoveChecker(List<CellComponent> pairs, int rowIndex, int columnIndex)
    {
        DestroyChecker.Clear();
        
        for (int i = 0; i < 2; i++, columnIndex = -columnIndex)
        {
            var checkRow = _currentCells.Coordinate.Y + rowIndex;
            var checkColumn = _currentCells.Coordinate.X + columnIndex;

            if (CheckFreeSteps(checkRow, checkColumn))
                pairs.Add(_cells[checkColumn, checkRow]);
            else if (CheckBoarders(checkRow, checkColumn) &&
                     _cells[checkColumn, checkRow].Pair.Color != _gameSide.CurrentSide)
            {
                DestroyChecker.Add(_cells[checkColumn, checkRow]);
                if (_gameSide.CurrentSide == ColorType.Black)
                    checkRow++;
                else
                    checkRow--;

                if (columnIndex > 0)
                    checkColumn++;
                else
                    checkColumn--;

                if (CheckFreeSteps(checkRow, checkColumn))
                    pairs.Add(_cells[checkColumn, checkRow]);
            }
        }
    }

    private bool CheckFreeSteps(int checkRow, int checkColumn)
    {
        if (!CheckBoarders(checkRow, checkColumn))
            return false;

        return _cells[checkColumn, checkRow].Pair == null;
    }

    private bool CheckBoarders(int checkRow, int checkColumn)
    {
        if (checkRow < 0 || checkRow >= _cells.GetUpperBound(0) + 1)
            return false;

        return checkColumn < _cells.GetUpperBound(1) + 1 && checkColumn >= 0;
    }

}