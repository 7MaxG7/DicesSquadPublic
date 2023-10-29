using System;
using Cysharp.Threading.Tasks.Linq;
using Infrastructure;
using Utils.Extensions;

namespace UI.Units
{
    public class HealthBarController
    {
        private readonly HealthBarModel _healthBarModel;
        private readonly HealthBarView _healthBarView;
        private readonly UIConfig _uiConfig;
        private readonly CancellationTokenProvider _tokenProvider;

        public HealthBarController(HealthBarModel healthBarModel, HealthBarView healthBarView, UIConfig uiConfig,
            CancellationTokenProvider tokenProvider)
        {
            _healthBarModel = healthBarModel;
            _healthBarView = healthBarView;
            _uiConfig = uiConfig;
            _tokenProvider = tokenProvider;
        }

        public void Init()
        {
            _healthBarView.Init(_uiConfig, _tokenProvider);
            
            _healthBarModel.CurrentHp.Subscribe(SetCurrentHp);
            _healthBarModel.MaxHp.Subscribe(SetMaxHp);
            _healthBarModel.Armor.Subscribe(SetArmor);
            _healthBarModel.IncomingDamage.Subscribe(SetIncomingDamage);
        }

        public void Clear()
        {
            _healthBarView.Clear();
        }

        public void UpdateHealth(int currentHp, int maxHp, int armor)
        {
            _healthBarModel.CurrentHp.Update(currentHp);
            if (currentHp <= 0)
                return;

            _healthBarModel.MaxHp.Update(maxHp);
            _healthBarModel.Armor.Update(armor);
        }

        public void UpdateDamage(int damage)
            => _healthBarModel.IncomingDamage.Update(damage);

        private void SetCurrentHp(int hp)
        {
            if (hp <= 0)
                _healthBarView.Disable();
            else
                CalculateBars(hp, _healthBarModel.Armor, _healthBarModel.IncomingDamage, _healthBarModel.MaxHp,
                    _healthBarModel.IsArmorIgnored);
        }

        private void SetMaxHp(int maxHp)
            => CalculateBars(_healthBarModel.CurrentHp, _healthBarModel.Armor, _healthBarModel.IncomingDamage, maxHp,
                _healthBarModel.IsArmorIgnored);

        private void SetArmor(int armor)
            => CalculateBars(_healthBarModel.CurrentHp, armor, _healthBarModel.IncomingDamage, _healthBarModel.MaxHp,
                _healthBarModel.IsArmorIgnored);

        private void SetIncomingDamage(int damage)
            => CalculateBars(_healthBarModel.CurrentHp, _healthBarModel.Armor, damage, _healthBarModel.MaxHp,
                _healthBarModel.IsArmorIgnored);

        private void CalculateBars(int currentHp, int armor, int damage, int maxHp, bool isArmorIgnored)
        {
            if (currentHp <= 0)
                return;
            
            var barsCapacity = Math.Max(maxHp, currentHp + armor);
            var ignoredArmorBar = isArmorIgnored ? currentHp + armor : 0;
            var damageBar = damage == 0
                ? 0
                : isArmorIgnored
                    ? currentHp
                    : currentHp + armor;
            var armorBar = isArmorIgnored ? 0 : currentHp + armor - damage;
            var currentHpBar = Math.Min(currentHp - damage + (isArmorIgnored ? 0 : armor), currentHp);

            _healthBarView.UpdateBars(barsCapacity, ignoredArmorBar, damageBar, armorBar, currentHpBar);
        }
    }
}