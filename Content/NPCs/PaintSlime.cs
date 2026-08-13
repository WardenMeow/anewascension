using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;

namespace anewascension.Content.NPCs
{
    public class PaintSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 24;
            NPC.damage = 10;
            NPC.defense = 2;
            NPC.lifeMax = 30;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 1; 
            AIType = NPCID.BlueSlime; 

            NPC.alpha = 110; // This makes the slime somewhat transparent. 
        }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.OneFromOptions(1, new int[] { ItemID.RedPaint, ItemID.BluePaint, ItemID.ShadowPaint }));
    }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // Spawn in the surface layer during the day
            if (spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime)
            {
                return 0.05f; // 5% chance to spawn
            }
            return 0f; // Do not spawn otherwise
        }
    }
}