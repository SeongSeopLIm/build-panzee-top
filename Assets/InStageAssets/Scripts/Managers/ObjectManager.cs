using UnityCommunity.UnitySingleton; 

namespace WAK
{
	public class ObjectManager : Singleton<ObjectManager>
	{



		public T Spawn<T>() where T : Actor
		{
			return T;
		}
	}
}

