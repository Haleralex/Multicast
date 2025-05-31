using Cysharp.Threading.Tasks;
using Data;

public interface ILevelDataLoader
{
    UniTask<HandledLevelData> LoadCurrentLevelDataAsync();
    UniTask<int> GetLevelAssetsCountAsync();
}