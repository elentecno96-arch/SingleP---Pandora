using UnityEngine;
using DG.Tweening;
using Game.Project.Utility.Extension.Move;

namespace Game.Project.Scripts.Player
{
    /// <summary>
    /// 플레이어 이동
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody rb;
        private Vector2 inputDir;
        private Vector2 _prevInputDir;

        //[SerializeField] private Transform visualChild;

        [SerializeField]
        private float moveSpeed;
        private void Update()
        {
            //VisualTilt(); //DOTween의 애니메이션은 프레임 기반이라 업데이트에 있는게 자연스러움
        }
        private void FixedUpdate()
        {
            if (rb == null) return;

            Move();
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

            Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);
            if (moveDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
            }
        }
        //private void VisualTilt()
        //{
        //    if (visualChild == null) return;
        //    if (inputDir == _prevInputDir) return;
        //    _prevInputDir = inputDir;

        //    visualChild.DOKill();

        //    if (inputDir.sqrMagnitude > 0.01f)
        //    {
        //        float tiltZ = inputDir.x > 0 ? -12f : 12f;
        //        if (inputDir.x == 0) tiltZ = 0;

        //        float tiltX = inputDir.y > 0 ? 12f : -12f;
        //        if (inputDir.y == 0) tiltX = 0;

        //        visualChild.DOLocalRotate(new Vector3(tiltX, 0, tiltZ), 0.15f).SetEase(Ease.OutQuad);
        //    }
        //    else
        //    {
        //        visualChild.DOLocalRotate(Vector3.zero, 0.15f).SetEase(Ease.OutQuad);
        //    }
        //}
    }
}
