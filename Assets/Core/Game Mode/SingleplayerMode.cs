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
	public class SingleplayerMode : GameMode.Module
	{
        public override void Init()
        {
            base.Init();
        }

        public override void Begin()
        {
            base.Begin();

            PhotonNetwork.OfflineMode = true;

            CreateRoom();
        }

        void CreateRoom()
        {
            Action<Photon.Realtime.Player> OnPlayerJoined = null;

            OnPlayerJoined = (Photon.Realtime.Player player) =>
            {
                Network.Players.OnJoined -= OnPlayerJoined;

                Network.OnBeginMatch += OnBeginMatch;

                Network.BeginMatch();
            };

            Network.Players.OnJoined += OnPlayerJoined;

            PhotonNetwork.CreateRoom("Singleplayer");
        }

        void OnBeginMatch()
        {
            Network.OnBeginMatch -= OnBeginMatch;

            Players.Spawn(Grid[0]);
            Menu.Dice.Show();
        }
    }
}