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
	public class PlayersManager : MonoBehaviour
	{
        public Core Core { get { return Core.Instance; } }

        public PlayGrid Grid { get { return Core.Grid; } }

        [SerializeField]
        protected GameObject prefab;
        public GameObject Prefab { get { return prefab; } }

        public Player Local { get; protected set; }

        public List<Player> List { get; protected set; }

        public int Count { get { return List.Count; } }

        public Player this[int index]
        {
            get
            {
                return List[index];
            }
        }

        public void Add(Player player)
        {
            List.Add(player);

            if (player.Owner.IsLocal)
                Local = player;
        }

        public void Remove(Player player)
        {
            List.Remove(player);

            if (player == Local)
                Local = null;
        }

        public Player Get(Photon.Realtime.Player networkPlayer)
        {
            for (int i = 0; i < List.Count; i++)
                if (List[i].Owner == networkPlayer)
                    return List[i];

            return null;
        }

        public void Init()
        {
            List = new List<Player>();
        }

        public void Spawn()
        {
            Local = Spawn(Grid[0]);
        }
        public virtual Player Spawn(PlayGridElement element)
        {
            if (element == null) throw new NullReferenceException();

            var instance = PhotonNetwork.Instantiate(prefab.name, element.Position, Quaternion.identity);

            var player = instance.GetComponent<Player>();
            player.Init(element);

            return player;
        }
    }
}