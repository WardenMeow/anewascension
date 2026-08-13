using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace anewascension.Content.WorldGeneration
{
    public class CustomCavernGenSystem : ModSystem
    {
        public override void PostWorldGen() {
            Mod.Logger.Info("Generating open Slime Caverns...");

            ushort tileSlime = TileID.SlimeBlock;
            ushort wallSlime = WallID.Slime; // Vanilla unsafe slime wall

            // Scale generation based on world width (Small ~16, Medium ~25, Large ~33)
            int numCaverns = (int)(Main.maxTilesX * 0.004f); 

            for (int k = 0; k < numCaverns; k++) {
                int centerX = WorldGen.genRand.Next(400, Main.maxTilesX - 400);
                int minY = (int)Main.rockLayer + 150; 
                int maxY = Main.maxTilesY - 400; // Safe distance above underworld
                int centerY = WorldGen.genRand.Next(minY, maxY);

                // Open cavern dimensions
                int radiusX = WorldGen.genRand.Next(55, 85);
                int radiusY = WorldGen.genRand.Next(30, 50);

                for (int x = centerX - radiusX; x <= centerX + radiusX; x++) {
                    for (int y = centerY - radiusY; y <= centerY + radiusY; y++) {
                        
                        if (x <= 0 || x >= Main.maxTilesX || y <= 0 || y >= Main.maxTilesY)
                            continue;

                        // Calculate distance from center to construct an ellipse
                        float dx = (float)(x - centerX) / radiusX;
                        float dy = (float)(y - centerY) / radiusY;
                        float distanceSquared = (dx * dx + dy * dy);

                        if (distanceSquared <= 1.0f) {
                            Tile tile = Main.tile[x, y];

                            // STAGE 1: Outer Shell Boundary Coating (Gives it a thick rim of slime blocks)
                            if (distanceSquared > 0.75f && distanceSquared <= 1.0f) {
                                if (tile.HasTile && IsOverwritable(tile.TileType)) {
                                    tile.TileType = tileSlime;
                                }
                            }
                            // STAGE 2: Cave Core Carving (Clears the air & paints the unsafe slime background)
                            else {
                                // Wipe out solid blocks to create an open room like marble/granite biomes
                                tile.HasTile = false; 
                                tile.TileType = 0; 
                                
                                // Place vanilla slime background wall (will naturally spawn mobs)
                                tile.WallType = wallSlime;
                            }
                        }
                    }
                }

                // STAGE 3: Organic Ledge Generation (Spawns flooring paths across the open space)
                GenerateSlimeLedges(centerX, centerY, radiusX, radiusY, tileSlime);
            }

            Mod.Logger.Info("Slime Caverns successfully carved out!");
        }

        private static bool IsOverwritable(ushort type) {
            // Protect dungeons, temples, and existing modded setups
            return type == TileID.Stone || type == TileID.Dirt || type == TileID.ClayBlock || type == TileID.Mud;
        }

        private static void GenerateSlimeLedges(int cx, int cy, int rx, int ry, ushort tileType) {
            // Adds horizontal strips of slime blocks inside the cave room so it isn't just an empty bubble
            int numLedges = WorldGen.genRand.Next(2, 5);
            for (int i = 0; i < numLedges; i++) {
                int ledgeY = cy + WorldGen.genRand.Next(-ry + 10, ry - 10);
                int startX = cx - WorldGen.genRand.Next(rx / 2, rx);
                int endX = cx + WorldGen.genRand.Next(rx / 2, rx);
                int thickness = WorldGen.genRand.Next(3, 6);

                for (int x = startX; x <= endX; x++) {
                    for (int y = ledgeY; y < ledgeY + thickness; y++) {
                        if (x > 0 && x < Main.maxTilesX && y > 0 && y < Main.maxTilesY) {
                            // Only draw if inside the carved cave sphere boundary
                            float dx = (float)(x - cx) / rx;
                            float dy = (float)(y - cy) / ry;
                            if ((dx * dx + dy * dy) < 0.85f) {
                                Tile tile = Main.tile[x, y];
                                tile.HasTile = true;
                                tile.TileType = tileType;
                            }
                        }
                    }
                }
            }
        }
    }
}