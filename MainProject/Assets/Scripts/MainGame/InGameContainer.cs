using VContainer;
using VContainer.Unity;
using UnityEngine;
using Player;
namespace GameMain
{
    public class InGameContainer : LifetimeScope
    {
        [SerializeField] private InGameUIModel inGameUIModel;
        [SerializeField] private PlayerModel playerModel;
        [SerializeField] private InputModel inputModel;
        [SerializeField] private PlayerCore playerCore;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InGameSequencer>();
            builder.RegisterInstance(inGameUIModel);
            builder.RegisterInstance(playerModel);
            builder.RegisterInstance(inputModel);
            builder.RegisterInstance(playerCore);
        }
    }
}