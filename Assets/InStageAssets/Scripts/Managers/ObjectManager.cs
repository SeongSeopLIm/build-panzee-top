// ObjectManager.cs
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Pool;
using WAK.Game;
using WAK.Managers;

namespace WAK
{
	public class ObjectManager : Singleton<ObjectManager>
	{
		// 어차피 오브젝트 종류 하나인데 나 이거 왜만들었징 ㅇㅅㅇ...
        private Dictionary<string, ObjectPool<Actor>> pools = new Dictionary<string, ObjectPool<Actor>>();
		private Dictionary<int, ActorImpl> objectImplByIntanceID = new Dictionary<int, ActorImpl>();
        private Dictionary<int, Actor> objectByIntanceID = new Dictionary<int, Actor>();

		/// <summary>
		/// 초기화에 사용될 오브젝트 파라미터. 추상적인 값이니, 보편벅인 값알 넣고 구현할 때 알아서 사용.
		/// </summary>
		public struct ObjectParams
        {
            public int dataKey;
            public string poolID;
        }

        public ActorType Spawn<ActorType>(ObjectParams objectParams) where ActorType : ActorImpl, new()
		{  

			if (!pools.ContainsKey(objectParams.poolID))
			{
				pools[objectParams.poolID] = new ObjectPool<Actor>(
					createFunc: () => { return CreateNewActor<ActorType>(objectParams); },
					actionOnGet: (arg1) => { OnGetActor(arg1, objectParams); },
					actionOnRelease: OnReleaseActor,
					actionOnDestroy: DestroyActor,
					collectionCheck: false,
					defaultCapacity: 10,
					maxSize: 1000
				);
			}

			Actor actorInstance = pools[objectParams.poolID].Get();  

			if (actorInstance == null)
			{
				Debug.LogError($"인스턴스 생성 실패 ID: {objectParams.poolID}");
				return null;
			}


			return actorInstance.Impl as ActorType;
		}

		public void Release(ActorImpl actorImpl)
		{ 
			if(!objectByIntanceID.TryGetValue(actorImpl.ObjectID, out var actor))
            {
                Debug.LogError($"필드에 없는 액터 Release 시도 : {actorImpl.GetType().Name}");
                return;
            }

			string poolID = actorImpl.ObjectParams.poolID;
			if (string.IsNullOrEmpty(poolID))
			{
				Debug.LogError($"ActorAttribute 누락 : {actor.GetType().Name}");
				return;
			}

			if (pools.ContainsKey(poolID))
			{
				pools[poolID].Release(actor);
			}
			else
			{
				Debug.LogError($"Not added pool : {poolID}");
				GameObject.Destroy(actor.gameObject);
			}
		}

		private bool TryGetActorPrefab<T>(out Actor prefab) where T : ActorImpl
        {
			var actorAttribute = typeof(T).GetCustomAttribute<ActorImplAttribute>();
			if(actorAttribute == null)
			{
				Debug.LogError($"ActorAttribute 누락 : {typeof(T).Name}");
				prefab = null;
				return false;
			}

			// OPTIMIZE : actorAttribute.PrefabPath말고 프리팹 폴더 긁어서 CodeGeneration 하는 방향으로 개선 가능
			var prefabObject = Resources.Load<GameObject>(actorAttribute.PrefabPath);
			if(prefabObject == null)
			{
				Debug.LogError($"Prefab 로드 실패 : {actorAttribute.PrefabPath}");
				prefab = null;
				return false;
			}
			return prefabObject.TryGetComponent<Actor>(out prefab);
		}

		private Actor CreateNewActor<T>(ObjectParams objectParams) where T : ActorImpl
        {
			/* 프리팹 - 스크립트 대응되는 구조가 아니라서 없는 게 맞을 듯.
			if (TryGetActorPrefab<T>(out Actor actorPrefab))
			{
				Debug.LogError($"Cannot find prefab : {nameof(T)}");
				return null;
			}
			*/

			var prefab = GameManager.Instance.GameSettings.SpawnBundleSettings.SpawnPrefabs[objectParams.dataKey];

            GameObject actorGO = GameObject.Instantiate(prefab);
			Actor actor = actorGO.GetComponent<Actor>();
			if (actor == null)
			{ 
				GameObject.Destroy(actorGO);
                return null;
            }

            var impl = Activator.CreateInstance(typeof(T)) as ActorImpl; 
            actor.OnCreated(impl); 

            actor.gameObject.SetActive(false);
			return actor;
		}

		private void OnGetActor(Actor actor, ObjectParams objectParams)
        {
            objectByIntanceID.Add(actor.gameObject.GetInstanceID(), actor);
            objectImplByIntanceID.Add(actor.gameObject.GetInstanceID(), actor.Impl); 
            actor.OnSpawn();
			actor.Impl.Set(objectParams);


            actor.gameObject.SetActive(true);
        }

		private void OnReleaseActor(Actor actor)
        { 
            objectByIntanceID.Remove(actor.gameObject.GetInstanceID());
            objectImplByIntanceID.Remove(actor.gameObject.GetInstanceID());
            actor.OnDespawn();
			actor.gameObject.SetActive(false);
		}

		private void DestroyActor(Actor actor)
        {
            objectByIntanceID.Remove(actor.gameObject.GetInstanceID());
            objectImplByIntanceID.Remove(actor.gameObject.GetInstanceID());
            GameObject.Destroy(actor.gameObject);
		}

	}
}
