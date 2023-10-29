using System;
using System.Collections.Generic;
using Abstractions.Battle;
using CustomTypes.Enums.Infrastructure;
using Infrastructure;
using Leopotam.EcsLite;
using Zenject;

namespace Battle
{
    public class BattleStateMachine
    {
        private readonly DebugService _debugService;

        private readonly EcsFilter _battleFilter;

        private readonly Dictionary<Type, IBattleState> _states;
        private IBattleState _currentState;

        [Inject]
        public BattleStateMachine(BattleDiceRollState battleDiceRollState, TargetSelectState targetSelectState,
            DicesApplyState dicesApplyState, EndBattleState endBattleState, DebugService debugService)
        {
            _debugService = debugService;
            _states = new Dictionary<Type, IBattleState>
            {
                [typeof(BattleDiceRollState)] = battleDiceRollState,
                [typeof(TargetSelectState)] = targetSelectState,
                [typeof(DicesApplyState)] = dicesApplyState,
                [typeof(EndBattleState)] = endBattleState,
            };
        }

        public void Enter<TState>() where TState : IBattleState
        {
            _currentState?.Exit();
            if (!_states.TryGetValue(typeof(TState), out var newState))
            {
                _debugService.Log(DebugType.Error, $"No state {typeof(TState)} in state machine");
                return;
            }

            _currentState = newState;
            _currentState.Enter();
        }
    }
}