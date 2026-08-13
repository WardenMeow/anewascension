using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace anewascension.Content.Projectiles.Melee 
{
    public class SlimeSlayerProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;       
            Projectile.height = 16;      
            Projectile.friendly = true;  
            Projectile.penetrate = 1;    
            
            Projectile.aiStyle = 0;      
            
            
            Projectile.DamageType = DamageClass.Melee; 
        }

         public override void AI()
        {
            // Add a glowing light (Red, Green, Blue values from 0.0 to 1.0)
            Lighting.AddLight(Projectile.Center, 0.5f, 0.2f, 0.9f); // Purple glow

            // Create a trail of particles (Dust)
            if (Main.rand.NextBool(2)) // 50% chance every frame
            {
                // Spawns a magic pink/purple dust (DustID.PurpleTorch) at the projectile's position
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
                dust.noGravity = true; // Makes the dust float instead of falling
                dust.velocity *= 0.5f; // Makes the trail stay closer together
            }
        }
    }
}