using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAK.Managers;

namespace WAK.Game
{
    public class Stage_Result : StageBase
    {
        public override void Enter()
        {
            base.Enter();
            GameManager.Instance.Stop();
        }

        public override void Exit()
        { 
            base.Exit();
        }
    }
}
