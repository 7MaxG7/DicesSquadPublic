using System.Collections.Generic;
using Battle;
using CustomTypes.Enums;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Leopotam.EcsLite;
using Units.Views;
using UnityEngine;

namespace Units
{
    public class BattleAnimatorService
    {
        public bool IsAnimationInProcess => _animatingUnitFilter.GetEntitiesCount() > 0 || _animationLaunchFilter.GetEntitiesCount() > 0;

        private readonly EcsService _ecsService;
        private readonly CancellationTokenProvider _tokenProvider;
        private readonly BattleAnimationConfig _animationConfig;

        private readonly EcsFilter _animatorViewFilter;
        private readonly EcsFilter _animationLaunchFilter;
        private readonly EcsFilter _animatingUnitFilter;
        private readonly EcsPool<AnimatorViewComponent> _animatorViewPool;
        private readonly EcsPool<AnimatingViewComponent> _animatingViewPool;
        private readonly EcsPool<AnimationActionReadyEventComponent> _animationActionReadyPool;

        private readonly Dictionary<DiceSideType, int> _facetActionHashes;
        private readonly Dictionary<DiceSideType, int> _facetReactionHashes;
        private readonly int _deathHash;

        public BattleAnimatorService(EcsService ecsService, CancellationTokenProvider tokenProvider, BattleAnimationConfig animationConfig)
        {
            _ecsService = ecsService;
            _tokenProvider = tokenProvider;
            _animationConfig = animationConfig;

            _animatorViewFilter = ecsService.World.Filter<AnimatorViewComponent>().End();
            _animationLaunchFilter = ecsService.World.Filter<AnimationLaunchComponent>().End();
            _animatingUnitFilter = ecsService.World.Filter<AnimatingViewComponent>().
                Inc<UnitComponent>().
                Exc<DeadComponent>().End();
            _animatorViewPool = ecsService.World.GetPool<AnimatorViewComponent>();
            _animatingViewPool = ecsService.World.GetPool<AnimatingViewComponent>();
            _animationActionReadyPool = ecsService.World.GetPool<AnimationActionReadyEventComponent>();
            
            _facetActionHashes = new Dictionary<DiceSideType, int>
            {
                [DiceSideType.MeleeAttack] = Animator.StringToHash(_animationConfig.MeleeAttackParameterName),
                [DiceSideType.RangeAttack] = Animator.StringToHash(_animationConfig.RangeAttackParameterName),
            };
            
            _facetReactionHashes = new Dictionary<DiceSideType, int>
            {
                [DiceSideType.MeleeAttack] = Animator.StringToHash(_animationConfig.DamageReceiveParameterName),
                [DiceSideType.RangeAttack] = Animator.StringToHash(_animationConfig.DamageReceiveParameterName),
            };

            _deathHash = Animator.StringToHash(_animationConfig.DeathParameterName);
        }

        public void Clear()
        {
            foreach (var animator in _animatorViewFilter)
            {
                ref var animatorViewComponent = ref _animatorViewPool.Get(animator);
                animatorViewComponent.AnimatorListener.OnStateEnter -= EnterFacetAnimation;
                animatorViewComponent.AnimatorListener.OnStateExit -= ExitFacetAnimation;
                animatorViewComponent.AnimatorListener.OnActionAnimationEvent -= InvokeActionStart;
            }
        }

        public void InitComponents(int unit, Animator animator)
        {
            ref var animatorComponent = ref _animatorViewPool.Add(unit);
            animatorComponent.Animator = animator;

            var animatorListener = animator.gameObject.GetComponent<AnimatorListenerView>();
            animatorListener.Init(_ecsService.World.PackEntity(unit));
            animatorListener.OnStateEnter += EnterFacetAnimation;
            animatorListener.OnStateExit += ExitFacetAnimation;
            animatorListener.OnActionAnimationEvent += InvokeActionStart;
            animatorComponent.AnimatorListener = animatorListener;
        }

        public void PlayFacetAnimation(int unit, DiceSideType sideType)
        {
            if (!_facetActionHashes.TryGetValue(sideType, out var facetHash))
                return;

            PlayAnimation(unit, facetHash);
        }

        public void PlayFacetReactionAnimation(int unit, DiceSideType sideType)
        {
            if (!_facetReactionHashes.TryGetValue(sideType, out var hash))
                return;

            PlayAnimation(unit, hash);
        }

        public void ToggleDeathAnimation(int unit, bool isDead)
        {
            ref var animatorComponent = ref _animatorViewPool.Get(unit);
            animatorComponent.Animator.SetBool(_deathHash, isDead);
        }

        public bool IsApplyActionReady(int unit)
            => _animationActionReadyPool.Has(unit);

        private void PlayAnimation(int unit, int hash)
        {
            ref var animatorComponent = ref _animatorViewPool.Get(unit);
            _animatingViewPool.Add(unit);
            animatorComponent.Animator.SetTrigger(hash);
        }

        private void EnterFacetAnimation(EcsPackedEntity unitPacked, int facetHash)
        {
            if (!_ecsService.TryUnpackWithWarning(unitPacked, out var unit))
                return;
            
            if (Animator.StringToHash(_animationConfig.IdleAnimationName) == facetHash)
            {
                ref var animatorComponent = ref _animatorViewPool.Get(unit);
                // Required only if animation includes moving from needed position
                ResetTransformPositionAsync(animatorComponent.Animator.transform).Forget();

                _animatingViewPool.Del(unit);
            }
        }

        private void ExitFacetAnimation(EcsPackedEntity unitPacked, int facetHash)
        {
            if (!_ecsService.TryUnpackWithWarning(unitPacked, out var unit))
                return;

            if (Animator.StringToHash(_animationConfig.DeathAnimationName) == facetHash)
                _animatingViewPool.Del(unit);
        }

        private void InvokeActionStart(EcsPackedEntity unitPacked)
        {
            if (!_ecsService.TryUnpackWithWarning(unitPacked, out var unit))
                return;

            _animationActionReadyPool.Add(unit);
        }

        private async UniTaskVoid ResetTransformPositionAsync(Transform transform)
        {
            using var localCts = _tokenProvider.CreateLocalCts();
            await UniTask.NextFrame(localCts.Token);

            transform.localPosition = Vector3.zero;
        }
    }
}