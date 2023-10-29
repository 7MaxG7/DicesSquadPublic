using Abstractions.Battle;
using CustomTypes.Enums.Infrastructure;
using Dices;
using Infrastructure;

namespace Battle
{
    public class TargetSelectState : IBattleState
    {
        private readonly DiceTargetSelectService _targetSelectService;
        private readonly DebugService _debug;

        public TargetSelectState(DiceTargetSelectService targetSelectService, DebugService debug)
        {
            _targetSelectService = targetSelectService;
            _debug = debug;
        }
        
        public void Enter()
        {
            _debug.Log(DebugType.Log, $"{nameof(TargetSelectState)} started");
            _targetSelectService.StartTargetSelection();
        }

        public void Exit()
        {
            _targetSelectService.FinishTargetSelection();
        }
    }
}