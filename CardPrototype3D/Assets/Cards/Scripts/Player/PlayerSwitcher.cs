using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class PlayerSwitcher : MonoBehaviour
    { //TODO: remove/refactor
        [SerializeField] private float _timeSpeedSwitch;
        [SerializeField] private Transform _playerOne;
        [SerializeField] private Transform _playerTwo;
        [SerializeField] private Button _endTurn;
        
        private Camera _camera;
        private Transform _nextPlayer;
        
        public bool firstPlayerMove = true;
        private Transform CameraTransform => _camera.transform;

        private void Awake()
        {
            _camera = Camera.main;
            CameraTransform.position = _playerOne.position;
            CameraTransform.rotation = _playerOne.rotation;
            _nextPlayer = _playerTwo; 
            //_endTurn.onClick.AddListener(SwitchPlayer);
        }

        private void SwitchPlayer()
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
            }
            
            _nextPlayer = _nextPlayer == _playerOne 
                ? _playerTwo 
                : _playerOne;

            firstPlayerMove = _nextPlayer == _playerTwo;
            //_manaController.IncreaseMana(firstPlayerMove);
        }        
    }
}
