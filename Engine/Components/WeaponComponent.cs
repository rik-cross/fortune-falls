namespace Engine
{
    public class WeaponComponent : Component
    {
        // Todo: Destroy entity if character entity is destroyed

        public Weapon Weapon { get; private set; }
        //public Sprite WeaponSprite { get; private set; } // Or SpriteComponent?
        public Entity Character { get; private set; }
        public bool IsAttackActive;

        public WeaponComponent(Weapon weapon = null, Sprite weaponSprite = null,
            Entity character = null)
        {
            Weapon = weapon;
            //WeaponSprite = weaponSprite;
            Character = character;
            IsAttackActive = false;
        }

        public void SetWeapon(Weapon weapon)
        {
            // Todo: Reset sprite values? Inform character?
            Weapon = weapon;
        }

        public void SetWeaponSprite(Sprite weaponSprite)
        {
            // Todo: Reset sprite values?
            //WeaponSprite = weaponSprite;
        }

        public void SetBaseCharacter(Entity character)
        {
            // Todo: Reset sprite values?
            Character = character;
        }

    }

}
