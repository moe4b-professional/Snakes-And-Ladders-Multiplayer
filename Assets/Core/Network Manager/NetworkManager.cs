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
    public class NetworkManager : MonoBehaviourPun
    {
        public Core Core { get { return Core.Instance; } }

        public NetworkCallbacks Callbacks { get; protected set; }

        public NetworkPlayers Players { get; protected set; }

        public void Init()
        {
            Callbacks = Utility.GetDependancy<NetworkCallbacks>();
            Callbacks.Connection.DisconnectedEvent += OnDisconnected;

            Players = Utility.GetDependancy<NetworkPlayers>();
            Players.Init();

            PhotonNetwork.LocalPlayer.NickName = Core.PlayerName.Value;
            Core.PlayerName.OnChange += OnPlayerNameChanged;
        }

        void OnDisconnected(DisconnectCause cause)
        {
            if (cause != DisconnectCause.DisconnectByClientLogic)
                Core.Popup.Show("Disconnected\n" + Utility.RichText.Color(Utility.FormatCaps(cause.ToString()), "red"), Core.Reload, "Reload");
        }

        void OnPlayerNameChanged(string newValue)
        {
            PhotonNetwork.LocalPlayer.NickName = newValue;
        }

        public void BeginMatch()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            photonView.RPC(nameof(BeginMatchRPC), RpcTarget.AllBuffered);
        }
        public event Action OnBeginMatch;
        [PunRPC]
        void BeginMatchRPC()
        {
            if (OnBeginMatch != null) OnBeginMatch();
        }

        public void EndMatch(Photon.Realtime.Player winner)
        {
            photonView.RPC(nameof(EndMatchRPC), RpcTarget.AllBuffered, winner);
        }
        public event Action<Photon.Realtime.Player> OnEndMatch;
        [PunRPC]
        void EndMatchRPC(Photon.Realtime.Player winner)
        {
            var isWinner = winner == PhotonNetwork.LocalPlayer;

            Core.Popup.Show("You " + (isWinner ? "Won" : "Lost"), Core.Reload, "Reload");

            if (OnEndMatch != null) OnEndMatch(winner);
        }

        public void Stop()
        {
            PhotonNetwork.Disconnect();
        }
    }
}