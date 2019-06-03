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

        public NetworkPlayers NetworkPlayers { get { return Core.Network.Players; } }
        public PlayersManager Players { get { return Core.Players; } }

        public int PlayerIndex { get; protected set; }
        public int ClampToPlayerIndex(int value)
        {
            if (value >= Players.Count) value = 0;

            return value;
        }

        public void Init()
        {
            PlayerIndex = 0;

            coroutines = new Coroutines(this);

            Core.Dice.OnRoll += OnDiceRoll;
            Network.OnBeginMatch += OnBeginMatch;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W) && PhotonNetwork.IsMasterClient)
            {
                Players.Local.Sync(95);

                OnDiceRoll(99 - Players.Local.Progress);
            }
        }

        void OnBeginMatch()
        {
            if(PhotonNetwork.IsMasterClient)
                photonView.RPC(nameof(Setup), RpcTarget.All, NetworkPlayers[PlayerIndex]);
        }

        [PunRPC]
        void Setup(Photon.Realtime.Player firstPlayer)
        {
            Core.Dice.Interactable = firstPlayer == PhotonNetwork.LocalPlayer;
        }

        void OnDiceRoll(int roll)
        {
            Core.Dice.Interactable = false;

            photonView.RPC(nameof(DiceRoll), RpcTarget.All, roll);
        }

        [PunRPC]
        void DiceRoll(int roll, PhotonMessageInfo msgInfo)
        {
            Core.Dice.Text = roll.ToString();

            if (PhotonNetwork.IsMasterClient)
            {
                if (InTurn)
                {
                    Debug.LogWarning("Player: " + msgInfo.Sender.NickName + " Threw dice mid-turn, ignoring");
                    return;
                }

                if (msgInfo.Sender != null && msgInfo.Sender != NetworkPlayers[PlayerIndex])
                {
                    Debug.LogWarning("Player: " + msgInfo.Sender.NickName + " Threw dice when it's not their turn, ignoring");
                    return;
                }

                var player = Players.Get(NetworkPlayers[PlayerIndex]);

                photonView.RPC(nameof(TurnStart), RpcTarget.All, player.Owner, player.Progress, roll);
            }
        }

        [PunRPC]
        void TurnStart(Photon.Realtime.Player networkPlayer, int progress, int roll)
        {
            var player = Players.Get(networkPlayer);

            if (PhotonNetwork.IsMasterClient)
            {

            }
            else
            {
                player.Sync(progress);
            }

            var target = Grid.Get(Grid[progress], roll);

            coroutines.Start(Procedure(player, Grid[progress], target));
        }

        Coroutines coroutines;
        public bool InTurn { get { return coroutines.Count > 0; } }
        IEnumerator Procedure(Player player, PlayGridElement current, PlayGridElement target)
        {
            if(target == null)
            {
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                yield return coroutines.Yield(Move(player, target));
            }

            if (PhotonNetwork.IsMasterClient)
                photonView.RPC(nameof(TurnEnd), RpcTarget.All, NetworkPlayers[PlayerIndex], NetworkPlayers[ClampToPlayerIndex(PlayerIndex + 1)], player.Progress);
        }

        IEnumerator Move(Player player, PlayGridElement target)
        {
            yield return coroutines.Yield(player.Move(target));

            while (player.CurrentElement.Transition != null)
                yield return coroutines.Yield(player.Transition(player.CurrentElement.Transition));
        }

        [PunRPC]
        void TurnEnd(Photon.Realtime.Player currentNetworkPlayer, Photon.Realtime.Player nextNetworkPlayer, int progress)
        {
            coroutines.StopAll();

            var currentPlayer = Players.Get(currentNetworkPlayer);
            var nextPlayer = Players.Get(nextNetworkPlayer);

            if (PhotonNetwork.IsMasterClient)
            {
                if (currentPlayer.Progress == 99)
                    Network.EndMatch(currentNetworkPlayer);

                PlayerIndex = ClampToPlayerIndex(PlayerIndex + 1);
            }
            else
                currentPlayer.Sync(progress);

            Core.Dice.Interactable = nextNetworkPlayer == PhotonNetwork.LocalPlayer;

            currentPlayer.Land();
        }
    }
}