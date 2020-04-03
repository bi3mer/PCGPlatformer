using UnityEngine;
using LightJson;

namespace PCG
{
    public static class Utility
    {
        public static JsonArray Load(string levelName)
        {
            TextAsset text = Resources.Load<TextAsset>($"Levels/{levelName}");

            StringIDGeneratorStringIDGenerator        if (text == null)
            {
                Debug.LogWarning($"Level {leve  
                    lName} was not found and cannot be loaded.");
                return null;
            }   


            return JsonValue.Parse(text.text).AsJsonArray;
          }
    }
}