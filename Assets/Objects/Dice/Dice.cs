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
    [RequireComponent(typeof(PhotonView))]
    public class Dice : UIElement
    {
        public const int Max = 6;

        public int RandomValue { get { return Random.Range(1, Max + 1); } }

        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

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
            View = GetComponent<PhotonView>();

            Button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            View.RPC(nameof(Roll), RpcTarget.All, RandomValue);
        }
        
        public event Action<int> OnRoll;
        [PunRPC]
        public void Roll(int value)
        {
            label.text = value.ToString();

            if (OnRoll != null) OnRoll(value);
        }
    }
}