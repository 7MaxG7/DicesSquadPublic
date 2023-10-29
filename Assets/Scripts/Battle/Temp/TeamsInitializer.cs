using Battle.Battlefield;
using CustomTypes;
using CustomTypes.Enums.Team;
using CustomTypes.Enums.Units;
using Infrastructure;
using Leopotam.EcsLite;
using Units.Factories;
using UnityEngine;
using Zenject;

namespace Battle.Temp
{
    public class TeamsInitializer
    {
        private readonly EcsService _ecsService;
        private readonly UnitFactory _unitFactory;
        private readonly BattleCellService _cellService;
        private readonly DiceRollsConfig _rollsConfig;

        private readonly EcsPool<BattleLocationComponent> _battleLocationPool;
        private readonly EcsPool<TeamComponent> _teamPool;
        private readonly EcsPool<TeamBattleDicesRollComponent> _teamBattleDicesRollPool;

        [Inject]
        public TeamsInitializer(EcsService ecsService, UnitFactory unitFactory, BattleCellService cellService, DiceRollsConfig rollsConfig)
        {
            _ecsService = ecsService;
            _unitFactory = unitFactory;
            _cellService = cellService;
            _rollsConfig = rollsConfig;

            _teamBattleDicesRollPool = ecsService.World.GetPool<TeamBattleDicesRollComponent>();
            _teamPool = ecsService.World.GetPool<TeamComponent>();
            _battleLocationPool = ecsService.World.GetPool<BattleLocationComponent>();
        }

        public void InitializeTeams()
        {
            CreateTeamRolls(TeamType.Player);
            CreateTeamRolls(TeamType.Enemy);

            CreateUnits();
        }

        private void CreateTeamRolls(TeamType team)
        {
            var playerTeamRolls = _ecsService.CreateEntity();
            ref var playerTeamComponent = ref _teamPool.Add(playerTeamRolls);
            ref var playerTeamBattleDicesRollComponent = ref _teamBattleDicesRollPool.Add(playerTeamRolls);

            playerTeamComponent.Team = team;
            playerTeamBattleDicesRollComponent.StartRollsCount =
                team == TeamType.Player ? _rollsConfig.PlayerDefaultRollsCount : _rollsConfig.EnemyDefaultRollsCount;
        }

        private void CreateUnits()
        {
            // TODO. Setup must be taken from config
            var cells = _cellService.GetCells();
            foreach (var cell in cells)
            {
                if (cell.Location.x is not 1 and not 2)
                    continue;

                var team = cell.Location.y < 2 ? TeamType.Player : TeamType.Enemy;

                var unit = _unitFactory.CreateUnit(new UnitSpecialization(UnitClass.None, UnitArchetype.None, 1), team);

                ref var battleLocationComponent = ref _battleLocationPool.Get(unit);
                battleLocationComponent.Cell = cell;
                battleLocationComponent.Rotation = Quaternion.Euler(team == TeamType.Player ? Vector3.zero : new Vector3(0, 180, 0));
                cell.Occupier = _ecsService.World.PackEntity(unit);
            }
        }
    }
}