using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using anewascension.Content.Systems;

namespace anewascension.Content.Biomes
{
    public class GelatinousCaverns : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override bool IsBiomeActive(Player player)
        {
            return player.ZoneRockLayerHeight && player.ZoneDirtLayerHeight &&

            ModContent.GetInstance<BiomeTileCount>().SlimeBlockCount >= 40 &&

            Math.Abs(player.position.ToTileCoordinates().X - Main.maxTilesX / 2) < Main.maxTilesX / 6;
        }
    }
}