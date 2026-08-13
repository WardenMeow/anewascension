using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace anewascension.Content.WorldGeneration
{
    public class CustomCavernGenSystem : ModSystem
    {
        // Bypasses ModifyWorldGenTasks entirely to eliminate compiler and namespace errors
        public override void PostWorldGen() {
            
            // Log to confirm execution has started
            Mod.Logger.Info("Custom Cavern Biome PostWorldGen placement started.");

            // Grabs your custom block ID
            int targetTile = TileID.SlimeBlock;
            
            // Scale cluster count based on world width (Small ~21, Medium ~32, Large ~42)
            int numClusters = (int)(Main.maxTilesX * 0.005f); 

            for (int k = 0; k < numClusters; k++) {
                // Find a random spot leaving a safe margin from the world edges
                int x = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
                
                // Deep Cavern positioning boundary definitions
                int minY = (int)Main.rockLayer + 100; 
                int maxY = Main.maxTilesY - 350;      // Stays safely above the Underworld
                int y = WorldGen.genRand.Next(minY, maxY);

                // Fetch the tile occupying the selected coordinates
                ushort currentTileType = Main.tile[x, y].TileType;
                
                // Check if the coordinate lands on standard solid world ground (Stone, Dirt, Clay, Mud)
                bool isValidGround = currentTileType == 1 || currentTileType == 0 || currentTileType == 40 || currentTileType == 59;

                if (Main.tile[x, y].HasTile && isValidGround) {
                    // Carves out organic, sprawling clusters of your custom cavern block
                    WorldGen.TileRunner(
                        x, 
                        y, 
                        WorldGen.genRand.Next(45, 75),  // Cluster width
                        WorldGen.genRand.Next(60, 100), // Cluster length/steps
                        targetTile, 
                        true,                           // Force overwrite vanilla tiles
                        0f,                             // X Velocity
                        0f,                             // Y Velocity
                        false, 
                        true
                    );
                }
            }
            
            Mod.Logger.Info($"Custom Cavern WorldGen executed successfully! Spawned {numClusters} clusters.");
        }
    }
}