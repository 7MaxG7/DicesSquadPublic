using Infrastructure;
using Leopotam.EcsLite;

namespace Dices
{
    public class DiceService
    {
        private readonly EcsService _ecsService;

        private readonly EcsPool<DiceComponent> _dicePool;

        public DiceService(EcsService ecsService)
        {
            _ecsService = ecsService;

            _dicePool = ecsService.World.GetPool<DiceComponent>();
        }

        public void InitComponents(int unit, DiceConfig config, int dice)
        {
            ref var diceComponent = ref _dicePool.Add(dice);
            diceComponent.CurrentSide = config.Sides[0];
            diceComponent.Unit = _ecsService.World.PackEntity(unit);
            diceComponent.Config = config;
        }
    }
}