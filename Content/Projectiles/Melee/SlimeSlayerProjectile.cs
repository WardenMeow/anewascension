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
            Lighting.AddLight(Projectile.Center, 0.05f, 0.4f, 0.05f);

            // 3. Spawn dripping slime dust trails
            if (Main.rand.NextBool(2)) // 50% chance every frame
            {
                
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 193);
                
                dust.noGravity = false; 
                dust.scale = Main.rand.NextFloat(0.8f, 1.2f); 
                dust.velocity.X *= 0.2f; 
            }
        }

        public public override void OnKill(int timeLeft)
{
    // 1. Play a loud explosion sound instead of a soft squish
    if (Projectile.identity % 3 == 0) 
    {
        Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item14, Projectile.position); 
    }

    // 2. Spawn a massive burst of fiery dust particles
    for (int i = 0; i < 40; i++) // Increased from 15 to 40 for a huge blast
    {
        // DustID.Torch (6) creates bright fiery sparks
        Dust fireDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, Terraria.ID.DustID.Torch);
        fireDust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 9f); // Shoot out fast in all directions
        fireDust.scale = Main.rand.NextFloat(1.5f, 2.5f); // Make the particles physically larger
        fireDust.noGravity = true;

        // DustID.Smoke (31) creates lingering dark explosion clouds
        if (Main.rand.NextBool(2))
        {
            Dust smokeDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, Terraria.ID.DustID.Smoke);
            smokeDust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 5f);
            smokeDust.scale = Main.rand.NextFloat(1.0f, 2.0f);
        }
    }

    // 3. Optional: Deal damage to all enemies in a wide explosion radius
    // 160 pixels equals a massive 10-block blast radius
    int explosionRadius = 160; 
    for (int i = 0; i < Main.maxNPCs; i++)
    {
        NPC target = Main.npc[i];
        if (target.CanBeChasedBy() && Vector2.Distance(Projectile.Center, target.Center) <= explosionRadius)
        {
            // Deal damage to any enemy caught in the shockwave
            int direction = target.Center.X > Projectile.Center.X ? 1 : -1;
            target.StrikeNPC(target.CalculateDamageInfo(Projectile.damage * 2, direction, false, Projectile.knockBack));
        }
    }
}
    }
}