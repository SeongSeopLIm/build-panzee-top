
namespace WAK.Game
{
    /// <summary>
    /// ��ġ���� ���� ������� ������Ʈ 
    /// </summary>
    [ActorImpl(poolType: "WakHead", prefabRelativePath: "/WakHead.prefab")]
    public class WakHead : Actor
    {
        protected WakHeadImpl wakHeadimpl => impl as WakHeadImpl;

    }
}