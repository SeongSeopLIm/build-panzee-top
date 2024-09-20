using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using Cysharp.Threading.Tasks;

namespace WAK.Managers
{ 
    /// <summary>
    /// ����Ƽ �� ��ȯ�� ����. ���� �۷ι� ������Ʈ ������ StageManager ���.
    /// </summary>
    public class UnityGameSceneManager : PersistentMonoSingleton<UnityGameSceneManager>
    {
        public enum SceneType
        {
            scene_initial,
            scene_lobby_and_game // �κ�� ���� �÷��� ���� ������ ����
        } 

        public async UniTask LoadSceneAsync(SceneType sceneType)
        {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneType.ToString());

        }
    }
}
