using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI.Permanent
{
    public class CurtainUIView : MonoBehaviour
    {
		[SerializeField] private CanvasGroup _canvasGroup;
		
		private UiAnimationService _uiAnimationService;
		private UIConfig _uiConfig;

		public virtual void Init(UiAnimationService uiAnimationService, UIConfig uiConfig)
		{
			_uiConfig = uiConfig;
			_uiAnimationService = uiAnimationService;

			_canvasGroup.alpha = 0;
			gameObject.SetActive(false);
		}

		public virtual void Clear()
		{
			_canvasGroup.DOKill();
		}

		public async UniTask ToggleActiveAsync(bool isActive, CancellationTokenSource cts)
			=> await _uiAnimationService.ToggleCanvasGroupVisibilityAsync(_canvasGroup, isActive, _uiConfig.DefaultAnimationDuration, cts);

		public void ShowInstantly()
		{
			_canvasGroup.alpha = 1f;
			gameObject.SetActive(true);
		}
    }
}