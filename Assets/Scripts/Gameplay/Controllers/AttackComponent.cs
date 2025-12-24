public class AttackComponent
{
    public float FireRate { get; }
    public int Damage { get; }

    private float _lastAttackTime;

    public AttackComponent(float fireRate, int damage)
    {
        FireRate = fireRate;
        Damage = damage;
    }

    public bool CanAttack(float time)
        => time - _lastAttackTime >= FireRate;

    public void RegisterAttack(float time)
        => _lastAttackTime = time;
}