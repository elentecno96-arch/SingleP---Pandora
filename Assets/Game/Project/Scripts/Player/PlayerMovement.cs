using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Game.Project.Utillity.Extension.Move;

namespace Game.Project.Scripts.Player
{
    /// <summary>
    /// 플레이어 이동
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody rb;
        private Vector2 inputDir;

        [SerializeField]
        private float moveSpeed;
        private void FixedUpdate()
        {
            if (rb == null) return;

            Move();
            VisualTilt();
        }
        public void Init(float speed)
        {
            rb = GetComponent<Rigidbody>();
            this.moveSpeed = speed;
        }
        public void SetInput(Vector2 direction)
        {
            //대각선 보정을 위해 normalized
            inputDir = direction.sqrMagnitude > 1f ? direction.normalized : direction;
        }
        private void Move()
        {
            if (inputDir.sqrMagnitude < 0.01f)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                return;
            }
            rb.Move3D(inputDir, moveSpeed);
        }
        private void VisualTilt()
        {
            transform.DOKill();

            if (inputDir.sqrMagnitude > 0.01f)
            {
                float tiltZ = inputDir.x > 0 ? -10f : 10f;
                if (inputDir.x == 0) tiltZ = 0;

                float tiltX = inputDir.y > 0 ? 10f : -10f;
                if (inputDir.y == 0) tiltX = 0;

                transform.DORotate(new Vector3(tiltX, 0, tiltZ), 0.15f);
            }
            else
            {
                transform.DORotate(Vector3.zero, 0.15f);
            }
        }
    }
}
