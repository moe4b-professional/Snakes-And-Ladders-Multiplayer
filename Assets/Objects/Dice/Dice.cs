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

        public Button Button { get; protected set; }

        public Core Core { get { return Core.Instance; } }

        public PhotonView View { get; protected set; }

        void Awake()
        {
            View = GetComponent<PhotonView>();

            Button = GetComponent<Button>();
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
            Button.interactable = false;

            label.text = value.ToString();

            if (PhotonNetwork.IsMasterClient)
                if (OnRoll != null) OnRoll(value);
        }

        public void Set(Player player)
        {
            View.RPC(nameof(SetRPC), RpcTarget.All, player.Owner);
        }
        [PunRPC]
        void SetRPC(Photon.Realtime.Player networkPlayer)
        {
            var player = Core.Players.Get(networkPlayer);

            Button.interactable = player == Core.Players.Local;
        }
    }
}