using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

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
            // 1. Add a soft blue/aquamarine glow like a gel slime
            Lighting.AddLight(Projectile.Center, 0.1f, 0.4f, 0.8f);

            // 3. Spawn dripping slime dust trails
            if (Main.rand.NextBool(2)) // 50% chance every frame
            {
                
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.t_Slime);
                
                dust.noGravity = false; 
                dust.scale = Main.rand.NextFloat(0.8f, 1.2f); 
                dust.velocity.X *= 0.2f; 
            }
        }

        public override void OnKill(int timeLeft)
        {

            if (Projectile.identity % 3==0)
            {
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position); // Standard slime pop sound
            }
            // 5. Create a burst of 15 slime splatters hitting the ground
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.t_Slime);
                
                // Explode outwards in a splash shape
                dust.velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-4f, 0f)); 
                dust.scale = Main.rand.NextFloat(0.6f, 1.4f);
            }
        }
    }
}