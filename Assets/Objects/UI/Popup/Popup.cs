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
        public string Text
        {
            get
            {
                return label.text;
            }
            set
            {
                label.text = value;

                RebuildLayout();
            }
        }

        [SerializeField]
        protected Button button;
        public Button Button { get { return button; } }
        public bool Interactable
        {
            get
            {
                return button.gameObject.activeSelf;
            }
            set
            {
                button.gameObject.SetActive(value);

                RebuildLayout();
            }
        }

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
            RebuildLayout();

            base.Show();
        }

        void RebuildLayout()
        {
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }
    }
}