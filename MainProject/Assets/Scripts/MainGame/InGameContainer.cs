using VContainer;
using VContainer.Unity;
using UnityEngine;
using Player;
using Stage;
namespace GameMain
{
    public class InGameContainer : LifetimeScope
    {
        [SerializeField] private InGameUIModel inGameUIModel;
        [SerializeField] private PlayerModel playerModel;
        [SerializeField] private InputModel inputModel;
        [SerializeField] private PlayerCore playerCore;
        [SerializeField] private MainGameUIViewer mainGameUIViewer;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InGameSequencer>();
            builder.RegisterInstance(inGameUIModel);
            builder.RegisterInstance(playerModel);
            builder.RegisterInstance(inputModel);
            builder.RegisterInstance(playerCore);
            builder.RegisterInstance(StageLoader.CurrentStage);
            builder.RegisterInstance(mainGameUIViewer);
        }
    }
}