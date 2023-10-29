using CustomTypes.Enums.Infrastructure;
using CustomTypes.Enums.Team;
using Dices;
using Infrastructure;
using Leopotam.EcsLite;
using Zenject;

namespace Battle
{
    public class DiceApplyStateSystem : IEcsRunSystem
    {
        private readonly DebugService _debug;
        private readonly TeamService _teamService;
        private readonly DiceTargetSelectService _targetSelectService;
        private readonly BattleDeathService _battleDeathService;
        private readonly DiceApplyViewService _diceApplyViewService;
        private readonly BattleDiceLockService _lockService;
        private readonly BattleStateMachine _battleStateMachine;
        private readonly DiceApplyService _diceApplyService;

        [Inject]
        public DiceApplyStateSystem(DebugService debug, BattleStateMachine battleStateMachine, DiceApplyService diceApplyService,
            TeamService teamService, DiceTargetSelectService targetSelectService, DiceApplyViewService diceApplyViewService,
            BattleDeathService battleDeathService, BattleDiceLockService lockService)
        {
            _debug = debug;
            _teamService = teamService;
            _targetSelectService = targetSelectService;
            _battleDeathService = battleDeathService;
            _diceApplyViewService = diceApplyViewService;
            _lockService = lockService;
            _battleStateMachine = battleStateMachine;
            _diceApplyService = diceApplyService;
        }

        public void Run(IEcsSystems systems)
        {
            if (!_diceApplyService.IsApplyingDice)
                return;

            if (IsDiceApplyStateCompleted())
            {
                _targetSelectService.ClearAllDicesTarget();
                _lockService.UnlockAllDices();
                
                if (_battleDeathService.IsAnyTeamDead())
                    _battleStateMachine.Enter<EndBattleState>();
                else
                    _battleStateMachine.Enter<BattleDiceRollState>();
                
                return;
            }

            if (IsPlayerDiceApplyingComplete())
            {
                _teamService.SetCurrentTeam(TeamType.Enemy);
                _debug.Log(DebugType.Log, "Enemy's turn");
            }
        }

        private bool IsDiceApplyStateCompleted()
            => _teamService.IsCurrentTeam(TeamType.Enemy) && _diceApplyService.IsCurrentTeamDiceApplyingFinished() &&
               !_diceApplyViewService.IsInProgress;

        private bool IsPlayerDiceApplyingComplete()
            => _teamService.IsCurrentTeam(TeamType.Player) && _diceApplyService.IsCurrentTeamDiceApplyingFinished() &&
               !_diceApplyViewService.IsInProgress;
    }
}