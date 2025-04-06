using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public interface ISceneLoader
    {
        void LoadGameplayScene();
        void LoadMenuScene();
    }
}
