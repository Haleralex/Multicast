using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ILevelAnimationService
{
    UniTask PlayLevelStartAnimation(List<WordPiece> wordPieces);
}