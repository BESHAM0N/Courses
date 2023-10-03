using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private void Awake()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        Jump();
        Left();
        Right();
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            _playerMovement.Jump();
    }

    private void Left()
    {
        if(Input.GetKey(KeyCode.A))
            _playerMovement.MoveLeft();
    }

    private void Right()
    {
        if(Input.GetKey(KeyCode.D))
            _playerMovement.MoveRight();
    }
    
    
}
