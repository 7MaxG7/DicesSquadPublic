using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace Infrastructure
{
    public class StaticDataService
    {
        private Dictionary<int, UnitConfig> _unitDatas;

        public void Init()
        {
            _unitDatas = Resources
                .LoadAll<UnitConfig>(Constants.UNIT_DATA_PATH)
                .Where(data => data.IsEnabled)
                .ToDictionary(data => data.Specialization.Id, data => data);
        }

        public UnitConfig GetUnit(int id)
            => TryGetData(_unitDatas, id);

        private TConfig TryGetData<TId, TConfig>(IReadOnlyDictionary<TId, TConfig> datas, TId id)
            => datas.TryGetValue(id, out var data)
                ? data
                : default;
    }
}