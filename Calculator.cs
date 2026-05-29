using System.Linq;
using System.Collections.Generic;

public class Calculator
{
    public Dictionary<string, int> Calculate(Dictionary<string, int> blueprintDict, Dictionary<string, Dictionary<string,int>> sourceDict)
    {
        Dictionary<string, int> totalComponents = new Dictionary<string, int>();

        foreach (var blueprintBlock in blueprintDict)
        {
            if (sourceDict.TryGetValue(blueprintBlock.Key, out var components))
            {
                foreach (var component in components)
                {
                    int sum = blueprintBlock.Value * component.Value;

                    if (totalComponents.ContainsKey(component.Key))
                    {
                        totalComponents[component.Key] += sum; 
                    }
                    else
                    {
                        totalComponents[component.Key] = sum;
                    }
                }
            }
        }

        return totalComponents;

    }
}