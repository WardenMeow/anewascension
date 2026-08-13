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
    }
}