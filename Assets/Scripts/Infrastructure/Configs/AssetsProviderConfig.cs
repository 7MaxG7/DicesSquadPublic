using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Configs
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(AssetsProviderConfig), fileName = nameof(AssetsProviderConfig), order = 1)]
    public class AssetsProviderConfig : ScriptableObject
    {
        [SerializeField] private AssetReference[] _gameAssets;
        [SerializeField] private AssetReference[] _battleSceneAssets;

        public IEnumerable<AssetReference> GetAssetReferencesForScene(string sceneName, out bool isDontDestroy)
        {
            isDontDestroy = sceneName == Constants.BOOTSTRAP_SCENE_NAME;
            return sceneName switch
            {
                Constants.BOOTSTRAP_SCENE_NAME => _gameAssets,
                Constants.BATTLE_SCENE_NAME => _battleSceneAssets,
                _ => Array.Empty<AssetReference>()
            };
        }
    }
}