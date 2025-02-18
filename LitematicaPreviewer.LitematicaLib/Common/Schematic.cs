using System.Collections.Generic;
using System.IO;
using SharpNBT;

namespace LitematicaPreviewer.LitematicaLib.Common
{
    public class Schematic
    {
        public string Name { get; init; }
        public string Author { get; init; }
        public string Description { get; init; }
        public int LmVersion { get; init; }
        public int LmSubversion { get; init; }
        public int McVersion { get; init; }
        public long Created { get; init; }
        public long Modified { get; init; }
        public Dictionary<string, Region> Regions { get; init; } = new();
        public List<int> Preview { get; init; } = [];
        public int Width { get; init; }
        public int Height { get; init; }
        public int Length { get; init; }

        public static Schematic Load(string path)
        {
            // 检查文件是否存在
            if(!File.Exists(path))
            {
                throw new FileNotFoundException("文件不存在", path);
            }

            // 读取文件
            var litematicaDocument = NbtFile.Read(path,FormatOptions.Java);

            var meta = litematicaDocument.Get<CompoundTag>("Metadata");
            if (meta == null)
            {
                throw new InvalidDataException("文件头信息读取错误");
            }

            var schematic = new Schematic
            {
                Name = meta.Get<StringTag>("Name"),
                Author = meta.Get<StringTag>("Author"),
                Description = meta.Get<StringTag>("Description"),
                LmVersion = litematicaDocument.Get<IntTag>("Version"),
                LmSubversion = litematicaDocument.Get<IntTag>("SubVersion"),
                McVersion = litematicaDocument.Get<IntTag>("MinecraftDataVersion"),
                Created = meta.Get<LongTag>("TimeCreated"),
                Modified = meta.Get<LongTag>("TimeModified"),
                Width = meta.Get<CompoundTag>("EnclosingSize").Get<IntTag>("x"),
                Height = meta.Get<CompoundTag>("EnclosingSize").Get<IntTag>("y"),
                Length = meta.Get<CompoundTag>("EnclosingSize").Get<IntTag>("z"),
            };


            foreach (var tag in litematicaDocument.Get<CompoundTag>("Regions"))
            {
                schematic.Regions.Add(tag.Name, Common.Region.FromCompoundTag((CompoundTag)tag));
            }

            return schematic;

        }
    }

}
