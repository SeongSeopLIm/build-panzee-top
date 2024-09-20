using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace WAK
{
    public abstract class StageBase : IState
    {
        private CompositeDisposable _disposables = null;
        protected CompositeDisposable disposables
        {
            get
            {
                if(_disposables == null || _disposables.IsDisposed)
                {
                    _disposables = new CompositeDisposable();
                }
                return _disposables;
            }
        }

        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {
            Dispose();
        }

        public virtual void Update()
        {

        }

        /// <summary>
        /// 기본적으로 StageBase.Exit호출되나, 일반화 과정에서 부모 Exit호출하지 않을 시 직접호출 필요.
        /// </summary>
        protected virtual void Dispose()
        {
            _disposables?.Clear();
        }
    }
}
