using CustomTypes.Enums.Team;
using Cysharp.Threading.Tasks;
using Infrastructure;
using TMPro;
using UnityEngine;

namespace UI.Battle
{
    public class BattleEndUIView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _lable;

        private UiAnimationService _uiAnimationService;
        private UIConfig _uiConfig;
        private CancellationTokenProvider _tokenProvider;

        public void Init(UiAnimationService uiAnimationService, UIConfig uiConfig, CancellationTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
            _uiAnimationService = uiAnimationService;
            _uiConfig = uiConfig;
        }

        public async UniTaskVoid ShowAsync()
        {
            using var localCts = _tokenProvider.CreateLocalCts();
            await _uiAnimationService.ToggleCanvasGroupVisibilityAsync(_canvasGroup, true, _uiConfig.DefaultAnimationDuration,
                localCts);
        }

        public void SetWinnerLabel(TeamType winner)
            => _lable.text = winner == TeamType.Player ? Constants.WIN_END_BATTLE_LABLE : Constants.DEFEAT_END_BATTLE_LABLE;
    }
}