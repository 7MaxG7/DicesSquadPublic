using Battle;
using CustomTypes;
using CustomTypes.Enums.Team;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Leopotam.EcsLite;
using UI.Units;
using Units.Views;
using UnityEngine;
using Zenject;

namespace Units.Factories
{
    public class UnitViewFactory
    {
        private readonly EcsService _ecsService;
        private readonly AssetsProvider _assetsProvider;
        private readonly StaticDataService _dataService;
        private readonly HighlightService _highlightService;
        private readonly UnitUIOverlayService _uiOverlayService;
        private readonly BattleAnimatorService _battleAnimatorService;

        private readonly EcsPool<UnitViewComponent> _unitViewPool;

        private Transform _unitsParent;

        [Inject]
        public UnitViewFactory(EcsService ecsService, AssetsProvider assetsProvider, StaticDataService dataService,
            HighlightService highlightService, UnitUIOverlayService uiOverlayService, BattleAnimatorService battleAnimatorService)
        {
            _ecsService = ecsService;
            _assetsProvider = assetsProvider;
            _dataService = dataService;
            _highlightService = highlightService;
            _uiOverlayService = uiOverlayService;
            _battleAnimatorService = battleAnimatorService;

            _unitViewPool = ecsService.World.GetPool<UnitViewComponent>();
        }

        public void Init()
        {
            if (_unitsParent == null)
                _unitsParent = new GameObject(Constants.UNITS_PARENT_NAME).transform;
        }

        public async UniTask<UnitView> SpawnUnitAsync(int unit, UnitSpecialization specialization, Vector3 position, Quaternion rotation,
            TeamType team)
        {
            var prefab = _dataService.GetUnit(specialization.Id).Prefab;
            var unitView = await _assetsProvider.CreateInstanceAsync<UnitView>(prefab, position, rotation, _unitsParent);

            InitComponents(unit, unitView );

            unitView.SelectView.Init(_ecsService.World.PackEntity(unit));
            unitView.Renderer.material.color = team == TeamType.Player ? Color.cyan : Color.red;

            return unitView;
        }

        private void InitComponents(int unit, UnitView unitView)
        {
            ref var unitViewComponent = ref _unitViewPool.Add(unit);
            unitViewComponent.View = unitView;

            var animator = unitView.Animator;
            _battleAnimatorService.InitComponents(unit, animator);

            _ecsService.AddEntityDebugView(unitView.gameObject, unit);
            _highlightService.InitComponents(unit, unitView.Highlight);
        }
    }
}