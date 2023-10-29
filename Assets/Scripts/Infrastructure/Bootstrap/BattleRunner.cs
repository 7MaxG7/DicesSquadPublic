using System.Threading;
using Battle;
using Battle.Battlefield;
using Cysharp.Threading.Tasks;
using UI;
using UI.Battle;
using Units;
using Units.Factories;
using Zenject;

namespace Infrastructure.Bootstrap
{
    public class BattleRunner : SceneRunner
    {
        private readonly BattlefieldBuilder _battlefieldBuilder;
        private readonly UnitViewFactory _unitViewFactory;
        private readonly BattleUIBuilder _battleUIBuilder;
        private readonly BattleAnimatorService _animatorService;
        private readonly CurtainUIController _curtainUIController;
        private readonly BattleUpdateSystemsInitializer _updateSystemsInitializer;
        private readonly BattleFixedUpdateSystemsInitializer _fixedUpdateSystemsInitializer;
        private readonly BattleStateMachine _battleStateMachine;
        private readonly BattleSelectionService _battleSelectionService;

        [Inject]
        public BattleRunner(GameBootstrapper gameBootstrapper, EcsService ecsService, BattleSelectionService battleSelectionService,
            BattleUpdateSystemsInitializer updateSystemsInitializer, BattleFixedUpdateSystemsInitializer fixedUpdateSystemsInitializer,
            BattleStateMachine battleStateMachine, AssetsProvider assetsProvider, BattlefieldBuilder battlefieldBuilder,
            UnitViewFactory unitViewFactory, BattleUIBuilder battleUIBuilder, BattleAnimatorService animatorService,
            CurtainUIController curtainUIController, CancellationTokenProvider tokenProvider) : base(gameBootstrapper, ecsService,
            assetsProvider, tokenProvider)
        {
            _battlefieldBuilder = battlefieldBuilder;
            _unitViewFactory = unitViewFactory;
            _battleUIBuilder = battleUIBuilder;
            _animatorService = animatorService;
            _curtainUIController = curtainUIController;
            UpdateSystemsInitializer = updateSystemsInitializer;
            FixedUpdateSystemsInitializer = fixedUpdateSystemsInitializer;
            _battleStateMachine = battleStateMachine;
            _battleSelectionService = battleSelectionService;
        }

        protected override async UniTask OnInitAsync(CancellationTokenSource cts)
        {
            _unitViewFactory.Init();

            await _battlefieldBuilder.BuildBattlefieldAsync();
            await _battleUIBuilder.BuildBattleUIAsync();
            _battleSelectionService.Init();

            _battleStateMachine.Enter<BattleDiceRollState>();
            await _curtainUIController.Hide(cts);
        }

        protected override void OnDispose()
        {
            _animatorService.Clear();
            _battleSelectionService.Clear();
            _battleUIBuilder.ClearBattleUI();
            _battlefieldBuilder.Clear();
        }
    }
}