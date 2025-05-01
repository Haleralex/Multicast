
# Project Title

A brief description of what this project does and who it's for

# Multicast - Проектная документация

## Содержание
1. Архитектура кода
2. Процесс загрузки уровней через CCD + Addressables
3. Оптимизация

## Архитектура кода

### Структура проекта
Проект организован модульно с использованием Assembly Definitions (asmdef), что обеспечивает четкое разделение ответственности и слабую связанность компонентов:

- **Core** - основная игровая логика и механики
- **Data** - структуры данных и модели
- **Progress** - управление прогрессом и сохранением игры
- **UISystem** - компоненты пользовательского интерфейса
- **Utils** - вспомогательные утилиты
- **Navigation** - управление переходами между экранами интерфейса
- **Bootstrap** - инициализация игры и загрузка ресурсов

### Используемые паттерны

#### 1. Dependency Injection (Zenject)
Проект построен с использованием фреймворка Zenject для инъекции зависимостей, что делает код более тестируемым и модульным:

```csharp
[Inject]
public LevelPresenter(
    LevelModel model,
    ILevelView view,
    ILevelInitializer levelInitializer,
    IValidator validator,
    ISceneLoader sceneLoader)
{
    this.model = model;
    this.view = view;
    this.levelInitializer = levelInitializer;
    this.sceneLoader = sceneLoader;
    this.validator = validator;
}
```

#### 2. Model-View-Presenter (MVP)
Игровая логика организована по паттерну MVP:

- **Model** (`LevelModel`) - хранит данные уровня и бизнес-логику
- **View** (`LevelView`) - отвечает за визуальное представление и получение пользовательского ввода
- **Presenter** (`LevelPresenter`) - связывает Model и View, обрабатывает события UI и обновляет данные

```csharp
public async void Initialize()
{
    view.WordPieceSelected += OnWordPieceSelected;
    view.WordPieceReleased += OnWordPieceReleased;
    // ...
    
    var currentProgress = model.GetCurrentProgress();
    var currentLevelData = await model.GetCurrentLevelData();
    levelInitializer.InitializeLevel(currentLevelData.CorrectSlotsMapping, 
                                   currentLevelData.MissingSlotsMapping, 
                                   currentProgress);
}
```

#### 3. Object Pool
Для оптимизации производительности используется пул объектов `WordPiecesPool`:

```csharp
public void InitializeLevel(...)
{
    foreach (var wordPiece in activeWordPieces)
    {
        wordPiecesPool.ReturnWordPiece(wordPiece);
    }
    activeWordPieces.Clear();
    // ...
    var availableWordPiece = wordPiecesPool.GetWordPiece();
    // ...
}
```

#### 4. Observer
Используется для коммуникации между компонентами через события:

```csharp
// Объявление событий
public event Action<WordPiece> WordPieceSelected;
public event Action<WordPiece> WordPieceReleased;
public event Action<WordPiece> WordPieceDoubleClicked;
public event Action ValidateLevelPressed;
public event Action NextLevelPressed;
public event Action GoToMenuPressed;

// Подписка на события
private void OnEnable()
{
    wordPiecesView.WordPieceSelected += OnWordPieceSelected;
    wordPiecesView.WordPieceReleased += OnWordPieceReleased;
    // ...
}
```

#### 5. Command
Изменения состояния игры моделируются как команды через `MappingUpdate`:

```csharp
public class MappingUpdate
{
    public enum UpdateType { Add, Remove, Clear, InitializeAll }
    
    // Фабричные методы для создания различных типов обновлений
    public static MappingUpdate AddOrUpdate(int wordPieceId, int slotId) =>
        new MappingUpdate(UpdateType.Add, wordPieceId, slotId);
    
    public static MappingUpdate Remove(int wordPieceId) =>
        new MappingUpdate(UpdateType.Remove, wordPieceId, null);
    // ...
}
```

#### 6. Strategy
Использует интерфейс `IValidator` для разных стратегий валидации уровней:

```csharp
private void OnValidateLevelPressed()
{
    // ...
    bool isValid = validator.IsLevelValid(expectedWord, fragmentsCache, currentMappings);
    // ...
}
```

#### 7. Factory
Косвенно используется через Zenject для создания зависимостей и управления их жизненным циклом.

## Процесс загрузки уровней через CCD + Addressables

### Инициализация загрузки

Процесс загрузки начинается в `BootstrapInitializer`, который инициирует асинхронную загрузку ресурсов:

```csharp
public void Initialize()
{
    InitializeAsync().Forget();
}

private async UniTaskVoid InitializeAsync()
{
    try
    {
        await assetLoader.LoadAssets(300);
        menuLoader.LoadMenu();
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Ошибка при инициализации: {ex.Message}");
    }
}
```

### Загрузка ассетов и отслеживание прогресса

`AssetLoader` управляет процессом загрузки и предоставляет информацию о прогрессе:

```csharp
public async UniTask LoadAssets(int fakeLoadDelayMilliSeconds = 0)
{
    try
    {
        LoadingStarted?.Invoke();
        var e = Addressables.DownloadDependenciesAsync("Game_Level");

        while (!e.IsDone)
        {
            ProgressChanged?.Invoke(e.PercentComplete);
            await UniTask.Yield();
        }

        if (e.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError($"Failed to download assets: {e.OperationException}");
        }

        Addressables.Release(e);
        ProgressChanged?.Invoke(1);
        await UniTask.Delay(fakeLoadDelayMilliSeconds); 
        LoadingCompleted?.Invoke();
    }
    catch (Exception ex)
    {
        Debug.LogException(ex);
    }
}
```

