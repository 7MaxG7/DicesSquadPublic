using System;
using CustomTypes.Enums.Units;
using UnityEngine;

namespace CustomTypes
{
    [Serializable]
    public struct UnitSpecialization
    {
        [SerializeField] private UnitClass _class;
        [SerializeField] private UnitArchetype _archetype;
        [SerializeField] [Min(1)] private int _level;

        public UnitSpecialization(UnitClass unitClass, UnitArchetype archetype, int level)
        {
            _class = unitClass;
            _archetype = archetype;
            _level = level;
        }

        public UnitClass Class => _class;
        public UnitArchetype Archetype => _archetype;
        public int Level => _level;

        public int Id => (int)_class * 10000 + (int)_archetype * 100 + _level;

        public override string ToString()
            => $"{Class} | {Archetype} | {Level}";
    }
}