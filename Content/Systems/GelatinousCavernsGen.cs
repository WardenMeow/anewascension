using MonoMod.Cil;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace anewascension.Content.Systems
{
    public class GelatinousCavernsGen : ModSystem
    {
        // Register generation pass into world gen task list
        // Removed 'override' to match current ModSystem signature if override is unavailable
        public void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int index = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (index != -1)
            {
                tasks.Insert(index + 1, new PassLegacy("Gelatinous Caverns", (GenerationProgress progress, GameConfiguration _config) => GenerateGelatinousCaverns(progress)));
            }
        }

        // Simple generation pass stub. Accepts GenerationProgress as required by AddGenerationPass.
        private static void GenerateGelatinousCaverns(GenerationProgress progress)
        {
            progress.Message = "Generating Gelatinous Caverns";
            
            // Generate multiple cavern clusters throughout the world
            int cavernCount = Main.maxTilesX / 150; // Scale caverns based on world size
            
            for (int i = 0; i < cavernCount; i++)
            {
                // Random position in cavern layer
                int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                int y = WorldGen.genRand.Next((int)(Main.maxTilesY * 0.5), (int)(Main.maxTilesY * 0.75));
                
                // Generate cavern chamber
                int radius = WorldGen.genRand.Next(20, 40);
                GenerateCavernChamber(x, y, radius);
                
                progress.Set((float)i / cavernCount);
            }
        }

        private static void GenerateCavernChamber(int centerX, int centerY, int radius)
        {
            // Create gelatinous cavern chamber with organic shapes
            for (int x = centerX - radius; x < centerX + radius; x++)
            {
                for (int y = centerY - radius; y < centerY + radius; y++)
                {
                    if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
                        continue;

                    // Calculate distance from center with some noise for organic shapes
                    int distX = x - centerX;
                    int distY = y - centerY;
                    double distance = Math.Sqrt(distX * distX + distY * distY);
                    double noiseOffset = (Math.Sin(x * 0.05) + Math.Cos(y * 0.05)) * 5;
                    
                    if (distance < radius + noiseOffset)
                    {
                        // Clear the tile to create open space
                        Main.tile[x, y].ClearTile();
                        
                        // Optionally add gelatinous walls or liquid
                        if (distance > radius + noiseOffset - 3)
                        {
                            // Create wall edges
                            Main.tile[x, y].WallType = WallID.Slime;
                        }
                    }
                }
            }
        }
    }
}