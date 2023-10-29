using CustomTypes;
using Dices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Units
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(UnitConfig), fileName = nameof(UnitConfig), order = 0)]
    public class UnitConfig : ScriptableObject
    {
        [SerializeField] private bool _isEnabled;
        [SerializeField] private string _name;
        [SerializeField] private UnitSpecialization _specialization;
        [SerializeField] private DiceConfig _dice;
        [SerializeField] private int _hp;
        [SerializeField] private AssetReference _prefab;

        public bool IsEnabled => _isEnabled;
        public UnitSpecialization Specialization => _specialization;
        public int Hp => _hp;
        public DiceConfig Dice => _dice;
        public AssetReference Prefab => _prefab;
    }
}