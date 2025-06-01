using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
namespace Core.Interfaces
{
    public interface IAsyncFactory<T> : IDisposable
    {
        UniTask Initialize();
        T Create();
    }
}