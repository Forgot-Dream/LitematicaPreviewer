using SharpNBT;
using System;
using System.Collections.Generic;

namespace LitematicaPreviewer.LitematicaLib.Common
{
    public record Region(int X, int Y, int Z, int Width, int Height, int Length)
    {
        public List<BlockState> Palette { get; init; } = [];
        public int[,,] Blocks { get; init; } = new int[Width, Height, Length];
        public List<Entity> Entities { get; init; } = [];
        public List<CompoundTag> BlockTicks { get; init; } = [];
        public List<CompoundTag> FluidTicks { get; init; } = [];
        public List<TileEntity> TileEntities { get; init; } = [];


        public static Region FromCompoundTag(CompoundTag tag)
        {
            var pos = tag.Get<CompoundTag>("Position");
            int x = pos.Get<IntTag>("x");
            int y = pos.Get<IntTag>("y");
            int z = pos.Get<IntTag>("z");

            var size = tag.Get<CompoundTag>("Size");
            int width = size.Get<IntTag>("x");
            int height = size.Get<IntTag>("y");
            int length = size.Get<IntTag>("z");

            var region = new Region(x, y, z, int.Abs(width), int.Abs(height), int.Abs(length));

            var palette = tag.Get<ListTag>("BlockStatePalette");
            foreach (var blockNbt in palette)
            {
                var block = BlockState.FromCompoundTag((CompoundTag)blockNbt);
                region.Palette.Add(block);
            }

            var entities = tag.Get<ListTag>("Entities");
            foreach (var entityNbt in entities)
            {
                var entity = Entity.FromCompoundTag((CompoundTag)entityNbt);
                region.Entities.Add(entity);
            }

            var tileEntities = tag.Get<ListTag>("TileEntities");
            foreach (var tileEntityNbt in tileEntities)
            {
                var tileEntity = TileEntity.FromTagCompound((CompoundTag)tileEntityNbt);
                region.TileEntities.Add(tileEntity);
            }

            var blocks = tag.Get<LongArrayTag>("BlockStates");
            int nbits = GetNeededNbits(region.Palette.Count);
            var bitArray = LitematicaBitArray.FromNbtLongArray(blocks, region.Volume(), nbits);

            for (int x1 = 0; x1 < region.Width; x1++)
            {
                for (int y1 = 0; y1 <region.Height; y1++)
                {
                    for (int z1 = 0; z1 < region.Length; z1++)
                    {
                        int ind = (y1 * region.Width * region.Length) + (z1 * region.Height) + x1;
                        region.Blocks[x1, y1, z1] = bitArray[ind];
                    }
                }
            }

            var blockTicks = tag.Get<ListTag>("PendingBlockTicks");
            foreach (var blockTick in blockTicks)
            {
                region.BlockTicks.Add((CompoundTag)blockTick);
            }

            var fluidTicks = tag.Get<ListTag>("PendingFluidTicks");
            foreach (var fluidTick in fluidTicks)
            {
                region.FluidTicks.Add((CompoundTag)fluidTick);
            }

            return region;
        }

        private static int GetNeededNbits(int paletteSize)
        {
            // 计算所需的位数
            return Math.Max((int)Math.Ceiling(Math.Log2(paletteSize)), 2);
        }

        private int Volume()
        {
            return Width * Height * Length;
        }
    }
}
