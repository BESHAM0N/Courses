using System.Collections;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    [SerializeField] private float _timeSpeedSwitch;
    [SerializeField] private Transform _playerOne;
    [SerializeField] private Transform _playerTwo;

    private Camera _camera;
    private Transform _nextPlayer;
    private Transform CameraTransform => _camera.transform;

    private void Awake()
    {
        _camera = Camera.main;
        CameraTransform.position = _playerOne.position;
        CameraTransform.rotation = _playerOne.rotation;
        _nextPlayer = _playerTwo;
    }

    public IEnumerator Switch()
    {
        while (Vector3.Distance(CameraTransform.position, _nextPlayer.position) >= 0.01f)
        {
            CameraTransform.position = Vector3.Lerp(
                CameraTransform.position,
                _nextPlayer.position,
                _timeSpeedSwitch * Time.deltaTime);
            CameraTransform.rotation = Quaternion.Lerp(CameraTransform.rotation,
                _nextPlayer.rotation,
                _timeSpeedSwitch * Time.deltaTime);

            yield return null;
        }
        _nextPlayer = _nextPlayer == _playerOne ? _playerTwo : _playerOne;
    }
}