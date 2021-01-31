using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DieScreen : MonoBehaviour
    {
        public CanvasGroup Grp;

        public static DieScreen Inst;

        public Text Text;
        
        public bool IsOn;

        private Action hideCallback;

        private void Awake()
        {
            Inst = this;
        }

        public void Update()
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Dash") && IsOn)
            {
                Hide();
            }
        }

        public void Show(Action onHideCallback)
        {
            IsOn = true;
            Time.timeScale = 0;
            Grp.alpha = 1;
            hideCallback = onHideCallback;
        }

        public void Hide()
        {
            IsOn = false;
            Time.timeScale = 1;
            Grp.alpha = 0;
            if (hideCallback != null)
            {
                hideCallback();
                hideCallback = null;
            }
        }
    }
}