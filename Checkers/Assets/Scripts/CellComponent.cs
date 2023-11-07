using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellComponent : BaseClickComponent
{
    public bool IsFreeCellToMove { get; set; }
    [SerializeField] private Material _selectMaterial;
    [SerializeField] private Material _freeCellMaterial;

    public void SelectFreeCellTOMove()
    {
        SetMaterial(_freeCellMaterial);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current == null)
        {
            SetMaterial();
            return;
        }

        SetMaterial(_selectMaterial);
        CallBackEvent(this, true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (IsFreeCellToMove)
            SetMaterial(_freeCellMaterial);

        else
            SetMaterial();

        CallBackEvent(this, false);
    }

    public override IEnumerator Move(BaseClickComponent cell)
    {
        //реализация не нужна
        throw new System.NotImplementedException();
    }
}