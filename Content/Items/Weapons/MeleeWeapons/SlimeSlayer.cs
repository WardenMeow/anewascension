using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace anewascension.Content.Items.Weapons.MeleeWeapons
{
    

    public class SlimeSlayer : ModItem
    {
        public override void SetDefaults()
        {
            // Visual/Animation Properties
            Item.width = 64;
            Item.height = 64;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            // Weapon Stats
            Item.DamageType = DamageClass.Melee;
            Item.damage = 45;
            Item.knockBack = 6;
            Item.crit = 6;

            // Rarity & Value
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;

            // Projectile Shooting Properties
            Item.shoot = ModContent.ProjectileType<SlimeSlayerProjectile>(); 
            Item.shootSpeed = 8f;
        }
    
           public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.MouseWorld; 
            float ceilingLimit = target.Y;

            if (ceilingLimit > player.Center.Y - 200f)
            {
                ceilingLimit = player.Center.Y - 200f;
            }

            for (int i = 0; i < 3; i++)
            {
                // 4. Made math numbers consistent and clean
                position = player.Center - new Vector2(Main.rand.NextFloat(400f) * player.direction, 600f);
                position.Y -= 100f * i;

                Vector2 heading = target - position;

                if (heading.Y < 0f)
                {
                    heading.Y *= -1f;
                }

                if (heading.Y < 20f)
                {
                    heading.Y = 20f;
                }

                heading.Normalize();
                heading *= velocity.Length();
                heading.Y += Main.rand.NextFloat(-0.8f, 0.8f); 

                Projectile.NewProjectile(source, position, heading, type, damage * 2, knockback, player.whoAmI, 0f, ceilingLimit);
            }

            return false;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation = player.MountedCenter;
        }
    }
}
