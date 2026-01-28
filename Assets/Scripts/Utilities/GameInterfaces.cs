// THIS PAGE CONTAINS ALL INTERFACES THAT USED DURING THE GAMEPLAY

/* -------------------------------------------------------------------------------------------
 * Interface that contains all methods about damageable character (e.g. enemy)
 * All of them can be damaged and can die
 * ------------------------------------------------------------------------------------------- */
interface IDamageable
{
    void OnTakenDamage(float baseDamage);
    void OnDead();
}

/* --------------------------------------------------------------------------------------------
 * Interface that contains all methods about player interaction
 * -------------------------------------------------------------------------------------------- */
interface IInteractable
{
    void OnHovered();
    void OnHoverExit();
    void OnSelected();
}
