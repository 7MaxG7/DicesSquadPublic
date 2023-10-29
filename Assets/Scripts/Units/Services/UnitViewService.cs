using Battle;
using CustomTypes.Enums.Infrastructure;
using Dices;
using Infrastructure;
using Leopotam.EcsLite;
using Zenject;

namespace Units
{
    public class UnitViewService
    {
        private readonly EcsService _ecsService;
        private readonly DebugService _debug;
        private readonly UnitService _unitService;
        private readonly DiceViewService _diceViewService;
        private readonly HighlightService _highlightService;

        private readonly EcsPool<UnitComponent> _unitPool;

        [Inject]
        public UnitViewService(EcsService ecsService, HighlightService highlightService, BattleAnimatorService battleAnimatorService,
            DebugService debug, UnitService unitService, DiceViewService diceViewService)
        {
            _ecsService = ecsService;
            _debug = debug;
            _unitService = unitService;
            _diceViewService = diceViewService;
            _highlightService = highlightService;

            _unitPool = ecsService.World.GetPool<UnitComponent>();
        }

        public void ToggleUnitDiceHighlight(int unit, bool mustHighlighted)
        {
            ref var unitComponent = ref _unitPool.Get(unit);
            if (!_ecsService.TryUnpack(unitComponent.MainDice, out var dice))
            {
                _debug.Log(DebugType.Warning, $"Cannot unpack dice for unit {unit}");
                return;
            }

            _highlightService.ToggleHighlight(dice, mustHighlighted);
        }

        public void Die(int unit)
        {
            if (_unitService.TryGetMainDice(unit, out var dice))
                _diceViewService.Die(dice);
        }
    }
}