using UnityEngine;

namespace PCG
{
    public static class Utility
    {
        public static string[] Load(string levelName)
        {
            TextAsset text = Resources.Load<TextAsset>($"Levels/{levelName}");

            if (text == null)
            {
                Debug.LogError($"Level {levelName} was not found and cannot be loaded.");
                return null;
            }   


            return text.text.Split('\n');
          }
    }
}