using CustomTypes;
using CustomTypes.Enums.Battle;
using Infrastructure;
using Leopotam.EcsLite;
using UnityEngine;
using Utils.Extensions;

namespace Battle.Battlefield
{
    public class BattlefieldViewService
    {
        private readonly EcsFilter _battlefieldViewFilter;
        private readonly EcsPool<BattlefieldViewComponent> _battlefieldViewPool;

        public BattlefieldViewService(EcsService ecsService)
        {
            _battlefieldViewFilter = ecsService.World.Filter<BattlefieldViewComponent>().End();
            _battlefieldViewPool = ecsService.World.GetPool<BattlefieldViewComponent>();
        }

        public void SetTile(Vector3Int position, TileState state)
        {
            var battlefieldViewEntity = _battlefieldViewFilter.GetSingle();
            ref var battlefieldViewComponent = ref _battlefieldViewPool.Get(battlefieldViewEntity);
            battlefieldViewComponent.BattlefieldView.SetTile(position, state);
        }

        public Vector3 GetTilePosition(BattleCell cell)
        {
            var battlefieldViewEntity = _battlefieldViewFilter.GetSingle();
            ref var battlefieldViewComponent = ref _battlefieldViewPool.Get(battlefieldViewEntity);
            return battlefieldViewComponent.BattlefieldView.Grid.GetCellCenterWorld(cell.Location);
        }
    }
}