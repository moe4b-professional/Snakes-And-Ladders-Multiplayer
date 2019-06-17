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

            Network.Callbacks.Connection.DisconnectedEvent += OnDisconnected;

            Connect();
        }

        void Connect()
        {
            Core.Popup.Show("Connecting", CancelEntry, "Cancel");
            Core.Popup.Interactable = false;

            Network.Callbacks.Connection.ConnectedToMasterEvent += OnConnected;

            if(Input.GetMouseButton(1) && Application.isEditor)
            {
                PhotonNetwork.OfflineMode = true;
            }
            else
            {
                if (PhotonNetwork.ConnectUsingSettings())
                {

                }
                else
                {
                    OnDisconnected(DisconnectCause.OperationNotAllowedInCurrentState);
                }
            }
        }
        void OnConnected()
        {
            Network.Callbacks.Connection.ConnectedToMasterEvent -= OnConnected;

            FindRoom();
        }

        void FindRoom()
        {
            Menu.Popup.Text = "Finding Match";

            Network.Callbacks.Matchmaking.JoinedRoomEvent += OnJoinedRoom;
            Network.Callbacks.Matchmaking.JoinRandomRoomFailedEvent += OnJoinRoomFailed;
            Network.Callbacks.Matchmaking.JoinRoomFailedEvent += OnJoinRoomFailed;

            if (PhotonNetwork.JoinRandomRoom())
            {

            }
            else
                OnJoinRoomFailed(0, "Initial Setup Error");
        }
        void OnJoinedRoom()
        {
            Network.Callbacks.Matchmaking.JoinedRoomEvent -= OnJoinedRoom;
            Network.Callbacks.Matchmaking.JoinRandomRoomFailedEvent -= OnJoinRoomFailed;
            Network.Callbacks.Matchmaking.JoinRoomFailedEvent -= OnJoinRoomFailed;

            OnEntryComplete();
        }
        void OnJoinRoomFailed(short returnCode, string message)
        {
            Network.Callbacks.Matchmaking.JoinedRoomEvent -= OnJoinedRoom;
            Network.Callbacks.Matchmaking.JoinRandomRoomFailedEvent -= OnJoinRoomFailed;
            Network.Callbacks.Matchmaking.JoinRoomFailedEvent -= OnJoinRoomFailed;

            CreateRoom();
        }

        void CreateRoom()
        {
            Menu.Popup.Text = "Creating Match";

            Network.Callbacks.Matchmaking.CreatedRoomEvent += OnCreatedRoom;
            Network.Callbacks.Matchmaking.CreateRoomFailedEvent += OnCreateRoomFailed;

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
                OnCreateRoomFailed(0, "Initial Setup Error");
        }
        void OnCreatedRoom()
        {
            Network.Callbacks.Matchmaking.CreatedRoomEvent -= OnCreatedRoom;
            Network.Callbacks.Matchmaking.CreateRoomFailedEvent -= OnCreateRoomFailed;

            OnEntryComplete();
        }
        void OnCreateRoomFailed(short returnCode, string message)
        {
            Network.Callbacks.Matchmaking.CreatedRoomEvent -= OnCreatedRoom;
            Network.Callbacks.Matchmaking.CreateRoomFailedEvent -= OnCreateRoomFailed;

            Menu.Popup.Show(Utility.RichText.Color("Failed to Create Match" + Environment.NewLine + message, "red"), Core.Reload, "Reload");
        }

        void CancelEntry()
        {
            Network.Stop(() => { Menu.Multiplayer.Reset(); CleanUp(); });
        }

        void OnDisconnected(DisconnectCause cause)
        {
            Network.Callbacks.Connection.DisconnectedEvent -= OnDisconnected;

            if(cause != DisconnectCause.DisconnectByClientLogic)
                Core.Popup.Show("Connection Failure\n" + Utility.RichText.Color(Utility.FormatCaps(cause.ToString()), "red"), Core.Reload, "Reload");
        }

        void OnEntryComplete()
        {
            Menu.Popup.Hide();

            Menu.Multiplayer.Hide();

            Menu.Room.Show();

            Core.Pawns.Spawn(Pawn.Player);

            Core.Match.OnBegin += OnBeginMatch;
        }
        
        void OnBeginMatch()
        {
            Core.Match.OnBegin -= OnBeginMatch;

            Menu.Room.Hide();
        }

        void OnDestroy()
        {
            CleanUp();
        }

        void CleanUp()
        {
            Network.Callbacks.Connection.DisconnectedEvent -= OnDisconnected;

            Network.Callbacks.Connection.ConnectedToMasterEvent -= OnConnected;

            Network.Callbacks.Matchmaking.JoinedRoomEvent -= OnJoinedRoom;
            Network.Callbacks.Matchmaking.JoinRandomRoomFailedEvent -= OnJoinRoomFailed;
            Network.Callbacks.Matchmaking.JoinRoomFailedEvent -= OnJoinRoomFailed;

            Network.Callbacks.Matchmaking.CreatedRoomEvent -= OnCreatedRoom;
            Network.Callbacks.Matchmaking.CreateRoomFailedEvent -= OnCreateRoomFailed;

            Core.Match.OnBegin -= OnBeginMatch;
        }
    }
}