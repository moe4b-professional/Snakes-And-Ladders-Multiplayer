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

namespace Game
{
	public class VersusMode : GameMode.Module
	{
        public override void Begin()
        {
            base.Begin();

            Network.Callbacks.Connection.ConnectedToMasterEvent += OnConnected;
            PhotonNetwork.OfflineMode = true;
        }

        void OnConnected()
        {
            Network.Callbacks.Connection.ConnectedToMasterEvent -= OnConnected;

            Network.Callbacks.Matchmaking.CreatedRoomEvent += OnCreatedRoom;
            PhotonNetwork.CreateRoom("Offline");
        }

        void OnCreatedRoom()
        {
            Network.Callbacks.Matchmaking.CreatedRoomEvent -= OnCreatedRoom;

            Pawns.OnAdd += OnPlayersAdd;
            Pawns.Spawn(Pawn.Player);
            Pawns.Spawn(Pawn.Player);
        }

        void OnPlayersAdd(Pawn player)
        {
            if (Pawns.Count == 2)
            {
                Pawns.OnAdd -= OnPlayersAdd;

                Core.Match.OnBegin += OnBeginMatch;
                Core.Match.Begin();
            }
        }

        void OnBeginMatch()
        {
            Core.Match.OnBegin -= OnBeginMatch;

            Menu.Versus.Hide();
        }

        void OnDestroy()
        {
            Network.Callbacks.Connection.ConnectedToMasterEvent -= OnConnected;

            Network.Callbacks.Matchmaking.CreatedRoomEvent -= OnCreatedRoom;

            Pawns.OnAdd -= OnPlayersAdd;

            Core.Match.OnBegin -= OnBeginMatch;
        }
    }
}