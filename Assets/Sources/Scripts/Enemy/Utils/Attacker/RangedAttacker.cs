using UnityEngine;
using UnityEngine.Animations.Rigging;
using Zenject;

public class RangedAttacker : EnemyAttacker
{
    [Header("Компоненты Rigging")]
    [SerializeField] private MultiAimConstraint _bodyAimConstraint;
    [SerializeField] private MultiAimConstraint _headAimConstraint;
    [SerializeField] private Transform _aimTarget;
    [SerializeField] private float _aimSpeed = 2f;

    [Inject]
    public void Construct(Player player)
    {
        ResetWeight();
    }

    public override void Activate()
    {
        _collider.enabled = true;
        ResetWeight();
    }

    public override void Deactivate()
    {
        _collider.enabled = false;
        ResetWeight();
    }

    public void RotateToAim(Vector3 target)
    {
        _aimTarget.position = target;

        _bodyAimConstraint.weight = Mathf.MoveTowards(_bodyAimConstraint.weight, 1f, _aimSpeed * Time.deltaTime);
        _headAimConstraint.weight = Mathf.MoveTowards(_bodyAimConstraint.weight, 1f, _aimSpeed * Time.deltaTime);       
    }

    public void RotateToIdle()
    {
        _bodyAimConstraint.weight = Mathf.MoveTowards(_bodyAimConstraint.weight, 0f, _aimSpeed * Time.deltaTime);
        _headAimConstraint.weight = Mathf.MoveTowards(_bodyAimConstraint.weight, 0f, _aimSpeed * Time.deltaTime);
    }

    public void ClearAim()
    {
        ResetWeight();
    }

    private void ResetWeight()
    {
        _bodyAimConstraint.weight = 0;
        _headAimConstraint.weight = 0;
    }
}
