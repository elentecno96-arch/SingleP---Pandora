using UnityEngine;

namespace Game.Project.Utility.Extension.Move
{
    public static class RigidbodyExtensions
    {
        /// <summary>
        /// 3D 공간에서 XZ 평면 이동을 처리합니다.
        /// </summary>
        public static void Move3D(this Rigidbody rb, Vector2 inputDir, float speed)
        {
            // 입력의 Y를 3D의 Z(전진)로 매핑합니다.
            Vector3 targetVelocity = new Vector3(inputDir.x, 0, inputDir.y) * speed;

            // 현재의 Y축 속도(중력)는 유지하면서 X, Z만 변경합니다.
            targetVelocity.y = rb.velocity.y;

            rb.velocity = targetVelocity;
        }
    }
}
