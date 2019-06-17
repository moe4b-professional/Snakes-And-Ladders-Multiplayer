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

using Photon.Pun;

using PunPlayer = Photon.Realtime.Player;

namespace Game
{
	public class RoomMenu : UIElement
	{
        public Core Core { get { return Core.Instance; } }

        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

        [SerializeField]
        protected Button begin;
        public Button Begin { get { return begin; } }

        void Awake()
        {
            begin.onClick.AddListener(OnBegin);
        }

        void OnEnable()
        {
            if(PhotonNetwork.InRoom == false)
            {
                Debug.LogError("Opened Room Menu Without Actually Being In A Room");

                Visible = false;

                return;
            }

            label.text = PhotonNetwork.CurrentRoom.Name;

            begin.gameObject.SetActive(PhotonNetwork.IsMasterClient);

            Core.Network.Callbacks.Room.MasterClientChangedEvent += OnMasterClientChanged;
        }

        void OnMasterClientChanged(PunPlayer obj)
        {
            begin.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        }

        void OnBegin()
        {
            Core.Match.Begin();
        }

        void OnDisable()
        {
            Core.Network.Callbacks.Room.MasterClientChangedEvent -= OnMasterClientChanged;
        }
    }
}