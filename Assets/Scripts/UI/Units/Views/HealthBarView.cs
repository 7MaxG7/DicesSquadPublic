using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Units
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Image _ignoredArmorBar;
        [SerializeField] private Image _incomingDamageBar;
        [SerializeField] private Image _armorBar;
        [SerializeField] private Image _currentHpBar;

        private UIConfig _uiConfig;
        private CancellationTokenProvider _tokenProvider;

        private bool _isAnimating;

        public void Init(UIConfig uiConfig, CancellationTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
            _uiConfig = uiConfig;
        }

        public void Clear()
            => KillDoTween();

        public void UpdateBars(int barsCapacity, int ignoredArmor, int damage, int armor, int currentHp)
        {
            using var localCts = _tokenProvider.CreateLocalCts();

            if (currentHp > 0 && !gameObject.activeSelf)
                Enable();

            KillDoTween();

            var capacity = (float)barsCapacity;
            var newDamage = damage / capacity;
            var newIgnoredArmor = ignoredArmor / capacity;
            
            if (newDamage < _incomingDamageBar.fillAmount && newIgnoredArmor <= _ignoredArmorBar.fillAmount)
                _incomingDamageBar.DOFillAmount(newDamage, _uiConfig.HealthBarAnimationLength).WithCancellation(localCts.Token);
            else
                _incomingDamageBar.fillAmount = newDamage;

            _ignoredArmorBar.fillAmount = newIgnoredArmor;
            _armorBar.DOFillAmount(armor / capacity, _uiConfig.HealthBarAnimationLength).WithCancellation(localCts.Token);
            _currentHpBar.DOFillAmount(currentHp / capacity, _uiConfig.HealthBarAnimationLength).WithCancellation(localCts.Token);
        }

        public void Disable()
        {
            if (gameObject != null)
                gameObject.SetActive(false);
        }

        private void Enable()
            => gameObject.SetActive(true);

        private void KillDoTween()
        {
            _armorBar.DOKill();
            _currentHpBar.DOKill();
        }
    }
}