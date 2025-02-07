using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsGameActive { get; private set; } = false;
    [SerializeField] private GameObject _hitPoint;
    [SerializeField] private List<CubeController> _aliveCubes;
    [SerializeField] private List<CubeController> _deadCubes;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsGameActive = !IsGameActive;
        }
    }

    public void SpawnHitPoint()
    {
        Instantiate(_hitPoint);
    }

    public void KillCube(CubeController cube)
    {
        cube.gameObject.SetActive(false);
        _aliveCubes.Remove(cube);
        _deadCubes.Add(cube);
    }
}
