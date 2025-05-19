using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
namespace Core.Interfaces
{
    public interface IWordPiecesFactory : IAsyncFactory<WordPiece>
    {

    }   
}