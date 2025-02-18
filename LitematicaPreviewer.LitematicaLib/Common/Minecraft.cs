using SharpNBT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LitematicaPreviewer.LitematicaLib.Common
{

    public record EntityPosition(double X, double Y, double Z);
    public record EntityRotation(float Pitch, float Yaw);
    public record EntityMotion(double X, double Y, double Z);
    public record BlockPosition(int X, int Y, int Z);

    public record BlockState(string BlockId, Dictionary<string,string> Properties, string IdentifierCache = null)
    {
        public static BlockState FromCompoundTag(CompoundTag tag)
        {
            string blockId = tag.Get<StringTag>("Name");

            var properties = new Dictionary<string, string>();

            if (!tag.ContainsKey("Properties"))
            {
                return new BlockState(blockId, properties);
            }

            foreach (StringTag property in tag.Get<CompoundTag>("Properties"))
            {
                properties.Add(property.Name, property);
            }

            return new BlockState(blockId, properties);
        }
    };

    public record Entity(
        string Id,
        CompoundTag Data,
        EntityPosition Position,
        EntityRotation Rotation,
        EntityMotion Motion)
    {
        public static Entity FromCompoundTag(CompoundTag tag)
        {
            string id = tag.Get<StringTag>("id");

            EntityPosition position;
            EntityRotation rotation;
            EntityMotion motion;
            if (tag.ContainsKey("Pos"))
            {
                var pos = tag.Get<ListTag>("Pos");
                position = new EntityPosition(
                    (DoubleTag)pos[0],
                    (DoubleTag)pos[1],
                    (DoubleTag)pos[2]
                );
            }
            else
            {
                position = new EntityPosition(0, 0, 0);
            }

            if (tag.ContainsKey("Rotation"))
            {
                var rot = tag.Get<ListTag>("Rotation");
                rotation = new EntityRotation(
                    (FloatTag)rot[0],
                    (FloatTag)rot[1]
                );
            }
            else
            {
                rotation = new EntityRotation(0, 0);
            }

            if (tag.ContainsKey("Motion"))
            {
                var mot = tag.Get<ListTag>("Motion");
                motion = new EntityMotion(
                    (DoubleTag)mot[0],
                    (DoubleTag)mot[1],
                    (DoubleTag)mot[2]
                );
            }
            else
            {
                motion = new EntityMotion(0, 0, 0);
            }

            return new Entity(id, tag, position, rotation, motion);
        }
    };

    public record TileEntity(CompoundTag Data, BlockPosition Position)
    {
        public static TileEntity FromTagCompound(CompoundTag tag)
        {
            int x = tag.ContainsKey("x") ? tag.Get<IntTag>("x") : 0;
            int y = tag.ContainsKey("y")? tag.Get<IntTag>("y") : 0;
            int z = tag.ContainsKey("z") ? tag.Get<IntTag>("z") : 0;
            var position = new BlockPosition(x, y, z);
            return new TileEntity(tag, position);
        }
    };
}
