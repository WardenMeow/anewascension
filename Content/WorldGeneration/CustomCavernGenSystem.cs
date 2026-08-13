using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace anewascension.Content.WorldGeneration
{
    public class CustomCavernGenSystem : ModSystem
    {
        // Injects your custom pass into the standard Terraria worldgen workflow
        public void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) {
            // "Shinies" is the vanilla pass where ores spawn. 
            // Spawning right after it ensures the main terrain is already fully generated.
            int shiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            
            if (shiniesIndex != -1) {
                // Insert a new generation pass right after "Shinies"
                tasks.Insert(shiniesIndex + 1, new PassLegacy("Gelatinous Caverns", GenerateCustomCavernBiome));
            }
        }

        private void GenerateCustomCavernBiome(GenerationProgress progress, GameConfiguration configuration) {
            // Update the loading screen text
            progress.Message = "Gelling The Depths...";

            // Determine how many clusters to spawn based on the world size
            // Main.maxTilesX values: Small = 4200, Medium = 6400, Large = 8400
            int numClusters = (int)(Main.maxTilesX * Main.maxTilesY * 0.00002);

            // Fetch your custom block type safely
            int targetTile = TileID.SlimeBlock;

            for (int k = 0; k < numClusters; k++) {
                // Pick a random X coordinate, leaving a 200-tile buffer from the world edges
                int x = WorldGen.genRand.Next(200, Main.maxTilesX - 200);

                // Pick a random Y coordinate strictly within the cavern layer
                // Starts at rockLayer and stops 300 tiles above the underworld/bottom boundary
                int minY = (int)Main.rockLayer;
                int maxY = Main.maxTilesY - 300;
                int y = WorldGen.genRand.Next(minY, maxY);

                // Ensure we only place the biome core if it lands inside solid ground (Stone/Dirt)
                if (Main.tile[x, y].HasTile && (Main.tile[x, y].TileType == TileID.Stone || Main.tile[x, y].TileType == TileID.Dirt)) {
                    
                    // WorldGen.TileRunner generates a blob-like cluster of blocks
                    // Parameters: (X, Y, Random Strength, Random Steps, TileType, OverwriteExisting, SpeedX, SpeedY, NoInvert, Food)
                    WorldGen.TileRunner(
                        x, 
                        y, 
                        WorldGen.genRand.Next(35, 60),  // Width/strength of the blob
                        WorldGen.genRand.Next(40, 75),  // Length/steps of the blob tracking
                        targetTile, 
                        true,                           // Overwrite existing tiles
                        0f,                             // Speed X
                        0f,                             // Speed Y
                        false, 
                        true
                    );
                }
            }
        }
    }
}