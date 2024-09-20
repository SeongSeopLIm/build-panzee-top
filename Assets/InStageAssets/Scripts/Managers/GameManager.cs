using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using WAK.Game;
using UnityEngine.InputSystem;
using UnityEditor.ShaderGraph;
using Cysharp.Threading.Tasks;
namespace WAK.Managers
{

    public class GameManager : MonoSingleton<GameManager>
    {
        public GameSettings GameSettings{ get; private set; }
        public GamePlayerController PlayerController { get; private set; }
        public MainControl MainControls { get; private set; } 

        public Camera MainCamera => player.PlayerCamera;

        #region GameLiveData
        private GameObject world;
        private Player player;

        private WakHeadImpl highestObject;
        private WakHeadImpl currentHoldingObject;
        public float CurrentTopHeight { get; private set; } = 0;

        public bool IsHoldingObject => currentHoldingObject != null; 
        #endregion

        public void Set(GameSettings gameSettings, GamePlayerController playerController)
        {
            Clear();
            this.GameSettings = gameSettings;
            this.PlayerController = playerController;
            if(MainControls == null)
            {
                MainControls = new MainControl();
                MainControls.play.Enable();
            }

            PlayerController.Initalize();
            var world = Instantiate(GameSettings.WorldPrefab, transform);
            Instantiate(GameSettings.PlayerPrefab, world.transform).TryGetComponent<Player>(out player); 
        }

        public void Clear()
        {
            currentHoldingObject = null;
            CurrentTopHeight = 0;
            // 쩝 게임로직은 일단 대충해보자. 
            if(world)
            {
                Destroy(world);
            }
            if(player)
            {
                Destroy(player.gameObject);
            }
            if(MainControls != null)
            {
                MainControls.RemoveAllBindingOverrides();
            }
            ObjectManager.Instance.Clear();
        }

        /// <summary>
        /// UI를 통한 사용 금지. Stage_Play 전환으로 글로벌 상태를 변경하여 진행.
        /// </summary>
        public void Play()
        {
            PlayerController.InputStateMachine.SwitchState(StateBase.GetOrCreate<InputState_Play>(PlayerController));
             
            SpawnRandomAndHold(Vector2.zero);
        }

        public void Stop()
        {
            PlayerController.InputStateMachine.SwitchState(StateBase.GetOrCreate<InputState_Wait>(PlayerController)); 
        }

        public void SpawnRandomAndHold(Vector2 screenPos)
        {
            var maxCount = GameSettings.SpawnBundleSettings.SpawnPrefabs.Count;
            var idx = Random.Range(0, maxCount);

            var objectParams = new ObjectManager.ObjectParams()
            {
                dataKey = idx, 
                poolID = GameSettings.SpawnBundleSettings.SpawnPrefabs[idx].name // 프리팹 이름 겹치지 않게 주의
            }; 

            var wakHeadImpl = ObjectManager.Instance.Spawn<WakHeadImpl>(objectParams);
            wakHeadImpl.holdingAtCursor.Value = true;
            currentHoldingObject = wakHeadImpl;
        }
         
        public void ReleaseHold()
        {
            if(currentHoldingObject != null)
            {
                currentHoldingObject.holdingAtCursor.Value = false;
                currentHoldingObject = null;
                ReloadHold_JustForTest().Forget();
            }  
        }
        public void RegisterHighestObject(WakHeadImpl nextHighestObject)
        {
            highestObject = nextHighestObject;
            CurrentTopHeight = Mathf.Max(CurrentTopHeight, highestObject.TopPositionY);
            Debug.Log($"[Game] New TOP Height : {CurrentTopHeight}");
        }

        private async UniTaskVoid ReloadHold_JustForTest()
        {
            await UniTask.Delay(1000);
            if(StageManager.Instance.CurrentStageType.Value == StageManager.StageType.Play)
            {
                SpawnRandomAndHold(Vector2.zero);
            } 
        }

        public Vector3 GetCursorHoldPosition()
        {
            var screenPos = Mouse.current.position.ReadValue();
            if (MainCamera == null)
            {
                Debug.LogWarning("Main Camera is not assigned.");
                return Vector3.zero;
            }

            Vector3 worldPos = MainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, MainCamera.nearClipPlane));
            worldPos.z = 0;
            worldPos.y = Mathf.Max(GameSettings.SpawnHeightMin, CurrentTopHeight + GameSettings.SpawnDistanceToTop);
            return worldPos;
        }

        private Vector3 GetCursorWorldPosition()
        {
            var screenPos = Mouse.current.position.ReadValue(); 
            if (MainCamera == null)
            {
                Debug.LogWarning("Main Camera is not assigned.");
                return Vector3.zero;
            }

            Vector3 worldPos = MainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, MainCamera.nearClipPlane));
            worldPos.z = 0;
            return worldPos;
        }
         
    }
}

