using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using System.Linq;

namespace SECalc;
public class SourceBlocksLoader
{
    public Dictionary<string, Dictionary<string,int>> blocks 
        = new Dictionary<string, Dictionary<string,int>>();
    
    public void Load(string path)
    {
        var files = 
        Directory.GetFiles(path, "*.sbc", SearchOption.AllDirectories)
        .Where(f => Path.GetFileName(f).StartsWith("CubeBlocks_"));


        foreach (string file in files)
        {   
            XDocument doc = XDocument.Load(file);

            var blockElements = doc.Descendants()
                .Where(e => e.Name.LocalName == "Definition");


            foreach (var blockElement in blockElements)
            {
                var idElement = blockElement
                                .Elements()
                                .FirstOrDefault(e => e.Name.LocalName == "Id");

                if (idElement == null) continue;

                var typeId = idElement
                            .Elements()
                            .FirstOrDefault(e => e.Name.LocalName == "TypeId")?
                            .Value;
                
                var subtypeId = idElement
                                .Elements()
                                .FirstOrDefault(e => e.Name.LocalName == "SubtypeId")?.Value ?? "";

                string blockKey = $"{typeId}_{subtypeId}";
                



                var componentsElement = blockElement
                                        .Elements()
                                        .FirstOrDefault(e => e.Name.LocalName == "Components");
                
                if (componentsElement != null)
                {
                    Dictionary<string, int> blockCost = new Dictionary<string, int>();

                    var componentList = componentsElement
                                        .Elements()
                                        .Where(e => e.Name.LocalName == "Component");
                    
                    foreach(var comp in componentList)
                    {
                        string? compName = comp.Attribute("Subtype")?.Value;
                        string? compCountStr = comp.Attribute("Count")?.Value;

                        if (compName != null && int.TryParse(compCountStr, out int compCount))
                        {
                            if (blockCost.ContainsKey(compName))
                            {
                                blockCost[compName] += compCount;
                            }
                            else
                            {
                                blockCost[compName] = compCount;
                            }
                        }
                    
                    }

                    blocks[blockKey] = blockCost;

                }
                    

            }

        }
    }
}