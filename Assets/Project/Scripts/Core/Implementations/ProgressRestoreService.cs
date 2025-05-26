using Cysharp.Threading.Tasks;
using Progress;

public class ProgressRestoreService : IProgressRestoreService
{
    private readonly LevelModel _levelModel;

    public ProgressRestoreService(LevelModel levelModel)
    {
        _levelModel = levelModel;
    }

    public async UniTask RestoreProgress(GameProgress progress)
    {
        if (progress?.WordPiecesMapping?.Count > 0)
        {
            _levelModel.InitializeFromProgress();
        }
        await UniTask.CompletedTask;
    }
}
