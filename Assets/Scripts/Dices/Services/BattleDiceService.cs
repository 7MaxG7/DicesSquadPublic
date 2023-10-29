using Battle;
using CustomTypes;
using CustomTypes.Enums;
using CustomTypes.Enums.Infrastructure;
using Infrastructure;
using Leopotam.EcsLite;

namespace Dices
{
    public class BattleDiceService
    {
        private readonly EcsService _ecsService;
        private readonly TeamService _teamService;
        private readonly BattleLocationService _locationService;
        private readonly DebugService _debug;
        private readonly BattleDeathService _battleDeathService;

        private readonly EcsPool<DiceComponent> _dicePool;

        public BattleDiceService(EcsService ecsService, TeamService teamService, BattleLocationService locationService, DebugService debug,
            BattleDeathService battleDeathService)
        {
            _ecsService = ecsService;
            _teamService = teamService;
            _locationService = locationService;
            _debug = debug;
            _battleDeathService = battleDeathService;

            _dicePool = ecsService.World.GetPool<DiceComponent>();
        }

        public DiceSide GetCurrentSide(int dice)
        {
            ref var diceComponent = ref _dicePool.Get(dice);
            return diceComponent.CurrentSide;
        }

        public bool IsDiceUsableForTarget(int dice, int target)
        {
            var sideType = GetCurrentSide(dice).SideType;
            switch (sideType)
            {
                case DiceSideType.MeleeAttack:
                    return !_battleDeathService.IsDead(target) && TryGetUnit(dice, out var unit) &&
                           !_locationService.IsObjectBetweenUnitsExists(target, unit, IsEnemy) && unit != target;

                case DiceSideType.RangeAttack:
                    return !_battleDeathService.IsDead(target) && TryGetUnit(dice, out unit) && unit != target;

                case DiceSideType.Armor:
                    return !_battleDeathService.IsDead(target);
            }

            return false;
        }

        public bool TryGetUnit(int dice, out int unit)
        {
            ref var diceComponent = ref _dicePool.Get(dice);
            if (_ecsService.TryUnpack(diceComponent.Unit, out unit))
                return true;

            _debug.Log(DebugType.Warning, $"Cannot unpack unit for dice {dice}");
            return false;
        }

        private bool IsEnemy(int unit)
            => !_teamService.IsCurrentTeamEntity(unit);
    }
}