using Infrastructure;

namespace UI.Units
{
    public class UnitUIOverlayController
    {
        private readonly UnitUIOverlayView _uiOverlayView;
        private readonly UIConfig _uiConfig;
        private readonly CancellationTokenProvider _tokenProvider;
        
        private HealthBarController _healthBarController;

        public UnitUIOverlayController(UnitUIOverlayView uiOverlayView, UIConfig uiConfig, CancellationTokenProvider tokenProvider)
        {
            _uiOverlayView = uiOverlayView;
            _uiConfig = uiConfig;
            _tokenProvider = tokenProvider;
        }

        public void Init()
        {
            _healthBarController = new HealthBarController(new HealthBarModel(), _uiOverlayView.HealthBar, _uiConfig, _tokenProvider);
            _healthBarController.Init();
        }

        public void Clear()
        {
            _healthBarController.Clear();
        }

        public void UpdateHealth(int currentHp, int maxHp, int armor)
            => _healthBarController.UpdateHealth(currentHp, maxHp, armor);

        public void UpdateDamage(int damage)
            => _healthBarController.UpdateDamage(damage);
    }
}