using System.Collections.Generic;
using System.Linq;
using CustomTypes;
using CustomTypes.Enums.Battle;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Battle.Battlefield
{
    public class BattlefieldView : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private Tilemap _tilemap;

        public Grid Grid => _grid;
        
        private Dictionary<TileState,TileBase> _stateTiles;

        public void Init(IEnumerable<StateTile> stateTiles)
            => _stateTiles = stateTiles.ToDictionary(data => data.State, data => data.Tile);

        public void SetTile(Vector3Int cellPosition, TileState tileState)
        {
            if (!_stateTiles.TryGetValue(tileState, out var tile))
            {
                Debug.LogWarning($"No tile for state {tileState}");
                return;
            }
            
            _tilemap.SetTile(cellPosition, tile);
        }
    }
}