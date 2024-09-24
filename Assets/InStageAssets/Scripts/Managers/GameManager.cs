
using UnityEngine;
using UnityCommunity.UnitySingleton;
using WAK.Game;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace WAK.Managers
{
    public interface IGameDataListener
    {
        // REVIEW : �켱 �׳� �̱��濡�� �˾Ƽ� ã�� ��������. �����ϸ� �׶� ���ӵ����� �Ѱ��ִ� ��� ��ȯ
        void OnUpdate();
    }

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

        public float Score { get; private set; } = 0;
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
            Score = 0;
            // �� ���ӷ����� �ϴ� �����غ���. 
            if (world)
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
        /// UI�� ���� ��� ����. Stage_Play ��ȯ���� �۷ι� ���¸� �����Ͽ� ����.
        /// </summary>
        public void Play()
        {
            ObjectManager.Instance.Clear();
            currentHoldingObject = null;
            CurrentTopHeight = 0;
            Score = 0;
            PlayerController.InputStateMachine.SwitchState(StateBase.GetOrCreate<InputState_Play>(PlayerController)); 
            SpawnRandomAndHold(Vector2.zero);
        }

        public void Stop()
        {
            PlayerController.InputStateMachine.SwitchState(StateBase.GetOrCreate<InputState_Wait>(PlayerController)); 
        }


        // NOTE : ���ؾ� 1~2�� ������ �������� ����Ʈ�� �ۼ�. 
        List<IGameDataListener> gameDataListeners = new List<IGameDataListener>();
        public void AddDataListener(IGameDataListener listener)
        { 
            gameDataListeners.Add(listener);
        }
        public void RemoveDataListener(IGameDataListener listener)
        {
            gameDataListeners.Remove(listener);
        }

        public void SpawnRandomAndHold(Vector2 screenPos)
        {
            var spawnBundleDatas = GameSettings.SpawnBundleSettings.SpawnBundleDatas;
            if (spawnBundleDatas == null || spawnBundleDatas.Count == 0)
            {
                Debug.LogError("SpawnBundleDatas ����Ʈ�� ����ֽ��ϴ�.");
                return;
            }
             
            int totalProbability = GameSettings.SpawnBundleSettings.GetTotalProbability();
            if (totalProbability <= 0)
            {
                Debug.LogError("probabilityCount �� 0");
                return;
            }
             
            int randomValue = Random.Range(0, totalProbability); 

            int selectedIndex = GameSettings.SpawnBundleSettings.GetSelectedIndex(randomValue);
            if (selectedIndex < 0 || selectedIndex >= spawnBundleDatas.Count)
            {
                Debug.LogError("������ ���ÿ� ���� randomValue: " + randomValue);
                return;
            }
            Debug.Log($"SpawnIdx : {selectedIndex} / Totla : {totalProbability} / Target : {randomValue}");

            var selectedData = spawnBundleDatas[selectedIndex];
            var objectParams = new ObjectManager.ObjectParams()
            {
                dataKey = selectedIndex,
                poolID = selectedData.spawnPrefab.name // ������ �̸��� ��ġ�� �ʵ��� ����
            };

            var wakHeadImpl = ObjectManager.Instance.Spawn<WakHeadImpl>(objectParams, GetCursorHoldPosition());
            wakHeadImpl.holdingAtCursor.Value = true;
            currentHoldingObject = wakHeadImpl;
        }


        public void ReleaseHold()
        {
            if(currentHoldingObject != null)
            {
                currentHoldingObject.holdingAtCursor.Value = false;
                currentHoldingObject = null; 
            }  
        }

        public void RegisterHighestObject(WakHeadImpl nextHighestObject)
        {
            highestObject = nextHighestObject;
            CurrentTopHeight = Mathf.Max(CurrentTopHeight, highestObject.TopPositionY);
            if(Score < CurrentTopHeight)
            {// �켱 �ְ� ���� �������� ���ھ� ����
                Score = CurrentTopHeight; 
            }
            
            Debug.Log($"[Game] New TOP Height : {CurrentTopHeight}");
            gameDataListeners.ForEach(listener => listener.OnUpdate());
        }

        public void CheckRespawn()
        {
            if (StageManager.Instance.CurrentStageType.Value != StageManager.StageType.Play)
                return;
            if (IsHoldingObject)
                return;

            bool canResapwn = true;
            var iter = ObjectManager.Instance.GetObjects();
            while (iter.MoveNext())
            {
                var spawnObject = iter.Current;
                if (spawnObject is not WakHeadImpl wakHead)
                    continue;

                if(wakHead.isMoving.Value)
                {
                    canResapwn = false;
                    break;
                }
            }
            if (!canResapwn)
                return;

            SpawnRandomAndHold(Vector2.zero);

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

