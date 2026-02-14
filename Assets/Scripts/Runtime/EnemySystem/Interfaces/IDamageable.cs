/* -------------------------------------------------------------------------------------------
 * Interface that contains all methods about damageable character (e.g. enemy)
 * All of them can be damaged and can die
 * ------------------------------------------------------------------------------------------- */
interface IDamageable
{
    void OnTakenDamage(float baseDamage);
    void OnDead();
}