### Загрузка данных уровня

Данные уровня загружаются из JSON-файлов, хранящихся в Addressables:

```csharp
public async UniTask<HandledLevelData> GetCurrentLevelData()
{
    var levelIndex = progressManager.LoadProgress().CurrentLevel % 4;
    var handle = Addressables.LoadAssetAsync<TextAsset>($"LevelData_{levelIndex}");
    TextAsset jsonAsset = await handle.Task;

    if (jsonAsset == null)
    {
        Debug.LogError($"Failed to load LevelData JSON for level {levelIndex}");
        return null;
    }

    try
    {
        LevelData levelData = JsonConvert.DeserializeObject<LevelData>(jsonAsset.text);
        // Обработка данных уровня...
        
        Addressables.Release(handle);
        var handledLevelData = new HandledLevelData(correctSlotsMapping, missingSlotsMapping);
        return handledLevelData;
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Failed to deserialize LevelData JSON: {ex.Message}");
        return null;
    }
}
```

### Формат данных уровня

Уровни хранятся в формате JSON и содержат информацию о словах и их фрагментах:

```json
{
    "Words": [
        {
            "fullWord": "москва",
            "clusters": [
                "мос",
                "ква"
            ],
            "missingClusters": [0]
        },
        // другие слова...
    ]
}
```

### Визуализация загрузки

`AssetLoadingDisplay` отображает прогресс загрузки:

```csharp
void OnEnable()
{
    assetLoader.LoadingCompleted += OnLoadingCompleted;
    assetLoader.LoadingStarted += OnLoadingStarted;
    assetLoader.ProgressChanged += OnProgressChanged;
}

private void OnProgressChanged(float obj)
{
    loadingBar.value = obj;
}
```

### Инициализация уровня

После загрузки данных, `LevelInitializer` настраивает игровые элементы:

```csharp
public void InitializeLevel(
    Dictionary<int, Dictionary<int, string>> correctMapping,
    Dictionary<int, string> missingSlots, 
    GameProgress progress = null)
{
    // Очистка предыдущих элементов и инициализация новых
    foreach (var wordPiece in activeWordPieces)
    {
        wordPiecesPool.ReturnWordPiece(wordPiece);
    }
    activeWordPieces.Clear();
    
    // Создание слотов и фрагментов
    // ...
    
    // Восстановление прогресса, если необходимо
    if (progress != null && progress.WordPiecesMapping.Count > 0)
    {
        levelModel.InitializeFromProgress();
    }
}
```

## Оптимизация

### 1. Объектные пулы

Использование пулов объектов для фрагментов слов предотвращает частое создание и уничтожение объектов:

```csharp
// Получение объекта из пула
var availableWordPiece = wordPiecesPool.GetWordPiece();

// Возврат объекта в пул
wordPiecesPool.ReturnWordPiece(wordPiece);
```

### 2. Асинхронная загрузка

Использование UniTask для асинхронных операций предотвращает блокировку основного потока:

```csharp
public async UniTask<HandledLevelData> GetCurrentLevelData()
{
    // Асинхронная загрузка без блокировки
    var handle = Addressables.LoadAssetAsync<TextAsset>($"LevelData_{levelIndex}");
    TextAsset jsonAsset = await handle.Task;
    // ...
}
```

### 3. Управление ресурсами Addressables

Корректное освобождение ресурсов для предотвращения утечек памяти:

```csharp
var handle = Addressables.LoadAssetAsync<TextAsset>($"LevelData_{levelIndex}");
// ...
Addressables.Release(handle);
```

### 4. Эффективные структуры данных

Использование словарей (Dictionary) для быстрого доступа к данным:

```csharp
private Dictionary<int, int> wordPiecesMappings = new();
private Dictionary<int, string> missingSlotsMapping = new();
private Dictionary<int, Dictionary<int, string>> correctSlotsMapping = new();
```

### 5. Модульная архитектура

Использование Assembly Definitions (asmdef) для модульности проекта:
- Ускоряет компиляцию (только измененные модули перекомпилируются)
- Улучшает организацию кода
- Обеспечивает четкие границы между компонентами

### 6. Кэширование

Кэширование часто используемых ссылок на компоненты:

```csharp
private Dictionary<int, string> fragmentsCache = new();
```

### 7. Оптимизация UI

Динамическое создание только необходимых UI-элементов и повторное использование существующих:

```csharp
public void SetUIElements(List<WordPieceSlot> slots, List<WordPiece> wordPieces)
{
    wordSlotsView.SetWordPieceSlots(slots);
    wordPiecesView.SetWordPieces(wordPieces);
    simpleLoadingScreen.gameObject.SetActive(false);
}
```

### 8. Оптимизация памяти

Использование StringBuilders для построения строк без избыточных аллокаций:

```csharp
public string GetWordForCompleteLevel()
{
    var strBuilder = new System.Text.StringBuilder();
    foreach (var k in correctSlotsMapping)
    {
        foreach (var v in k.Value.Where(v => missingSlotsMapping.ContainsKey(v.Key)))
        {
            strBuilder.Append(v.Value);
        }
    }
    return strBuilder.ToString();
}
```

### 9. Zenject TaskUpdater

Использование Zenject TaskUpdater для эффективного управления обновляемыми компонентами с приоритизацией выполнения.

---

Это документация охватывает основные архитектурные решения, процесс загрузки уровней и оптимизационные техники, использованные в проекте Multicast. Архитектура обеспечивает модульность, тестируемость и расширяемость, а процесс загрузки построен с учетом эффективного управления ресурсами и оптимальной производительности.
