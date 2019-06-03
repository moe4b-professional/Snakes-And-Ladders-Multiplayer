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

using UnityEngine.EventSystems;

using Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Game
{
    [RequireComponent(typeof(Button))]
    public class Dice : UIElement
    {
        public const int Max = 6;

        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }
        public string Text
        {
            set
            {
                label.text = value;
            }
        }

        [SerializeField]
        protected Button button;
        public Button Button { get { return button; } }
        public bool Interactable
        {
            set
            {
                Button.interactable = value;
            }
        }

        public Core Core { get { return Core.Instance; } }

        public PhotonView View { get; protected set; }

        void Awake()
        {
            Button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            Roll();
        }
        
        public event Action<int> OnRoll;
        public void Roll()
        {
            var value = Random.Range(1, Max + 1);

            label.text = value.ToString();

            if (OnRoll != null) OnRoll(value);
        }
    }
}