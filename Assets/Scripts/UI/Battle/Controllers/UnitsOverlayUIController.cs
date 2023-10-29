using System.Collections.Generic;
using System.Linq;
using Battle;
using CustomTypes.Enums.Team;
using Cysharp.Threading.Tasks;
using Infrastructure;
using UI.Units;
using UnityEngine;
using Utils.Extensions;
using Zenject;

namespace UI.Battle
{
    public class UnitsOverlayUIController
    {
        private readonly UnitUIOverlayService _unitUIOverlayService;
        private readonly BattleUIFactory _battleUIFactory;
        private readonly UIConfig _uiConfig;
        private readonly CancellationTokenProvider _tokenProvider;
        private readonly HealthService _healthService;

        private BattleUnitsOverlayUIView _unitsOverlayUIView;
        private readonly Dictionary<int, UnitUIOverlayController> _unitUIOverlayControllers = new();

        [Inject]
        public UnitsOverlayUIController(UnitUIOverlayService unitUIOverlayService, BattleUIFactory battleUIFactory, UIConfig uiConfig,
            CancellationTokenProvider tokenProvider, HealthService healthService)
        {
            _unitUIOverlayService = unitUIOverlayService;
            _battleUIFactory = battleUIFactory;
            _uiConfig = uiConfig;
            _tokenProvider = tokenProvider;
            _healthService = healthService;
        }

        public void Init(BattleUnitsOverlayUIView unitsOverlayUIView)
        {
            _unitsOverlayUIView = unitsOverlayUIView;
        }

        public void Clear()
        {
            foreach (var controller in _unitUIOverlayControllers.Values)
                controller.Clear();
        }

        public async UniTask CreateUnitsOverlayAsync(Dictionary<TeamType, List<int>> units)
        {
            foreach (var unit in units.SelectMany(teamUnits => teamUnits.Value))
                await CreateUIOverlayAsync(unit);
        }

        public void UpdateHealthBar(int unit)
            => _unitUIOverlayControllers[unit].UpdateHealth(_healthService.GetCurrentHp(unit).Ceiling(), _healthService.GetMaxHp(unit),
                _healthService.GetArmor(unit));

        public void UpdateIncomingDamage(int unit, int damage)
            => _unitUIOverlayControllers[unit].UpdateDamage(damage);

        private async UniTask CreateUIOverlayAsync(int unit)
        {
            var uiOverlayAnchor = _unitUIOverlayService.GetOverlayAnchor(unit);
            var position = Camera.main!.WorldToScreenPoint(uiOverlayAnchor.position);

            var uiOverlayView = await _battleUIFactory.CreateUnitOverlayViewAsync(position, _unitsOverlayUIView.OverlaysContent);
            var unitUIOverlayController = new UnitUIOverlayController(uiOverlayView, _uiConfig, _tokenProvider);
            unitUIOverlayController.Init();
            _unitUIOverlayControllers.Add(unit, unitUIOverlayController);
            
            _unitUIOverlayService.InitComponents(unit, uiOverlayView);

            UpdateHealthBar(unit);
            UpdateIncomingDamage(unit, 0);
        }
    }
}