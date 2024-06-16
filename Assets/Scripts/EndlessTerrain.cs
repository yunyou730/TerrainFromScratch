using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const float kMaxViewDst = 450.0f;
    public Transform _viewer;

    public static Vector2 _viewerPosition;
    
    private int _chunkSize;
    private int _chunksVisibleInViewDst;


    private Dictionary<Vector2, TerrainChunk> _terrainChunkDict = new Dictionary<Vector2, TerrainChunk>();
    private List<TerrainChunk> _terrainChunksVisibleLastFrame = new List<TerrainChunk>();

    void Start()
    {
        _chunkSize = MapGenerator.kMapChunkSize - 1;
        _chunksVisibleInViewDst = Mathf.RoundToInt(kMaxViewDst/_chunkSize);
    }

    private void Update()
    {
        _viewerPosition = new Vector2(_viewer.position.x,_viewer.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        for (int i = 0;i < _terrainChunksVisibleLastFrame.Count;i++)
        {
            _terrainChunksVisibleLastFrame[i].SetVisible(false);
        }
        _terrainChunksVisibleLastFrame.Clear();


        int currentChunkCoordX = Mathf.RoundToInt(_viewerPosition.x / _chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(_viewerPosition.y / _chunkSize);

        for (int yOffset = -_chunksVisibleInViewDst; yOffset <= _chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -_chunksVisibleInViewDst; xOffset <= _chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewChunkCoord = new Vector2(currentChunkCoordX + xOffset,currentChunkCoordY + yOffset);
                if (_terrainChunkDict.ContainsKey(viewChunkCoord))
                {
                    _terrainChunkDict[viewChunkCoord].UpdateTerrainChunk();
                    if (_terrainChunkDict[viewChunkCoord].IsVisible())
                    {
                        _terrainChunksVisibleLastFrame.Add(_terrainChunkDict[viewChunkCoord]);
                    }
                }
                else
                {
                    _terrainChunkDict.Add(viewChunkCoord,new TerrainChunk(viewChunkCoord,_chunkSize,transform));
                }

            }
        }
    }


    public class TerrainChunk
    {
        private GameObject _meshObject;
        private Vector2 _position;
        private Bounds _bounds;
        
        public TerrainChunk(Vector2 coord,int size,Transform parent)
        {
            _position = coord * size;
            _bounds = new Bounds(_position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(_position.x,0,_position.y);

            _meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            _meshObject.transform.position = positionV3;
            _meshObject.transform.localScale = Vector3.one * size / 10.0f;
            _meshObject.transform.parent = parent;
            SetVisible(false);
        }

        public void UpdateTerrainChunk()
        {
            float viewDstFromNearestEdge = _bounds.SqrDistance(_viewerPosition);
            bool visible = viewDstFromNearestEdge <= kMaxViewDst;
            SetVisible(visible);
        }

        public void SetVisible(bool bVisible)
        {
            _meshObject.SetActive(bVisible);
        }

        public bool IsVisible()
        {
            return _meshObject.activeSelf;
        }

    }

}
