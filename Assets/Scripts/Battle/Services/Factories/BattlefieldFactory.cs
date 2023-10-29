using Battle.Battlefield;
using CustomTypes;
using Infrastructure;
using Leopotam.EcsLite;
using Zenject;

namespace Battle
{
    public class BattlefieldFactory
    {
        private readonly BattlefieldConfig _battlefieldConfig;
        private readonly EcsService _ecsService;

        private readonly EcsPool<BattlefieldComponent> _battlefieldPool;

        [Inject]
        public BattlefieldFactory(EcsService ecsService, BattlefieldConfig battlefieldConfig)
        {
            _battlefieldConfig = battlefieldConfig;
            _ecsService = ecsService;

            _battlefieldPool = _ecsService.World.GetPool<BattlefieldComponent>();
        }

        public void CreateBattlefield()
        {
            var battlefieldEntity = _ecsService.CreateEntity();
            
            var sideSize = _battlefieldConfig.BattlefieldSize;
            var cells = new BattleCell[sideSize * sideSize];
            for (var y = 0; y < sideSize; y++)
            for (var x = 0; x < sideSize; x++)
                cells[y * sideSize + x] = new BattleCell(x, y);

            _battlefieldPool.Add(battlefieldEntity) = new BattlefieldComponent
            {
                Size = sideSize,
                BattlefieldCells = cells,
            };
        }
    }
}