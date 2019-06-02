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

using Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Game
{
	public class MultiplayerMenu : UIElement
    {
        public Core Core { get { return Core.Instance; } }

        public NetworkManager Network { get { return Core.Network; } }

		[SerializeField]
        protected PlayerNameInputField playerName;
        public PlayerNameInputField PlayerName { get { return playerName; } }

        [SerializeField]
        protected Button play;
        public Button Play { get { return play; } }

        void Start()
        {
            play.onClick.AddListener(OnPlay);

            Core.PlayerName.OnChange += OnPlayerNameChanged;
        }

        void OnPlayerNameChanged(string newValue)
        {
            play.interactable = Core.PlayerName.Value.Length > 0;
        }

        void OnPlay()
        {
            Core.Popup.Show("Connecting");

            Network.Callbacks.Connection.DisconnectedEvent += OnDisconnected;
            Network.Callbacks.Connection.ConnectedToMasterEvent += OnConnectedToMaster;

            PhotonNetwork.ConnectUsingSettings();
        }

        void OnConnectedToMaster()
        {
            Network.Callbacks.Connection.DisconnectedEvent -= OnDisconnected;
            Network.Callbacks.Connection.ConnectedToMasterEvent -= OnConnectedToMaster;

            Core.Popup.Hide();

            Core.Mode.Multiplayer.Begin();
        }

        void OnDisconnected(DisconnectCause cause)
        {
            Network.Callbacks.Connection.DisconnectedEvent -= OnDisconnected;
            Network.Callbacks.Connection.ConnectedToMasterEvent -= OnConnectedToMaster;

            Core.Popup.Show("Connection Failure\n" + Utility.RichText.Color(Utility.FormatCaps(cause.ToString()), "red"), Core.Popup.Hide, "Return");
        }
    }
}