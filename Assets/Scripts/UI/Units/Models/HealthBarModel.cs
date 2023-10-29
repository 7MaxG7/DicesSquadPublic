using Cysharp.Threading.Tasks;

namespace UI.Units
{
    public class HealthBarModel
    {
        public AsyncReactiveProperty<int> CurrentHp { get; } = new(default);
        public AsyncReactiveProperty<int> MaxHp { get; } = new(default);
        public AsyncReactiveProperty<int> Armor { get; } = new(default);
        public AsyncReactiveProperty<int> IncomingDamage { get; } = new(default);
        public bool IsArmorIgnored;
    }
}