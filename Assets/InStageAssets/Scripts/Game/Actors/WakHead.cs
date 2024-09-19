
namespace WAK.Game
{
    /// <summary>
    /// 위치값을 갖는 월드상의 오브젝트 
    /// </summary>
    [ActorImpl(poolType: "WakHead", prefabRelativePath: "/WakHead.prefab")]
    public class WakHead : Actor
    {
        protected WakHeadImpl wakHeadimpl => impl as WakHeadImpl;

    }
}