using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Localization", menuName = "Localization Strings", order = 0)]
public class LocalizationSO : ScriptableObject
{
    public Language language;
    public LocalizationElement[] element;

    public string GetElement (string key)
    {
        return element.First(e => e.key == key).value;
    }
}
