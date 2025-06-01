using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IServiceBootstrapper
{
    UniTask InitializeServicesAsync();
}