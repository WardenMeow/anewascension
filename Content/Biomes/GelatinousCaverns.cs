using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace anewascension.Content.Biomes
{
    public class GelatinousCaverns : ModBiome
    {
        // Name of the biome displayed in-game (e.g., for mods like Census)
        public override string Name => "Gelatinous Caverns";

        // Automatically set the priority so it overrides normal caverns
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

        // This determines if the player is counted as "inside" your biome
        public override bool IsBiomeActive(Player player) {
            // Requirement 1: Must be in the cavern layer or deeper
            bool isCavernDepth = player.ZoneRockLayerHeight;

            // Requirement 2: Must have enough custom tiles nearby (e.g., 100 tiles)
            // ModSystem tile counting logic links directly to this system
            bool hasEnoughTiles = ModContent.GetInstance<CustomCavernTileCounterSystem>().CustomTileCount >= 100;

            return isCavernDepth && hasEnoughTiles;
        }
    }

    // This system counts your custom biome tiles in the player's vicinity
    public class CustomCavernTileCounterSystem : ModSystem
    {
        public int CustomTileCount;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
            
            int tileType = TileID.SlimeBlock;
            
            // Extract the count of your specific tile from the game's scanned tiles
            CustomTileCount = tileCounts[tileType];
        }
    }
}