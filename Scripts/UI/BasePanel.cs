using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace  UI
{
// 2. UI面板基类
    public abstract class BasePanel : MonoBehaviour
    {
        public UILayer Layer { get; set; }
        public bool IsActive { get; private set; }

        public virtual void OnEnter(object args = null)
        {
            IsActive = true;
            gameObject.SetActive(true);
        }

        public virtual void OnExit()
        {
            IsActive = false;
            gameObject.SetActive(false);
        }

        public virtual void OnPause()
        {

            IsActive = false;
        }

        public virtual void OnResume()
        {
            IsActive = true;
        }
    }
}