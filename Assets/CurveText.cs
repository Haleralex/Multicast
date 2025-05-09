using TMPro;
using UnityEngine;

public class CurveText : MonoBehaviour
{
    public TMP_Text tmpText;
    public float arcHeight = 15.0f;     // Высота дуги
    public bool invertArc = false;      // Перевернуть дугу
    public float rotationMultiplier = 0.5f; // Множитель поворота символов
    
    void Start()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        ApplyParabolaCurve();
    }
    
    void ApplyParabolaCurve()
    {
        if (tmpText == null) return;
        
        // Заставляем TextMeshPro обновить вершины текста
        tmpText.ForceMeshUpdate();
        
        TMP_TextInfo textInfo = tmpText.textInfo;
        
        if (textInfo.characterCount == 0) return;
        
        // Получаем информацию о ширине текста
        float textWidth = textInfo.characterInfo[textInfo.characterCount - 1].bottomRight.x - 
                         textInfo.characterInfo[0].bottomLeft.x;
                         
        // Проходим по всем символам в тексте
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            // Пропускаем невидимые символы
            if (!textInfo.characterInfo[i].isVisible) continue;
            
            // Получаем индексы вершин для символа
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            
            // Получаем массив вершин для текущего символа
            Vector3[] vertices = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;
            
            // Определяем горизонтальное положение символа относительно ширины текста
            float charCenter = (textInfo.characterInfo[i].bottomLeft.x + textInfo.characterInfo[i].bottomRight.x) / 2f;
            float charPosition = charCenter - textInfo.characterInfo[0].bottomLeft.x;
            float normalizedPosition = (textWidth > 0) ? charPosition / textWidth : 0;
            
            // Рассчитываем смещение для параболы: y = a * (x - 0.5)^2
            float xOffset = normalizedPosition - 0.5f;
            float yOffset = arcHeight * (xOffset * xOffset);
            
            if (invertArc) yOffset = -yOffset;
            
            // Расчет угла наклона касательной к параболе в данной точке
            // Производная параболы y = a(x-0.5)² равна 2a(x-0.5)
            float tangent = 2 * arcHeight * xOffset;
            if (invertArc) tangent = -tangent;
            
            // Расчет угла поворота в радианах
            float angle = Mathf.Atan(tangent) * rotationMultiplier;
            
            // Получаем центр символа для вращения
            Vector2 charMidPoint = new Vector2(charCenter, 
                (vertices[vertexIndex].y + vertices[vertexIndex + 2].y) / 2f);
            
            // Применяем поворот и смещение к вершинам символа
            for (int j = 0; j < 4; j++)
            {
                // Перемещаем вершину в центр символа
                Vector2 vertPos = new Vector2(vertices[vertexIndex + j].x, vertices[vertexIndex + j].y);
                Vector2 offset = vertPos - charMidPoint;
                
                // Применяем поворот
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);
                Vector2 rotatedOffset = new Vector2(
                    offset.x * cos - offset.y * sin,
                    offset.x * sin + offset.y * cos
                );
                
                // Возвращаем вершину обратно и добавляем вертикальное смещение параболы
                vertices[vertexIndex + j].x = rotatedOffset.x + charMidPoint.x;
                vertices[vertexIndex + j].y = rotatedOffset.y + charMidPoint.y + yOffset;
            }
        }
        
        // Обновляем меши для отображения изменений
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}