using System.Collections.Generic;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
public class LevelAnimationService : ILevelAnimationService
{
    private readonly IWordPieceAnimator _animator;

    public LevelAnimationService(IWordPieceAnimator animator)
    {
        _animator = animator;
    }

    public async UniTask PlayLevelStartAnimation(List<WordPiece> wordPieces)
    {
        List<GameObject> gameObjects
            = wordPieces.Select(piece => piece.gameObject).ToList();
        _animator.PlaySequentialAppearAnimation(gameObjects);
        await UniTask.CompletedTask;
    }
}