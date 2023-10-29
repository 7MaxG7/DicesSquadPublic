using Battle;
using Battle.Battlefield;
using CustomTypes;
using CustomTypes.Enums.Team;
using Dices;
using Infrastructure;
using Leopotam.EcsLite;
using UnityEngine;

namespace Units.Factories
{
    public class UnitFactory
    {
        private readonly EcsService _ecsService;
        private readonly StaticDataService _dataService;
        private readonly DiceFactory _diceFactory;

        private readonly EcsPool<UnitComponent> _unitPool;
        private readonly EcsPool<BattleLocationComponent> _battleLocationPool;
        private readonly EcsPool<TeamComponent> _teamPool;
        private readonly EcsPool<HealthComponent> _healthPool;
        
        private Transform _unitsParent;

        public UnitFactory(EcsService ecsService, StaticDataService dataService, DiceFactory diceFactory)
        {
            _ecsService = ecsService;
            _dataService = dataService;
            _diceFactory = diceFactory;

            _unitPool = _ecsService.World.GetPool<UnitComponent>();
            _battleLocationPool = _ecsService.World.GetPool<BattleLocationComponent>();
            _teamPool = _ecsService.World.GetPool<TeamComponent>();
            _healthPool = _ecsService.World.GetPool<HealthComponent>();
        }

        public int CreateUnit(UnitSpecialization specialization, TeamType team)
        {
            var unit = _ecsService.CreateEntity();

            ref var unitComponent = ref _unitPool.Add(unit);
            ref var healthComponent = ref _healthPool.Add(unit);
            ref var teamComponent = ref _teamPool.Add(unit);
            _battleLocationPool.Add(unit);

            var config = _dataService.GetUnit(specialization.Id); 
            
            unitComponent.Specialization = specialization;
            unitComponent.MainDice = _ecsService.World.PackEntity(CreateMainDice(unit, team, config));
            
            teamComponent.Team = team;

            healthComponent.MaxHp = config.Hp;
            healthComponent.Hp = config.Hp;
            
            return unit;
        }

        private int CreateMainDice(int unit, TeamType team, UnitConfig config)
        {
            var mainDice = _diceFactory.CreateMainDice(unit, config.Dice);
            
            ref var teamComponent = ref _teamPool.Add(mainDice);
            teamComponent.Team = team;
            
            return mainDice;
        }
    }
}