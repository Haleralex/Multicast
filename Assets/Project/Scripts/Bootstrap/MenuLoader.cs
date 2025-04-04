using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MenuLoader
{
    [Inject] private readonly ISceneLoader asyncSceneLoader;
    public void LoadMenu()
    {
        asyncSceneLoader.LoadMenuScene();
    }
}
