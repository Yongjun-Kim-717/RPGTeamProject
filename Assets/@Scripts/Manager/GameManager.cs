using NUnit.Framework;
using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action<Vector2> OnMoveDirChanged;

    private Vector2 _moveDir;
    public Vector2 MoveDir
    {
        get => _moveDir; set
        {
            _moveDir = value;
            OnMoveDirChanged?.Invoke(value);
        }
    }
}
