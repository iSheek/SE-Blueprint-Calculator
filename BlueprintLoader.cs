using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using System.Linq;

namespace SECalc;
public class BlueprintLoader
{
    public Dictionary<string, int> gridBlocks = new Dictionary<string, int>();


    public void Load(string path)
    {
        XDocument doc = XDocument.Load(path);
        
        var cubeBlocks = doc.Descendants()
                            .Where(e => e.Name.LocalName == "CubeBlocks");
        
        foreach (var blockElement in cubeBlocks.Elements())
        {
            var typeId = blockElement
                        .Attribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "type")?
                        .Value;

            if (string.IsNullOrEmpty(typeId))
            {
                typeId = blockElement.Name.LocalName;
            }

            var subtypeName = blockElement 
                                .Elements()
                                .FirstOrDefault(e => e.Name.LocalName == "SubtypeName")?
                                .Value ?? "";

            
            typeId = typeId.Replace("MyObjectBuilder_", "");

            string blockKey = $"{typeId}_{subtypeName}";

            if (gridBlocks.ContainsKey(blockKey))
            {
                gridBlocks[blockKey]++;
            }
            else
            {
                gridBlocks[blockKey] = 1;
            }

        }

    }

}