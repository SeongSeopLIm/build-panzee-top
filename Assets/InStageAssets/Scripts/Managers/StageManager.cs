using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using WAK.Game;

namespace WAK.Managers
{ 
    /// <summary>
    /// Scene ���� �ٸ� ��������, ���� �� ������ ���࿡ ���� �ٸ� ���� �帧 ����
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
                Debug.LogError($"�������� ���� Stage ���� �õ� : {newState}");
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