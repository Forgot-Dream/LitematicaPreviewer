using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitematicaPreviewer.Common.Structs;
using LitematicaPreviewer.LitematicaLib.Common;

namespace LitematicaPreviewer.Common.Extension
{
    public static class SchematicExtension
    {
        /// <summary>
        /// 拓展方法：获取物品信息
        /// </summary>
        /// <param name="schematic"></param>
        /// <returns></returns>
        public static List<MaterialInfoItem> GetMaterialInfo(this Schematic schematic)
        {
            var materialInfoDict = new Dictionary<string, int> ();

            foreach (var block in 
                     from region in schematic.Regions.Values 
                     from int ind in region.Blocks 
                     select region.Palette[ind] into block 
                     where !materialInfoDict.TryAdd(block.BlockId, 1) 
                     select block)
            {
                materialInfoDict[block.BlockId] += 1;
            }

            var materialInfo = materialInfoDict.Select(pair => new MaterialInfoItem(pair.Key, pair.Key, pair.Value)).ToList();

            materialInfo.Sort((item, infoItem) => item.Count.CompareTo(infoItem.Count));

            return materialInfo;
        }
    }
}
