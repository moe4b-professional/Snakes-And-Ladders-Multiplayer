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
        [SerializeField]
        protected GameObject prefab;
        public GameObject Prefab { get { return prefab; } }

        public List<Player> List { get; protected set; }

        public Player this[int index]
        {
            get
            {
                return List[index];
            }
        }

        public void Init()
        {
            List = new List<Player>();
        }

        public virtual Player Spawn(PlayGridElement element)
        {
            if (element == null) throw new NullReferenceException();

            var instance = PhotonNetwork.Instantiate(prefab.name, element.Position, Quaternion.identity);

            var player = instance.GetComponent<Player>();
            player.Init(element);

            List.Add(player);

            return player;
        }
    }
}