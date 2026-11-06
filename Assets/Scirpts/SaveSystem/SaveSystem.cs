using UnityEngine;

namespace Saving
{
    public static class SaveSystem
    {
        public static event System.Action OnSave;
        public static event System.Action OnLoad;

        private const string SaveKey = "Arkanoid.Save";

        
        public static GameSaveData Data { get; private set; }

        public static void Save()
        {
            string json = JsonUtility.ToJson(Data);

            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();

            OnSave?.Invoke();
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

            OnLoad?.Invoke();
        }
    }
}
