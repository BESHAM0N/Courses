using System.Collections.Generic;
using UnityEngine;

public class GameSide : MonoBehaviour
{
    // Первыми ходят белые
    public ColorType CurrentSide { get; private set; } = ColorType.White;

    [SerializeField] private ClickHandler _clickHandler;
    private List<BaseClickComponent> _whiteCheckers = new();
    private List<BaseClickComponent> _blackCheckers = new();

    private void Start()
    {
        foreach (var checker in _clickHandler.Checkers)
        {
            if (checker.Color == ColorType.White)
                _whiteCheckers.Add(checker);
            else
                _blackCheckers.Add(checker);
        }
    }

    private void OnEnable()
    {
        _clickHandler.ObjectsMoved += OnObjectsMoved;
        _clickHandler.GameEnded += OnGameEnded;
        _clickHandler.CheckerDestroyed += OnChipDestroyed;
    }

    private void OnDisable()
    {
        _clickHandler.ObjectsMoved -= OnObjectsMoved;
        _clickHandler.GameEnded -= OnGameEnded;
        _clickHandler.CheckerDestroyed -= OnChipDestroyed;
    }

    private void OnObjectsMoved()
    {
        CurrentSide = CurrentSide == ColorType.Black ? ColorType.White : ColorType.Black;
    }
    
    private void OnGameEnded(ColorType colorSide)
    {
        var side = colorSide == ColorType.White ? "белой" : "черной";
        Debug.Log($"Поздравляем с победой игрока {side} стороны");
    }
    
    private void OnChipDestroyed(BaseClickComponent chip)
    {
        if (_whiteCheckers.Contains(chip))
            _whiteCheckers.Remove(chip);
        else
            _blackCheckers.Remove(chip);
        

        if (_whiteCheckers.Count == 0)
            OnGameEnded(ColorType.White);
        else if (_blackCheckers.Count == 0)
            OnGameEnded(ColorType.Black);
    }
}