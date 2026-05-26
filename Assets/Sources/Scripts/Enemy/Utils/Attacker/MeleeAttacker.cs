public class MeleeAttacker : EnemyAttacker
{
    public override void Activate()
    {
        _collider.enabled = true;
        _weapon.Activate();
    }

    public override void Deactivate()
    {
        _collider.enabled = false;
        _weapon.Deactivate();
    }
}
