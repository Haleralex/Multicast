using Cysharp.Threading.Tasks;
using Progress;
using UnityEngine;

public interface IProgressRestoreService
{
    UniTask RestoreProgress(GameProgress progress);
}
