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
    [RequireComponent(typeof(PhotonView))]
	public class MultiplayerMode : GameMode.Module
	{
        public const int MaxPlayers = 4;

        public PhotonView View { get; protected set; }

        public override void Init()
        {
            base.Init();

            View = GetComponent<PhotonView>();
        }

        public override void Begin()
        {
            base.Begin();

            JoinRandomRoom();
        }

        void JoinRandomRoom()
        {
            Menu.Popup.Show("Finding Match");

            Action onJoinedRoom = null;
            NetworkCallbacks.MatchmakingCallbacks.FailDelegate onJoinRoomFailed = null;

            onJoinedRoom = () =>
            {
                Network.Callbacks.Matchmaking.JoinedRoomEvent -= onJoinedRoom;
                Network.Callbacks.Matchmaking.JoinRandomRoomFailedEvent -= onJoinRoomFailed;
                Network.Callbacks.Matchmaking.JoinRoomFailedEvent -= onJoinRoomFailed;

                Menu.Popup.Hide();

                OnEntryEnd();
            };

            onJoinRoomFailed = (short returnCode, string message) =>
            {
                Network.Callbacks.Matchmaking.JoinedRoomEvent -= onJoinedRoom;
                Network.Callbacks.Matchmaking.JoinRandomRoomFailedEvent -= onJoinRoomFailed;
                Network.Callbacks.Matchmaking.JoinRoomFailedEvent -= onJoinRoomFailed;

                CreateRoom();
            };

            Network.Callbacks.Matchmaking.JoinedRoomEvent += onJoinedRoom;
            Network.Callbacks.Matchmaking.JoinRandomRoomFailedEvent += onJoinRoomFailed;
            Network.Callbacks.Matchmaking.JoinRoomFailedEvent += onJoinRoomFailed;

            if (PhotonNetwork.JoinRandomRoom())
            {

            }
            else
                onJoinRoomFailed(0, "Initial Setup Error");
        }

        void CreateRoom()
        {
            Menu.Popup.Show("Creating Match");

            Action onCreatedRoom = null;

            NetworkCallbacks.MatchmakingCallbacks.FailDelegate onCreateRoomFailed = null;

            onCreatedRoom = () =>
            {
                Network.Callbacks.Matchmaking.CreatedRoomEvent -= onCreatedRoom;
                Network.Callbacks.Matchmaking.CreateRoomFailedEvent -= onCreateRoomFailed;

                Menu.Popup.Hide();

                OnEntryEnd();
            };

            onCreateRoomFailed = (short returnCode, string message) =>
            {
                Network.Callbacks.Matchmaking.CreatedRoomEvent -= onCreatedRoom;
                Network.Callbacks.Matchmaking.CreateRoomFailedEvent -= onCreateRoomFailed;

                Menu.Popup.Show(Utility.RichText.Color("Failed to Create Match" + Environment.NewLine + message, "red"), Core.Reload, "Reload");
            };

            Network.Callbacks.Matchmaking.CreatedRoomEvent += onCreatedRoom;
            Network.Callbacks.Matchmaking.CreateRoomFailedEvent += onCreateRoomFailed;

            var options = new RoomOptions()
            {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = MultiplayerMode.MaxPlayers,
            };

            if (PhotonNetwork.CreateRoom(Core.PlayerName.Value + "'s Room", options))
            {

            }
            else
                onCreateRoomFailed(0, "Initial Setup Error");
        }

        void OnEntryEnd()
        {
            Menu.Multiplayer.Hide();

            Menu.Room.Show();
            
            Network.OnBeginMatch += OnBeginMatch;
        }
        
        void OnBeginMatch()
        {
            Network.OnBeginMatch -= OnBeginMatch;

            Core.Players.Spawn();

            Menu.Room.Hide();
        }

        public class Module : MonoBehaviour
        {
            public Core Core { get { return Core.Instance; } }

            public NetworkManager Network { get { return Core.Network; } }

            public PlayersManager Players { get { return Core.Players; } }

            public PlayGrid Grid { get { return Core.Grid; } }

            public GameMenu Menu { get { return Core.Menu; } }

            public GameMode Mode { get { return Core.Mode; } }

            public MultiplayerMode Multiplayer { get { return Mode.Multiplayer; } }

            public virtual void Init()
            {

            }

            public virtual void Begin()
            {

            }

            public event Action OnEnd;
            protected virtual void End()
            {
                if (OnEnd != null) OnEnd();
            }
        }
    }
}