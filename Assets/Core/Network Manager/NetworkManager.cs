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
        public NetworkCallbacks Callbacks { get; protected set; }

        public NetworkPlayers Players { get; protected set; }

        public void Init()
        {
            Callbacks = Utility.GetDependancy<NetworkCallbacks>();

            Players = Utility.GetDependancy<NetworkPlayers>();
            Players.Init();

            PhotonNetwork.LocalPlayer.NickName = Core.PlayerName.Value;
            Core.PlayerName.OnChange += OnPlayerNameChanged;
        }

        void OnPlayerNameChanged(string newValue)
        {
            PhotonNetwork.LocalPlayer.NickName = newValue;
        }

        public void BeginMatch()
        {
            photonView.RPC(nameof(BeginMatchRPC), RpcTarget.AllBuffered);
        }
        public event Action OnBeginMatch;
        [PunRPC]
        void BeginMatchRPC()
        {
            if (OnBeginMatch != null) OnBeginMatch();
        }

        public void Stop()
        {
            PhotonNetwork.Disconnect();
        }
    }
}