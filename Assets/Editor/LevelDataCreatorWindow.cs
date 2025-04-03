using UnityEditor;
using UnityEngine;

public class LevelDataCreatorWindow : EditorWindow
{
    private string levelName = "NewLevel";
    private string word1 = "привет";
    private string word2 = "ворота";
    private string word3 = "калина";
    private string word4 = "базука";
    private string message = ""; // Поле для отображения сообщений

    [MenuItem("Tools/Level Data Creator")]
    public static void ShowWindow()
    {
        GetWindow<LevelDataCreatorWindow>("Level Data Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Введите данные для генерации уровня", EditorStyles.boldLabel);

        levelName = EditorGUILayout.TextField("Название уровня", levelName);
        word1 = EditorGUILayout.TextField("Слово 1", word1);
        word2 = EditorGUILayout.TextField("Слово 2", word2);
        word3 = EditorGUILayout.TextField("Слово 3", word3);
        word4 = EditorGUILayout.TextField("Слово 4", word4);

        if (GUILayout.Button("Сгенерировать уровень"))
        {
            GenerateLevel();
        }

        // Отображение сообщения
        if (!string.IsNullOrEmpty(message))
        {
            EditorGUILayout.HelpBox(message, MessageType.Info);
        }
    }

    private void GenerateLevel()
    {
        if (string.IsNullOrEmpty(levelName))
        {
            message = "Название уровня не может быть пустым!";
            Repaint(); // Обновляем окно
            return;
        }

        string[] words = { word1, word2, word3, word4 };

        foreach (var word in words)
        {
            if (string.IsNullOrEmpty(word))
            {
                message = "Все 4 слова должны быть заполнены!";
                Repaint(); // Обновляем окно
                return;
            }

            if (word.Length != 6)
            {
                message = $"Слово '{word}' должно содержать ровно 6 букв!";
                Repaint(); // Обновляем окно
                return;
            }
        }

        LevelDataCreator creator = new();
        if (creator == null)
        {
            message = "LevelDataCreator не найден в сцене!";
            Repaint(); // Обновляем окно
            return;
        }

        creator.GenerateLevelData(levelName, words);
        message = $"Уровень '{levelName}' успешно сгенерирован!";
        Repaint(); // Обновляем окно
    }
}