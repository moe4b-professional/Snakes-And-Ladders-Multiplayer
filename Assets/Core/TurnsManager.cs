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
	public class TurnsManager : MonoBehaviourPun
	{
        public Core Core { get { return Core.Instance; } }

        public NetworkManager Network { get { return Core.Network; } }

        public PlayGrid Grid { get { return Core.Grid; } }

        public PlayersManager Players { get { return Core.Players; } }

        int turnIndex;
        public Player CurrentPlayer { get { return Players[turnIndex]; } }

        public void Init()
        {
            turnIndex = 0;

            Network.OnBeginMatch += OnBeginMatch;
        }

        void OnBeginMatch()
        {
            Core.Dice.OnRoll += OnDiceRoll;
        }

        void OnDiceRoll(int value)
        {
            if (InTurn) return;

            if (turnIndex >= Players.Count)
                turnIndex = 0;

            coroutine = StartCoroutine(Process(Players[turnIndex], value));
        }

        Coroutine coroutine;
        public bool InTurn { get { return coroutine != null; } }
        IEnumerator Process(Player player, int roll)
        {
            var target = Grid.Get(player.CurrentElement, roll);

            if(target == null)
            {

            }
            else
            {
                photonView.RPC(nameof(Sync), RpcTarget.Others, player.Owner, target.Index);

                yield return Move(player, target);
            }

            if(player.CurrentElement == Grid.Elements.Last())
            {
                Core.Popup.Show("You Won", Utility.ReloadScene, "Reload");
            }

            player.Sync();

            turnIndex++;
            if (turnIndex >= Players.Count)
                turnIndex = 0;

            Core.Dice.Set(CurrentPlayer);

            coroutine = null;
            yield break;
        }

        IEnumerator Move(Player player, PlayGridElement target)
        {
            yield return player.Move(target);

            while (player.CurrentElement.Transition != null)
                yield return player.Transition(player.CurrentElement.Transition);

            player.Land();
        }

        [PunRPC]
        void Sync(Photon.Realtime.Player networkPlayer, int index)
        {
            var player = Core.Players.List.Find(x => x.Owner == networkPlayer);

            StartCoroutine(Move(player, Grid[index]));
        }
    }
}