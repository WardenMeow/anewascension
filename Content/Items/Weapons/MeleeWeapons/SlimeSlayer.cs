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
            Item.width = 64;
            Item.height = 64;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Swing; // Comment this out if it doesn't work :)
            Item.DamageType = DamageClass.Melee;
            Item.damage = 45;
            Item.knockBack = 6;
            Item.crit = 6;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<Content.Projectiles.Melee.SlimeSlayerProjectile>();
            Item.shootSpeed = 8f;
        }
    
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            float ceilingLimit = target.Y;

            if (ceilingLimit > player.Center.Y - 200f)
            {
                ceilingLimit = player.Center.Y - 200f;
            }

            for (int i = 0; i < 3; i++)
            {
                position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
                position.Y -= 100 * i;

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
                heading.Y += Main.rand.Next(-40, 41) * 0.02f;

                Projectile.NewProjectile(source, position, heading, type, damage * 2, knockback, player.whoAmI, 0f, ceilingLimit);
            }

            return false;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation = player.MountedCenter;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
             // 1. Calculate how far along the swing animation is (from 1.0 down to 0.0)
            float progress = (float)player.itemAnimation / player.itemAnimationMax;

            // 2. Multiply by TwoPi (360 degrees) to get a full spin cycle
            // player.direction makes it spin forward whether facing left or right
            float spinAngle = MathHelper.TwoPi * progress * player.direction;

            // 3. Apply the spin directly to the weapon graphic in the hand
            player.itemRotation = spinAngle;

            // Optional: Add a trail of your green slime particles spinning off the hand!
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(player.MountedCenter, 10, 10, 16);
                dust.velocity = player.itemRotation.ToRotationVector2() * 3f;
                dust.noGravity = true;
            }
        }   
    }
}
