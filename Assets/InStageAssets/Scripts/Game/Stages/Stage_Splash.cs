using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using WAK.Core;
using WAK.Managers;

namespace WAK.Game
{
    public class Stage_Splash : StageBase
    {
        public override void Enter()
        {
            base.Enter();

            Framework.Instance.IsApplicationReady
                .DistinctUntilChanged()
                .Subscribe(OnApplicaitonReadyChanged)
                .AddTo(disposables);
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void OnApplicaitonReadyChanged(bool isReady)
        {
            if (isReady)
            {
                StageManager.Instance.SwitchState(StageManager.StageType.Lobby);
            }
        }

    }
}
