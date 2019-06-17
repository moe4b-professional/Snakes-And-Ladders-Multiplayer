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

using PunPlayer = Photon.Realtime.Player;

namespace Game
{
	public class PawnsManager : MonoBehaviour
	{
        public Core Core { get { return Core.Instance; } }

        public PlayGrid Grid { get { return Core.Grid; } }

        public List<Pawn> List { get; protected set; }

        public int Count { get { return List.Count; } }

        public Pawn this[int index] { get { return List[index]; } }

        public List<Pawn> Locals { get; protected set; }
        public bool IsLocal(Pawn player)
        {
            return Locals.Contains(player);
        }

        public event Action<Pawn> OnAdd;
        public void Add(Pawn player)
        {
            List.Add(player);

            if (player.Owner.IsLocal) Locals.Add(player);

            if (OnAdd != null) OnAdd(player);
        }

        public event Action<Pawn> OnRemove;
        public void Remove(Pawn player)
        {
            List.Remove(player);

            if (Locals.Contains(player)) Locals.Remove(player);

            if (OnRemove != null) OnRemove(player);
        }

        public Pawn Get(int ID)
        {
            for (int i = 0; i < List.Count; i++)
                if (List[i].ID == ID)
                    return List[i];

            return null;
        }

        public void Init()
        {
            List = new List<Pawn>();

            Locals = new List<Pawn>();
        }

        public virtual Pawn Spawn(string name)
        {
            var element = Grid[0];

            var instance = PhotonNetwork.Instantiate(name, element.Position, Quaternion.identity);

            var player = instance.GetComponent<Pawn>();

            player.SyncProgress(element.Index);

            return player;
        }
    }
}