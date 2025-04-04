using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelView
{
    public event Action<GameProgress> GameFieldChanged;
    public event Action NextLevelPressed;
    public event Action ValidateLevelPressed;
}
