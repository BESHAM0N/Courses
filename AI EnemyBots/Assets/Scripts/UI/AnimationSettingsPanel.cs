using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationSettingsPanel : MonoBehaviour
{
    public static bool _isAnimation;

    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _exitButton;
    [SerializeField] private List<TMP_Text> _fields = new();
    [SerializeField] private CanvasGroup _canvasGroup;
    private float _sizeWindowX;
    private float _sizeWindowY;
    private Vector2 _endPosition;

    private void Start()
    {
        _sizeWindowX = Screen.width;
        _sizeWindowY = Screen.height;
        _exitButton.onClick.AddListener(DeactivationPanel);
        var positionX = _sizeWindowX - _sizeWindowX * 0.5f;
        var positionY = _sizeWindowY - _sizeWindowY * 0.5f;
        _endPosition = new Vector2(positionX, positionY);
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
        _panel.GetComponent<Image>().material.DOFade(0, 0f);
        _panel.transform.position = _endPosition;
        
        _canvasGroup.blocksRaycasts = true;
        _panel.GetComponent<Image>().material.DOFade(1, 1f);

        foreach (var field in _fields)
        {
            field.DOFade(1, 0.5f);
        }
        _isAnimation = false;
    }

    private void DeactivationPanel()
    {
        _canvasGroup.blocksRaycasts = false;
        foreach (var field in _fields)
        {
            field.DOFade(0, 0.5f);
        }
        _panel.GetComponent<Image>().material.DOFade(0, 1f);
        _isAnimation = false;
    }
}