using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Blocker : MonoBehaviour
{
    [Header("Компоненты Rigging")]
    [SerializeField] private MultiAimConstraint _bodyAimConstraint;
    [SerializeField] private Transform _aimTarget;

    [Header("Настройки")]
    [SerializeField] private float _responseSpeed = 8f;

    private Transform _playerWeaponTransform;
    private bool _isWeaponInZone = false;

    public event Action OnWeaponInBlockingArea;

    public event Action OnWeaponOutBlockingArea;

    private void Awake()
    {
        _bodyAimConstraint.weight = 0f;
    }

    private void Update()
    {
        float targetWeight = _isWeaponInZone ? 1f : 0f;

        _bodyAimConstraint.weight = Mathf.Lerp(_bodyAimConstraint.weight, targetWeight, Time.deltaTime * _responseSpeed);

        if (_isWeaponInZone && _playerWeaponTransform != null)
        {
            _aimTarget.position = _playerWeaponTransform.position;
        }
    }

    public void Reset()
    {
        _aimTarget.position = Vector3.zero;
        _isWeaponInZone = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Weapon weapon = other.GetComponent<Weapon>();

        if (weapon != null)
        {
            _playerWeaponTransform = other.transform;
            _isWeaponInZone = true;
            OnWeaponInBlockingArea?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Weapon>() != null)
        {
            OnWeaponOutBlockingArea?.Invoke();
            _isWeaponInZone = false;
        }
    }
}
