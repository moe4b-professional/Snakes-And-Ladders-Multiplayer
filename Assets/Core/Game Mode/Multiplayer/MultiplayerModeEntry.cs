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
	public class MultiplayerModeEntry : MultiplayerMode.Module
    {
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

                End();
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

            var options = new RoomOptions()
            {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = MultiplayerMode.MaxPlayers,
            };

            Action onCreatedRoom = null;

            NetworkCallbacks.MatchmakingCallbacks.FailDelegate onCreateRoomFailed = null;

            onCreatedRoom = () =>
            {
                Network.Callbacks.Matchmaking.CreatedRoomEvent -= onCreatedRoom;
                Network.Callbacks.Matchmaking.CreateRoomFailedEvent -= onCreateRoomFailed;

                End();
            };

            onCreateRoomFailed = (short returnCode, string message) =>
            {
                Network.Callbacks.Matchmaking.CreatedRoomEvent -= onCreatedRoom;
                Network.Callbacks.Matchmaking.CreateRoomFailedEvent -= onCreateRoomFailed;

                Menu.Popup.Show(Utility.RichText.Color("Failed to Create Match" + Environment.NewLine + message, "red"), Reset, "Close");
            };

            Network.Callbacks.Matchmaking.CreatedRoomEvent += onCreatedRoom;
            Network.Callbacks.Matchmaking.CreateRoomFailedEvent += onCreateRoomFailed;

            if (PhotonNetwork.CreateRoom(Core.PlayerName.Value + " Room", options))
            {

            }
            else
                onCreateRoomFailed(0, "Initial Setup Error");
        }

        void Reset()
        {
            Menu.Popup.Show("Disconnecting");

            Action<DisconnectCause> onDisconnect = null;

            onDisconnect = (DisconnectCause cause) =>
            {
                Menu.Popup.Visible = false;

                Network.Callbacks.Connection.DisconnectedEvent -= onDisconnect;
            };

            Network.Callbacks.Connection.DisconnectedEvent += onDisconnect;

            PhotonNetwork.Disconnect();
        }
    }
}