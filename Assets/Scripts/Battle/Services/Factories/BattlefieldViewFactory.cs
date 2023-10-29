using Battle.Battlefield;
using CustomTypes.Enums.Battle;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Leopotam.EcsLite;
using UnityEngine;
using Utils.Extensions;

namespace Battle
{
    public class BattlefieldViewFactory
    {
        private readonly AssetsProvider _assetsProvider;
        private readonly BattlefieldViewConfig _battlefieldViewConfig;
        private readonly BattlefieldConfig _battlefieldConfig;
        private readonly BattleCellService _cellService;

        private readonly EcsFilter _battlefieldFilter;
        private readonly EcsPool<BattlefieldViewComponent> _battlefieldViewPool;

        public BattlefieldViewFactory(EcsService ecsService, AssetsProvider assetsProvider, BattlefieldViewConfig battlefieldViewConfig,
            BattlefieldConfig battlefieldConfig, BattleCellService cellService)
        {
            _assetsProvider = assetsProvider;
            _battlefieldViewConfig = battlefieldViewConfig;
            _battlefieldConfig = battlefieldConfig;
            _cellService = cellService;

            _battlefieldFilter = ecsService.World.Filter<BattlefieldComponent>().End();
            _battlefieldViewPool = ecsService.World.GetPool<BattlefieldViewComponent>();
        }
        
        public async UniTask CreateBattlefieldViewAsync()
        {
            var battlefield = _battlefieldFilter.GetSingle();
            
            var battlefieldView = await _assetsProvider.CreateInstanceAsync<BattlefieldView>(_battlefieldViewConfig.BattlefieldPref);
            battlefieldView.Init(_battlefieldViewConfig.StateTiles);

            var cellSize = battlefieldView.Grid.cellSize;
            var sideSize = _battlefieldConfig.BattlefieldSize;
            battlefieldView.Grid.transform.position =
                -new Vector3(cellSize.x, 0, cellSize.y) * sideSize * .5f;

            foreach (var cell in _cellService.GetCells(battlefield))
                battlefieldView.SetTile(cell.Location, TileState.Inactive);

            _battlefieldViewPool.Add(battlefield) = new BattlefieldViewComponent
            {
                BattlefieldView = battlefieldView,
            };
        }
    }
}