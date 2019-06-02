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
    [RequireComponent(typeof(Button))]
	public class UITransitionButton : MonoBehaviour
	{
		[SerializeField]
        protected UIElement current;
        public UIElement Current { get { return current; } }

        [SerializeField]
        protected UIElement target;
        public UIElement Target { get { return target; } }

        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            if (current != null)
                current.Hide();

            if (target != null)
                target.Show();
        }
    }
}