public class RangedAttacker : EnemyAttacker
{
    public override void Activate()
    {
        _collider.enabled = true;
    }

    public override void Deactivate()
    {
        _collider.enabled = false;
    }
}
