using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game
{
	public class Popup : UIElement
	{
		[SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

        [SerializeField]
        protected Button button;
        public Button Button { get { return button; } }

        public Text ButtonLabel { get; protected set; }

        public void Init()
        {
            ButtonLabel = button.GetComponentInChildren<Text>();

            button.onClick.AddListener(OnButton);
        }

        Action action;
        void OnButton()
        {
            if (action != null) action();
        }

        public void Show(string text)
        {
            Show(text, null, null);
        }

        public void Show(string text, Action callback, string buttonText)
        {
            label.text = text;

            if(callback == null)
            {
                action = null;

                button.gameObject.SetActive(false);
            }
            else
            {
                action = callback;

                if (buttonText != null)
                    ButtonLabel.text = buttonText;

                button.gameObject.SetActive(true);
            }

            if (!Visible) Show();
        }

        public override void Show()
        {
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);

            base.Show();
        }
    }
}