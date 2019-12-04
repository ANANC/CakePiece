using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANFramework
{

    public class States : BaseBehaviour
    {
        private int m_CurState;
        private int m_StateCount;
        private List<BaseStates> m_StateList = new List<BaseStates>();

        public override void Init()
        {
            Reset();
            m_StateList.Add(new StartState());
            m_StateCount = m_StateList.Count;
        }

        public override void Start()
        {
            NextState();
        }

        public override void Update()
        {
            if (m_CurState < m_StateCount)
            {
                if (m_StateList[m_CurState].Update())
                {
                    NextState();
                }
            }
        }

        private void Reset()
        {
            m_CurState = -1;
        }

        private void NextState()
        {
            if (m_StateCount > 0)
            {
                if (m_CurState >= 0)
                {
                    m_StateList[m_CurState].Exit();
                }

                m_CurState++;
                if (m_CurState < m_StateList.Count)
                {
                    m_StateList[m_CurState].Enter();
                }
            }
        }
    }
}