using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 풀링을 위한 별도의 위험 표시 클래스
/// </summary>
public class WarningIndicator : MonoBehaviour
{
    public void InitAndRelease(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ReleaseRoutine(duration));
    }

    private IEnumerator ReleaseRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        PoolManager.Instance.ReturnEffect(this.gameObject);
    }
}
