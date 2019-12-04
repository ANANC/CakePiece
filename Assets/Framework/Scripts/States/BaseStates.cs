using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANFramework
{
    public abstract class BaseStates
    {
        protected bool m_Result = true;

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual bool Update()
        {
            return m_Result;
        }

    }
}
