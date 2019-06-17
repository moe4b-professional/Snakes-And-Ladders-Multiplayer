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
	public class NetworkPlayers : MonoBehaviour
	{
        public Core Core { get { return Core.Instance; } }

        public NetworkManager Network { get { return Core.Network; } }

        public NetworkCallbacks Callbacks { get { return Network.Callbacks; } }

        public List<PunPlayer> List { get; protected set; }

        public int Count { get { return List.Count; } }

        public PunPlayer this[int index] { get { return List[index]; } }

        public event Action OnChange;
        void InvokeChange()
        {
            if (OnChange != null) OnChange();
        }

		public void Init()
        {
            List = new List<PunPlayer>();

            Callbacks.Matchmaking.JoinedRoomEvent += OnJoinedRoom;

            Callbacks.Room.PlayerEnteredEvent += OnPlayerEntered;
            Callbacks.Room.PlayerLeftEvent += OnPlayerLeft;

            Callbacks.Matchmaking.LeftRoomEvent += OnLeftRoom;
        }

        void OnJoinedRoom()
        {
            Clear();

            foreach (var data in PhotonNetwork.CurrentRoom.Players)
                OnPlayerEntered(data.Value);
        }

        public event Action<PunPlayer> OnJoined;
        void OnPlayerEntered(PunPlayer player)
        {
            List.Add(player);

            if (OnJoined != null) OnJoined(player);

            InvokeChange();
        }

        public event Action<PunPlayer> OnLeft;
        void OnPlayerLeft(PunPlayer player)
        {
            List.Remove(player);

            if (OnLeft != null) OnLeft(player);

            InvokeChange();
        }

        void OnLeftRoom()
        {
            Clear();
        }

        public event Action OnClear;
        void Clear()
        {
            List.Clear();

            if (OnClear != null) OnClear();

            InvokeChange();
        }

        void OnDestroy()
        {
            OnChange = null;
            OnJoined = null;
            OnLeft = null;
            OnClear = null;

            Callbacks.Matchmaking.JoinedRoomEvent -= OnJoinedRoom;

            Callbacks.Room.PlayerEnteredEvent -= OnPlayerEntered;
            Callbacks.Room.PlayerLeftEvent -= OnPlayerLeft;

            Callbacks.Matchmaking.LeftRoomEvent -= OnLeftRoom;
        }
    }
}