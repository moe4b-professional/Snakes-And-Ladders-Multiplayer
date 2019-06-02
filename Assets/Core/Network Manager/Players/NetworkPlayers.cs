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
	public class NetworkPlayers : MonoBehaviour
	{
        public Core Core { get { return Core.Instance; } }

        public NetworkManager Network { get { return Core.Network; } }

        public NetworkCallbacks Callbacks { get { return Network.Callbacks; } }

        public List<Photon.Realtime.Player> List { get; protected set; }

        public int Count { get { return List.Count; } }

        public event Action OnChange;
        void InvokeChange()
        {
            if (OnChange != null) OnChange();
        }

        public Photon.Realtime.Player this[int index]
        {
            get
            {
                return List[index];
            }
        }

		public void Init()
        {
            List = new List<Photon.Realtime.Player>();

            Callbacks.Room.PlayerEnteredEvent += OnPlayerEntered;
            Callbacks.Room.PlayerLeftEvent += OnPlayerLeft;

            Callbacks.Matchmaking.JoinedRoomEvent += OnJoinedRoom;
            Callbacks.Matchmaking.LeftRoomEvent += OnLeftRoom;
        }

        void OnJoinedRoom()
        {
            Clear();

            foreach (var data in PhotonNetwork.CurrentRoom.Players)
                OnPlayerEntered(data.Value);
        }

        public event Action<Photon.Realtime.Player> OnJoined;
        void OnPlayerEntered(Photon.Realtime.Player player)
        {
            List.Add(player);

            if (OnJoined != null) OnJoined(player);

            InvokeChange();
        }

        public event Action<Photon.Realtime.Player> OnLeft;
        void OnPlayerLeft(Photon.Realtime.Player player)
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
    }
}