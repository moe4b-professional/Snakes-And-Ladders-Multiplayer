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

namespace Game
{
	public class NetworkPlayersList : MonoBehaviour
	{
        [SerializeField]
        protected GameObject prefab;
        public GameObject Prefab { get { return prefab; } }

        public List<NetworkPlayersListElement> Elements { get; protected set; }

        public Core Core { get { return Core.Instance; } }

        public NetworkManager Network { get { return Core.Network; } }

        public NetworkPlayers Players { get { return Network.Players; } }

        void Awake()
        {
            Elements = new List<NetworkPlayersListElement>();
        }

        void OnEnable()
        {
            Players.OnJoined += OnJoined;
            Players.OnLeft += OnLeft;
            Players.OnClear += Clear;

            Refresh();
        }

        void Refresh()
        {
            Clear();

            for (int i = 0; i < Players.List.Count; i++)
            {
                var instance = Create(Players[i]);

                Elements.Add(instance);
            }
        }

        NetworkPlayersListElement Create(Photon.Realtime.Player player)
        {
            var instance = Instantiate(prefab, transform);

            var element = instance.GetComponent<NetworkPlayersListElement>();

            element.Init(player);

            return element;
        }

        void OnJoined(Photon.Realtime.Player player)
        {
            var instance = Create(player);

            Elements.Add(instance);
        }

        void OnLeft(Photon.Realtime.Player player)
        {
            var instance = Elements.Find((index) => index.Player == player);

            if(instance == null)
            {
                Debug.LogWarning("Trying to remove player " + player.NickName + " From Players List But That Player Was Never Assigned");
                return;
            }

            Destroy(instance.gameObject);

            Elements.Remove(instance);
        }

        void Clear()
        {
            for (int i = 0; i < Elements.Count; i++)
                Destroy(Elements[i].gameObject);

            Elements.Clear();
        }

        void OnDisable()
        {
            Players.OnJoined -= OnJoined;
            Players.OnLeft -= OnLeft;
            Players.OnClear -= Clear;
        }
	}
}