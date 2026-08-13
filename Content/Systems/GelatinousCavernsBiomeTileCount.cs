using System;
using Terraria.ModLoader;
using Terraria.ID;

namespace anewascension.Content.Systems
{
    public class BiomeTileCount : ModSystem
    {
        public int SlimeBlockCount;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            SlimeBlockCount = tileCounts[TileID.SlimeBlock];
        }
    }
}