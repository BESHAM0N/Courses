using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour
{
    public event Action ObjectsMoved;
    public event Action<ColorType> GameEnded;
    public event Action<BaseClickComponent> CheckerDestroyed;
    public List<CheckerComponent> Checkers { get; private set; }

    [SerializeField] private Material _selectCheckerMaterial;
    [SerializeField] private PlayerSwitcher _playerSwitcher;
    [SerializeField] private GameSide _gameSide;

    private CellComponent[,] _cells;
    private PathCreator _pathCreator;
    private List<CellComponent> _pairs;
    private bool _isReadyToMove;
    private BaseClickComponent _cachedCell;
    private Vector3 _previousPosition;

    public void Init(CellComponent[,] cells, List<CheckerComponent> checkers)
    {
        Checkers = checkers;
        _pathCreator = new PathCreator(cells, _gameSide);
        _cells = cells;
        foreach (var cell in cells)
        {
            cell.Clicked += OnCellClicked;
        }
    }
    
    private void OnDisable()
    {
        foreach (var cell in _cells)
        {
            cell.Clicked -= OnCellClicked;
        }
    }

    private void OnCellClicked(BaseClickComponent cell)
    {
        if (_isReadyToMove)
        {
            StartCoroutine(MakeMove(cell));
        }

        ClearBoard();

        if (cell.Pair == null)
            return;

        if (_gameSide.CurrentSide != cell.Pair.Color)
        {
            Debug.LogError("Сейчас ход другого игрока!");
            return;
        }
        
        SelectBaseObject(cell);
    }

    private IEnumerator MakeMove(BaseClickComponent cell)
    {
        if (!_pairs.Contains(cell))
        {
            yield break;
        }

        // блокировка хода при смене игрока
        var eventSystem = EventSystem.current;
        EventSystem.current.gameObject.SetActive(false);

        yield return StartCoroutine(_cachedCell.Pair.Move(cell));
        _previousPosition = cell.transform.position;
        var destroyChecker = _pathCreator.DestroyChecker;
        
        if (destroyChecker.Count != 0)
            DestroyChecker(destroyChecker);

        switch (_gameSide.CurrentSide)
        {
            case ColorType.Black when cell.Coordinate.Y == 7:
                GameEnded?.Invoke(ColorType.Black);
                yield break;
            case ColorType.Black when cell.Coordinate.Y == 0:
                GameEnded?.Invoke(ColorType.White);
                yield break;
        }

        cell.Pair = _cachedCell.Pair;
        _cachedCell.Pair = null;

        yield return StartCoroutine(_playerSwitcher.Switch());

        eventSystem.gameObject.SetActive(true);
        ObjectsMoved?.Invoke();
    }

    private void DestroyChecker(List<BaseClickComponent> destroyChecker)
    {
        foreach (var checker in destroyChecker.Where(checker =>
                     Vector3.Distance(checker.transform.position, _previousPosition) < 1.5f))
        {
            CheckerDestroyed?.Invoke(checker);
            checker.gameObject.SetActive(false);
            Destroy(checker.gameObject);
            return;
        }
    }

    private void SelectBaseObject(BaseClickComponent cell)
    {
        cell.Pair.SetMaterial(_selectCheckerMaterial);
        _pairs = _pathCreator.FindFreeCells(cell);
        _pairs = _pairs.Where(p => p != null).ToList();

        if (_pairs != null)
        {
            _isReadyToMove = true;
            _cachedCell = cell;
        }

        foreach (var pair in _pairs)
        {
            pair.IsFreeCellToMove = true;
            pair.SelectFreeCellTOMove();
        }
    }

    private void ClearBoard()
    {
        _isReadyToMove = false;

        if (_pairs != null)
        {
            foreach (var pair in _pairs)
            {
                pair.IsFreeCellToMove = false;
            }
        }

        foreach (var cell in _cells)
        {
            cell.SetMaterial();

            if (cell.Pair == null)
                continue;

            cell.Pair.SetMaterial();
        }
    }
}