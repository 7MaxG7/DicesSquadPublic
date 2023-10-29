using Abstractions.UI.Battle;
using CustomTypes.Enums.Team;
using Cysharp.Threading.Tasks.Linq;
using Infrastructure;
using Zenject;

namespace UI.Battle
{
    public class BattleEndUIController
    {
        private readonly UiAnimationService _uiAnimationService;
        private readonly CancellationTokenProvider _tokenProvider;
        private readonly UIConfig _uiConfig;
        
        private BattleEndUIView _battleEndUIView;
        private IBattleEndUIModel _battleEndUIModel;

        [Inject]
        public BattleEndUIController(UiAnimationService uiAnimationService, CancellationTokenProvider tokenProvider, UIConfig uiConfig)
        {
            _uiAnimationService = uiAnimationService;
            _uiConfig = uiConfig;
            _tokenProvider = tokenProvider;
        }

        public void Init(IBattleEndUIModel battleEndUIModel, BattleEndUIView battleEndUIView)
        {
            _battleEndUIModel = battleEndUIModel;
            _battleEndUIView = battleEndUIView;
            
            _battleEndUIView.Init(_uiAnimationService, _uiConfig, _tokenProvider);
            _battleEndUIModel.Winner.Subscribe(_battleEndUIView.SetWinnerLabel);
            
            _battleEndUIView.gameObject.SetActive(false);
        }

        public void ShowBattleEndLabel(TeamType winner)
        {
            _battleEndUIModel.Winner.Value = winner;
            _battleEndUIView.ShowAsync().Forget();
        }
    }
}