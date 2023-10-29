using CustomTypes.Enums.Battle;
using Infrastructure;
using Leopotam.EcsLite;
using Units;
using Utils.Extensions;
using Zenject;

namespace Battle
{
    public class UnitAnimationLaunchSystem : IEcsRunSystem
    {
        private readonly BattleDeathService _deathService;
        private readonly BattleAnimatorService _animatorService;

        private readonly EcsFilter _animationLaunchUnitsFilter;
        private readonly EcsFilter _diedDicesFilter;
        private readonly EcsPool<AnimationLaunchComponent> _animationLaunchPool;

        [Inject]
        public UnitAnimationLaunchSystem(EcsService ecsService, BattleDeathService deathService, BattleAnimatorService animatorService)
        {
            _deathService = deathService;
            _animatorService = animatorService;

            _animationLaunchUnitsFilter = ecsService.World.Filter<AnimationLaunchComponent>().Inc<UnitViewComponent>().End();
            _animationLaunchPool = ecsService.World.GetPool<AnimationLaunchComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var unit in _animationLaunchUnitsFilter)
            {
                ref var animationLaunchComponent = ref _animationLaunchPool.Get(unit);
                var sideType = animationLaunchComponent.DiceSideType;
                
                switch (animationLaunchComponent.AnimationType)
                {
                    case BattleAnimationType.FacetApply:
                        _animatorService.PlayFacetAnimation(unit, sideType);
                        break;
                    
                    case BattleAnimationType.FacetReaction:
                        if (sideType.IsDamageSide() && _deathService.IsDead(unit))
                            _animatorService.ToggleDeathAnimation(unit, true);
                        else
                            _animatorService.PlayFacetReactionAnimation(unit, sideType);
                        break;
                }
            }
        }
    }
}