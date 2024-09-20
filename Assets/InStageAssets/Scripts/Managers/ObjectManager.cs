// ObjectManager.cs
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Pool;
using WAK.Game;

namespace WAK
{
	public class ObjectManager : Singleton<ObjectManager>
	{
		// ������ ������Ʈ ���� �ϳ��ε� �� �̰� �ָ����¡ ������...
        private Dictionary<string, ObjectPool<Actor>> pools = new Dictionary<string, ObjectPool<Actor>>();
		private Dictionary<int, ActorImpl> objectImplByIntanceID = new Dictionary<int, ActorImpl>();
        private Dictionary<int, Actor> objectByIntanceID = new Dictionary<int, Actor>();

		/// <summary>
		/// �ʱ�ȭ�� ���� ������Ʈ �Ķ����. �߻����� ���̴�, ������� ���� �ְ� ������ �� �˾Ƽ� ���.
		/// </summary>
		public struct ObjectParams
		{
			public int dataKey;
		}

        public ActorType Spawn<ActorType>(ObjectParams objectParams) where ActorType : ActorImpl, new()
		{
			string implID = GetActorID<ActorType>();
			if (string.IsNullOrEmpty(implID))
			{
				Debug.LogError($"ActorAttribute ���� : {typeof(ActorType).Name}");
				return null;
			}

			if (!pools.ContainsKey(implID))
			{
				pools[implID] = new ObjectPool<Actor>(
					createFunc: () => { return CreateNewActor<ActorType>(); },
					actionOnGet: (arg1) => { OnGetActor(arg1, objectParams); },
					actionOnRelease: OnReleaseActor,
					actionOnDestroy: DestroyActor,
					collectionCheck: false,
					defaultCapacity: 10,
					maxSize: 1000
				);
			}

			Actor actorInstance = pools[implID].Get();  

			if (actorInstance == null)
			{
				Debug.LogError($"�ν��Ͻ� ���� ���� ID: {implID}");
				return null;
			}


			return actorInstance.Impl as ActorType;
		}

		public void Release(ActorImpl actorImpl)
		{ 
			if(!objectByIntanceID.TryGetValue(actorImpl.ObjectID, out var actor))
            {
                Debug.LogError($"�ʵ忡 ���� ���� Release �õ� : {actorImpl.GetType().Name}");
                return;
            }

			string poolID = GetActorPoolID(actorImpl.GetType());
			if (string.IsNullOrEmpty(poolID))
			{
				Debug.LogError($"ActorAttribute ���� : {actor.GetType().Name}");
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
				Debug.LogError($"ActorAttribute ���� : {typeof(T).Name}");
				prefab = null;
				return false;
			}

			// OPTIMIZE : actorAttribute.PrefabPath���� ������ ���� �ܾ CodeGeneration �ϴ� �������� ���� ����
			var prefabObject = Resources.Load<GameObject>(actorAttribute.PrefabPath);
			if(prefabObject == null)
			{
				Debug.LogError($"Prefab �ε� ���� : {actorAttribute.PrefabPath}");
				prefab = null;
				return false;
			}
			return prefabObject.TryGetComponent<Actor>(out prefab);
		}

		private Actor CreateNewActor<T>() where T : ActorImpl
        {
			if (TryGetActorPrefab<T>(out Actor actorPrefab))
			{
				Debug.LogError($"Cannot find prefab : {nameof(T)}");
				return null;
			}

			GameObject actorGO = GameObject.Instantiate(actorPrefab.gameObject);
			Actor actor = actorGO.GetComponent<Actor>();
			if (actor == null)
			{ 
				GameObject.Destroy(actorGO);
                return null;
            }

            var impl = Activator.CreateInstance(typeof(T)) as ActorImpl; 
            actor.OnCreated(impl); 

            actor.gameObject.SetActive(false);
			return actorPrefab;
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

		private string GetActorID<T>() where T : ActorImpl
		{
			Type type = typeof(T);
			// REVIEW : �̷� ������ Attribue ���� ��������, �ϴ� ������ �����̴� �׳� �����Ѵ�. ���߿� �Ž����� ���� 
			ActorImplAttribute attribute = type.GetCustomAttribute<ActorImplAttribute>();
			return attribute?.PoolID;
		}

		private string GetActorPoolID(Type actorImplType)
		{
			ActorImplAttribute attribute = actorImplType.GetCustomAttribute<ActorImplAttribute>();
			return attribute?.PoolID;
		}
	}
}
