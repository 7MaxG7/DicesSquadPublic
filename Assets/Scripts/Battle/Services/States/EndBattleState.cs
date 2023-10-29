using Abstractions.Battle;
using CustomTypes.Enums.Infrastructure;
using Infrastructure;
using UI.Battle;
using Zenject;

namespace Battle
{
    public class EndBattleState : IBattleState
    {
        private readonly DebugService _debug;
        private readonly BattleDeathService _battleDeathService;
        private readonly BattleUIController _battleUIController;

        [Inject]
        public EndBattleState(DebugService debug, BattleDeathService battleDeathService, BattleUIController battleUIController)
        {
            _debug = debug;
            _battleDeathService = battleDeathService;
            _battleUIController = battleUIController;
        }

        public void Enter()
        {
            _debug.Log(DebugType.Log, $"{nameof(EndBattleState)} started");

            _battleDeathService.TryGetAnyAliveTeam(out var winner);
            _battleUIController.ShowBattleEndLabel(winner);
        }

        public void Exit()
        {
        }
    }
}