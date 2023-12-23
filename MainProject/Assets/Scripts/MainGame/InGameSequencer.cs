using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System;
using UniRx;
using Player;
using KanKikuchi.AudioManager;
using Stage;
namespace GameMain
{
    public class InGameSequencer : IInitializable, IDisposable
    {
        private InGameUIModel inGameUIModel;
        private InputModel inputModel;
        private PlayerCore playerCore;
        private MainGameUIViewer mainGameUIViewer;
        private bool isPaused;
        private InGameUIModel.InGameUiType gameStatus;
        private CompositeDisposable _disposable = new CompositeDisposable();
        private float startTime;
        private StageData stageData;
        private int defeatEnemyCount;

        [Inject]
        public InGameSequencer
        (
            InGameUIModel inGameUIModel,
            MainGameUIViewer mainGameUIViewer,
            InputModel inputModel,
            PlayerModel playerModel,
            PlayerCore playerCore,
            StageData stageData
        )
        {
            Application.targetFrameRate = 60;

            this.inGameUIModel = inGameUIModel;
            this.inputModel = inputModel;
            this.playerCore = playerCore;
            this.stageData = stageData;
            this.mainGameUIViewer = mainGameUIViewer;

            StageLoader.InitSettings();

            SetInputEvent();

            //ゲームの状態を取得
            inGameUIModel.inGameUiId
            .SkipLatestValueOnSubscribe()
            .Subscribe(type =>
            {
                isPaused = type == InGameUIModel.InGameUiType.pause;

                gameStatus = type;

                if (type == InGameUIModel.InGameUiType.main)
                {
                    BGMManager.Instance.Play(BGMPath.MUS_MUS_BGM104, 0.3f);
                    startTime = Time.time;
                    return;
                }

                SEManager.Instance.Stop();
                BGMManager.Instance.Stop();

            }).AddTo(_disposable);

            //Playerのイベントを登録
            playerModel.PlayerHp
            .SkipLatestValueOnSubscribe()
            .Subscribe(hp =>
            {
                //死亡
                if (hp <= 0)
                {
                    inGameUIModel.ChangeUIId(InGameUIModel.InGameUiType.dead);
                }
            });

            //成功
            playerModel.isSucsess
            .SkipLatestValueOnSubscribe()
            .Subscribe(sucess =>
            {
                if (sucess)
                {
                    inGameUIModel.ChangeUIId(InGameUIModel.InGameUiType.clear);
                }

            }).AddTo(_disposable);
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
                if (gameStatus != InGameUIModel.InGameUiType.main) return;

                if (isFire)
                {
                    playerCore.Fire();
                }
                else
                {
                    playerCore.Release();
                }

            }).AddTo(_disposable);

            //Update
            Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (gameStatus != InGameUIModel.InGameUiType.main) return;

                //移動
                playerCore.Move(inputModel.Move.Value);
                playerCore.GrapplingMove();

                //視点移動
                playerCore.Look(inputModel.Look.Value);

                //時間計測
                inGameUIModel.ChangeTime(Time.time - startTime);

                Color color = playerCore.canReticle() ? Color.red : Color.white;
                mainGameUIViewer.ChnageReticleColor(color);

            }).AddTo(_disposable);

            //ポーズボタン
            inputModel.Pause
            .SkipLatestValueOnSubscribe()
            .Subscribe(pause =>
            {
                if (gameStatus != InGameUIModel.InGameUiType.main) return;
                isPaused = !isPaused;
                inGameUIModel.ChangeUIId(isPaused ? InGameUIModel.InGameUiType.pause : InGameUIModel.InGameUiType.main);
            }).AddTo(_disposable);

            //タイム更新
            inGameUIModel.passedTime
            .SkipLatestValueOnSubscribe()
            .Subscribe(time =>
            {
                mainGameUIViewer.UpdateTimeText(time);
            }).AddTo(_disposable);

        }

        public void Dispose()
        {
            _disposable.Clear();
        }

        public void Initialize()
        {
            inGameUIModel.ChangeUIId(InGameUIModel.InGameUiType.main);
            inGameUIModel.ChangeEnemyCount(0);
            inGameUIModel.SetStageEnemyCount(stageData.ClearEnemyCount);
            playerCore.setDefeatEnemyEvent = DefeatEnemy;
        }

        void DefeatEnemy()
        {
            defeatEnemyCount++;
            inGameUIModel.ChangeEnemyCount(inGameUIModel.enemyCount.Value + 1);
        }
    }
}