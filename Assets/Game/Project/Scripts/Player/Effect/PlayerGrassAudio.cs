using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrassAudio : MonoBehaviour
{
    [SerializeField] private LayerMask grassLayer;      
    [SerializeField] private float rayDistance = 0.5f;   
    [SerializeField] private float sfxInterval = 0.4f;  

    [SerializeField] private AudioClip grassStepSfx;    

    private CharacterController _controller;
    private float _lastSfxTime;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_controller != null && _controller.velocity.magnitude > 0.1f)
        {
            CheckGrass();
        }
    }

    private void CheckGrass()
    {
        if (Time.time - _lastSfxTime < sfxInterval) return;

        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, grassLayer))
        {
            PlayGrassSfx();
        }
    }

    private void PlayGrassSfx()
    {
        if (AudioManager.HasInstance && grassStepSfx != null)
        {
            AudioManager.Instance.PlaySfx(grassStepSfx);
            _lastSfxTime = Time.time;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + Vector3.down * rayDistance);
    }
}
