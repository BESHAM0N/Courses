using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseClickComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,
    IPointerExitHandler
{
    [Tooltip("Цветовая сторона игрового объекта")]
    public ColorType Color { get; set; }
    public BaseClickComponent Pair { get; set; }
    [field: SerializeField] public Material WhiteMaterial { get; set; }
    [field: SerializeField] public Material BlackMaterial { get; set; }
    
    public Coordinate Coordinate;
    private Material _defaultMaterial;

    //Меш игрового объекта
    private MeshRenderer _meshRenderer;

    protected virtual void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetDefaultMaterial(Material material)
    {
        _defaultMaterial = material;
        SetMaterial(material);
    }

    public void SetMaterial(Material material = null)
    {
        _meshRenderer.sharedMaterial = material ? material : _defaultMaterial;
    }

    /// <summary>
    /// Событие клика на игровом объекте
    /// </summary>
    public event ClickEventHandler Clicked;

    /// <summary>
    /// Событие наведения и сброса наведения на объект
    /// </summary>
    public event FocusEventHandler Focused;

    //При навадении на объект мышки, вызывается данный метод
    //При наведении на фишку, должна подсвечиваться клетка под ней
    //При наведении на клетку - подсвечиваться сама клетка
    public abstract void OnPointerEnter(PointerEventData eventData);

    //Аналогично методу OnPointerEnter(), но срабатывает когда мышка перестает
    //указывать на объект, соответственно нужно снимать подсветку с клетки
    public abstract void OnPointerExit(PointerEventData eventData);
    public abstract IEnumerator Move(BaseClickComponent cell);

    //При нажатии мышкой по объекту, вызывается данный метод
    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(this);
    }

    //Этот метод можно вызвать в дочерних классах (если они есть) и тем самым пробросить вызов
    //события из дочернего класса в родительский
    protected void CallBackEvent(CellComponent target, bool isSelect)
    {
        Focused?.Invoke(target, isSelect);
    }
}

public delegate void ClickEventHandler(BaseClickComponent component);

public delegate void FocusEventHandler(CellComponent component, bool isSelect);