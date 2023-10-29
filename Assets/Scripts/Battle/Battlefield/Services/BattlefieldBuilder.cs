using Battle.Temp;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Leopotam.EcsLite;
using Units;
using Utils.Extensions;
using Zenject;

namespace Battle.Battlefield
{
    public class BattlefieldBuilder
    {
        private readonly EcsService _ecsService;
        private readonly BattlefieldViewFactory _battlefieldViewFactory;
        private readonly UnitSpawner _unitSpawner;
        private readonly TeamsInitializer _teamsInitializer;
        private readonly BattlefieldFactory _battlefieldFactory;

        private readonly EcsFilter _unitFilter;
        private readonly EcsFilter _battlefieldFilter;

        [Inject]
        public BattlefieldBuilder(EcsService ecsService, BattlefieldViewFactory battlefieldViewFactory, UnitSpawner unitSpawner,
            TeamsInitializer teamsInitializer, BattlefieldFactory battlefieldFactory)
        {
            _ecsService = ecsService;
            _battlefieldViewFactory = battlefieldViewFactory;
            _unitSpawner = unitSpawner;
            _teamsInitializer = teamsInitializer;
            _battlefieldFactory = battlefieldFactory;

            _unitFilter = ecsService.World.Filter<UnitComponent>().End();
            _battlefieldFilter = ecsService.World.Filter<BattlefieldComponent>().End();
        }

        public async UniTask BuildBattlefieldAsync()
        {
            _battlefieldFactory.CreateBattlefield();
            await _battlefieldViewFactory.CreateBattlefieldViewAsync();
            _teamsInitializer.InitializeTeams();
            foreach (var unit in _unitFilter)
                await _unitSpawner.SpawnUnitAsync(unit);
        }

        public void Clear()
        {
            if (_ecsService.IsInited())
                _ecsService.DestroyEntity(_battlefieldFilter.GetSingle());
            
            foreach (var unit in _unitFilter)
                _unitSpawner.DespawnUnit(unit);
        }
    }
}