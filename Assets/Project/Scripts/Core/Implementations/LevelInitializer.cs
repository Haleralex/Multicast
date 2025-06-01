using System.Collections.Generic;
using ZLinq;
using Core.Implementations;
using Progress;
using Utils;
using Core.Interfaces;
using System.Linq;
using Cysharp.Threading.Tasks;

public class LevelInitializer : ILevelInitializer
{
    private readonly ILevelView _levelView;
    private readonly IWordPieceSlotsService _slotsService;
    private readonly IWordPiecesService _wordPiecesService;
    private readonly ILevelAnimationService _animationService;
    private readonly IProgressRestoreService _progressService;

    public LevelInitializer(
        ILevelView levelView,
        IWordPieceSlotsService slotsService,
        IWordPiecesService wordPiecesService,
        ILevelAnimationService animationService,
        IProgressRestoreService progressService)
    {
        _levelView = levelView;
        _slotsService = slotsService;
        _wordPiecesService = wordPiecesService;
        _animationService = animationService;
        _progressService = progressService;
    }

    public async void InitializeLevel(
        Dictionary<int, Dictionary<int, string>> correctMapping,
        Dictionary<int, string> missingSlots,
        GameProgress progress = null)
    {
        await ResetLevel();

        var wordPieceSlots = await _slotsService.CreateWordPieceSlots(correctMapping, missingSlots);
        var wordPieces = await _wordPiecesService.CreateWordPieces(correctMapping, missingSlots);

        _levelView.SetUIElements(wordPieceSlots, wordPieces);
        await _animationService.PlayLevelStartAnimation(wordPieces);

        if (progress != null)
        {
            await _progressService.RestoreProgress(progress);
        }
    }


    private async UniTask ResetLevel()
    {
        await UniTask.WhenAll(
            _slotsService.ResetSlots(),
            _wordPiecesService.ResetWordPieces()
        );
    }
}
