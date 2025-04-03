using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScreen : MonoBehaviour
{
    void Start()
    {
        AsyncSceneLoader.LoadGameplayScene();
    }
}
