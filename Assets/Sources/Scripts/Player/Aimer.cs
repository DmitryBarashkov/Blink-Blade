using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Aimer : MonoBehaviour
{
    private float _aimDistance = 10f;

    private Camera _camera;
    private RigBuilder _rigBuilder;
    private Transform _transform;
    private Transform _targetTransform;
    private Animator _animator;
    
    public void Initialize(Camera camera, RigBuilder rigBuilder, Transform target, Animator animator)
    {
        _camera = camera;
        _rigBuilder = rigBuilder;
        _targetTransform = target;
        _animator = animator;
        _transform = transform;
    }    
    
    public void StartAim()
    {
        Plane plane = new Plane(Vector3.forward, _transform.position + _transform.forward * _aimDistance);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        _rigBuilder.enabled = true;

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 mouseWorldPoint = ray.GetPoint(enter);

            _targetTransform.position = Vector3.Lerp(_targetTransform.position, mouseWorldPoint, Time.deltaTime * 10f);
        }

        _animator.SetBool("IsAiming", true);
    }

    public void Throw()
    {
        _animator.SetBool("IsAiming", false);

        _rigBuilder.enabled = false;
    }
}
