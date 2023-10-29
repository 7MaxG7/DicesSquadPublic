using Abstractions.Battle;
using CustomTypes.Enums.Infrastructure;
using Dices;
using Infrastructure;
using Zenject;

namespace Battle
{
    public class DicesApplyState : IBattleState
    {
        private readonly DiceApplyService _diceApplyService;
        private readonly DebugService _debug;

        [Inject]
        public DicesApplyState(DiceApplyService diceApplyService, DebugService debug)
        {
            _diceApplyService = diceApplyService;
            _debug = debug;
        }
        
        public void Enter()
        {
            _debug.Log(DebugType.Log, $"{nameof(DicesApplyState)} started");
            _diceApplyService.StartDiceApplying();
        }

        public void Exit()
            => _diceApplyService.FinishDiceApplying();
    }
}