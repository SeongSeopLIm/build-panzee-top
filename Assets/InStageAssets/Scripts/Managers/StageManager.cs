using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using WAK.Game;

namespace WAK.Managers
{ 
    /// <summary>
    /// Scene 과는 다른 개념으로, 동일 씬 내에서 진행에 따른 다른 개별 흐름 개념
    /// </summary>
    public class StageManager : Singleton<StageManager>
    {
        public enum StageType
        {
            Spalsh = 0,
            Lobby,
            Play,
            Result,
            Max
        }

        private ReactiveProperty<StageType> currentStageType = new ReactiveProperty<StageType>(StageType.Spalsh);
        public IReadOnlyReactiveProperty<StageType> CurrentStageType => currentStageType;
        public StageBase CurrentStage { get; private set; }
        private Dictionary<StageType, StageBase> stagesByType = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            GenerateStages();
        }

        /// <param name="newState"></param>
        public void SwitchStage(StageType newState)
        {
            Debug.Log($"[Stage] {CurrentStage} to {newState}");
            if (CurrentStage != null)
            {
                CurrentStage.Exit();
            }
            if(!stagesByType.TryGetValue(newState, out var stage))
            {
                Debug.LogError($"생성되지 않은 Stage 이전 시도 : {newState}");
                return;
            }
            CurrentStage = stage;

            if (CurrentStage != null)
            {
                CurrentStage.Enter();
            }
            currentStageType.Value = newState;
        }

        private void GenerateStages()
        {
            for(int i = 0; i < (int)StageType.Max; i++)
            {
                var stageType = (StageType)i;
                stagesByType.Add(stageType, StageFactory(stageType));
            }
        }

        private StageBase StageFactory(StageType type)
        {
            return type switch
            {
                StageType.Spalsh => new Stage_Splash(),
                StageType.Lobby => new Stage_Lobby(),
                StageType.Play => new Stage_Play(),
                StageType.Result => new Stage_Result(),
                StageType.Max => throw new System.NotImplementedException(),
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}