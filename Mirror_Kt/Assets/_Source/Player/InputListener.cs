using UnityEngine;
using Mirror;
using Zenject;

namespace Player
{
    public class InputListener : NetworkBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody2D rb;

        private PlayerController _controller;

        //[Inject]
        //public void Construct(PlayerController playerController)
        //{
        //    _controller = playerController;
        //}

        private void Start()
        {
            _controller = new PlayerController();
        }

        private void FixedUpdate()
        {
            if (isLocalPlayer)
            {
                Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                _controller.Move(rb, moveDirection, speed);
            }
        }
    }
}