using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int _mapWidth;
    [SerializeField]
    private int _mapHeight;
    [SerializeField]
    private float _noiseScale;

    [SerializeField]
    public bool _autoUpdate = false;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(_mapWidth, _mapHeight, _noiseScale);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }
}
