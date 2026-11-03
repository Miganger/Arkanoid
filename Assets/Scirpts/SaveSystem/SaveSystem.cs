using UnityEngine;

namespace Saving
{
    public static class SaveSystem
    {
        private const string SaveKey = "Arkanoid.Save";

        
        public static GameSaveData Data { get; private set; }

        public static void Save()
        {
            string json = JsonUtility.ToJson(Data);

            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public static void Load()
        {
            GameSaveData saveData = new GameSaveData();

            if (!PlayerPrefs.HasKey(SaveKey))
            {
                Data = saveData;
                return;
            }

            string json = PlayerPrefs.GetString(SaveKey);

            try
            {
                JsonUtility.FromJsonOverwrite(json, saveData);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Save loading failed: {e.Message}");
            }

            Data = saveData;
        }
    }
}
