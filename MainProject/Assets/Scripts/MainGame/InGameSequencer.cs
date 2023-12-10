using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;
using VContainer.Unity;
using System;
using UniRx;
using UnityEngine.InputSystem;
using Player;
using UniRx.Triggers;
namespace GameMain
{
    public class InGameSequencer : IInitializable, IDisposable
    {
        private InGameUIModel inGameUIModel;
        private InputModel inputModel;
        private PlayerCore playerCore;
        private bool isDied;
        private bool isPaused;
        private bool isSuccess;

        [Inject]
        public InGameSequencer
        (
            InGameUIModel inGameUIModel,
            InputModel inputModel,
            PlayerModel playerModel,
            PlayerCore playerCore
        )
        {
            this.inGameUIModel = inGameUIModel;
            this.inputModel = inputModel;
            this.playerCore = playerCore;

            SetInputEvent();

            inGameUIModel.inGameUiId
            .SkipLatestValueOnSubscribe()
            .Subscribe(type =>
            {
                isPaused = type == InGameUIModel.InGameUiType.pause;
            });

            //Playerのイベントを登録
            playerModel.PlayerHp
            .SkipLatestValueOnSubscribe()
            .Subscribe(hp =>
            {
                //死亡
                if (hp <= 0)
                {
                    isDied = true;
                    inGameUIModel.ChangeUIId(InGameUIModel.InGameUiType.dead);
                }
            });

            playerModel.isSucsess
            .SkipLatestValueOnSubscribe()
            .Subscribe(sucess =>
            {
                if (sucess)
                {
                    isSuccess = true;
                    inGameUIModel.ChangeUIId(InGameUIModel.InGameUiType.clear);
                }
            });
        }

        void SetInputEvent()
        {
            //入力イベントを登録
            inputModel.Initialize();

            //ワイヤーを撃つ
            inputModel.Fire
            .SkipLatestValueOnSubscribe()
            .Subscribe(isFire =>
            {
                if (isPaused || isDied || isSuccess) return;

                if (isFire)
                {
                    playerCore.Fire();
                }
                else
                {
                    playerCore.Release();
                }
            });

            //Update
            Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (isPaused || isDied || isSuccess) return;
                //移動
                playerCore.Move(inputModel.Move.Value);

                //視点移動
                playerCore.Look(inputModel.Look.Value);
            });

            //ポーズボタン
            inputModel.Pause
            .SkipLatestValueOnSubscribe()
            .Subscribe(pause =>
            {
                if (isDied || isSuccess || !pause) return;
                isPaused = !isPaused;
                inGameUIModel.ChangeUIId(isPaused ? InGameUIModel.InGameUiType.pause : InGameUIModel.InGameUiType.main);
            });

        }

        public void Dispose()
        {

        }

        public void Initialize()
        {
            inGameUIModel.ChangeUIId(InGameUIModel.InGameUiType.main);
        }
    }
}