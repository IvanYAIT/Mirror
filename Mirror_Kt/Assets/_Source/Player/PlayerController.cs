using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController
    {
        public void Move(Rigidbody2D rb, Vector2 direction, float speed)
        {
            rb.velocity = direction * speed;
        }
    }
}