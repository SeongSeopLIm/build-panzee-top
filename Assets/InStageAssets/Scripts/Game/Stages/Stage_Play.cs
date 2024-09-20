using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAK.Managers;

namespace WAK.Game
{
    public class Stage_Play : StageBase
    {
        public override void Enter()
        {
            base.Enter();

            GameManager.Instance.Play();
        }

        public override void Exit()
        {
            base.Exit();

            GameManager.Instance.Stop();
        }
    }
}
