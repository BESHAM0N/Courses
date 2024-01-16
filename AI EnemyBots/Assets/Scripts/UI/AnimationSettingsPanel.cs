using UnityEngine;
using UnityEngine.UI;

public class AnimationSettingsPanel : MonoBehaviour
{
    public static bool _isAnimation;

    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _exitButton;
    private float _sizeWindowX;
    private float _sizeWindowY;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private float _time = 10;
    private float _startTime;

    private void Start()
    {
        _sizeWindowX = Screen.width;
        _sizeWindowY = Screen.height;
        _exitButton.onClick.AddListener(DeactivationPanel);
        var positionX = _sizeWindowX - (_sizeWindowX * 0.5f);
        _startPosition = new Vector2(positionX, _sizeWindowY);
        var positionY = _sizeWindowY - (_sizeWindowY * 0.5f);
        _endPosition = new Vector2(positionX, positionY);
        _panel.transform.position = _startPosition;
    }

    private void Update()
    {
        if (_isAnimation)
        {
            GetActivePanel();
        }
    }

    private void GetActivePanel()
    {
        while (_startTime < _time)
        {
            var t = _startTime / _time;
            _panel.transform.position = Vector2.Lerp(_startPosition, _endPosition, t);
            _startTime += Time.deltaTime;
        }
        _startTime = 0;
        _isAnimation = false;
    }

    private void DeactivationPanel()
    {
        _panel.SetActive(false);
        _panel.transform.position = _startPosition;
        _isAnimation = false;
    }
}

