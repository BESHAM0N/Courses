using System.Linq;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    [SerializeField] private Material _selectMaterial;
    [SerializeField] private Material _unselectMaterial;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Camera _camera;
    private Renderer _renderer;
    private Unit _unit;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        OpenSettingsPanel();
    }

    private void OpenSettingsPanel()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.GetComponent<SelectObject>() != null)
                {
                    _panel.SetActive(true);
                    AnimationSettingsPanel._isAnimation = true;

                    if (hit.collider.gameObject.TryGetComponent(out IBaseZigguratMarker baseZigguratMarker))
                    {
                        _unit = UnitsStorage.Units.FirstOrDefault(x => x.Id == baseZigguratMarker.Id);
                        EventManager.CallGetUnit(_unit);
                    }
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        _renderer.material = _selectMaterial;
    }

    private void OnMouseExit()
    {
        _renderer.material = _unselectMaterial;
    }
}