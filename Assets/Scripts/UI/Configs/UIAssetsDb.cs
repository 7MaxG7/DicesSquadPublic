using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI
{
    [CreateAssetMenu(menuName = "Configs/UI/" + nameof(UIAssetsDb), fileName = nameof(UIAssetsDb), order = 0)]
    public class UIAssetsDb : ScriptableObject
    {
        [SerializeField] private AssetReference _permanentUIView;
        [SerializeField] private AssetReference _curtainView;

        public AssetReference PermanentUIView => _permanentUIView;

        public AssetReference CurtainView => _curtainView;
    }
}