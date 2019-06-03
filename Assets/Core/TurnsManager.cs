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

            coroutines = new CoroutineList(this);

            Network.OnBeginMatch += OnBeginMatch;
        }

        void OnBeginMatch()
        {
            Core.Dice.OnRoll += OnDiceRoll;

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
            Debug.Log(PhotonNetwork.IsMasterClient);

            if (!PhotonNetwork.IsMasterClient) return;

            Debug.Log("This Shouldn't Work");

            if (InTurn) return;

            var player = Players.Get(NetworkPlayers[PlayerIndex]);

            //photonView.RPC(nameof(TurnStart), RpcTarget.All, player.Owner, player.Progress, roll);

            Core.Dice.Interactable = false;
        }

        [PunRPC]
        void TurnStart(Photon.Realtime.Player networkPlayer, int progress, int roll)
        {
            var player = Players.Get(networkPlayer);

            var target = Grid.Get(player.CurrentElement, roll);

            coroutines.Start(Procedure(player, Grid[progress], target));
        }

        CoroutineList coroutines;
        public bool InTurn { get { return coroutines.Count > 0; } }
        IEnumerator Procedure(Player player, PlayGridElement current, PlayGridElement target)
        {
            if(target == null)
            {

            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    
                }
                else
                {
                    player.Sync(current.Index);
                }

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
                PlayerIndex = ClampToPlayerIndex(PlayerIndex + 1);
            else
                currentPlayer.Sync(progress);

            Core.Dice.Interactable = nextNetworkPlayer == PhotonNetwork.LocalPlayer;

            currentPlayer.Land();
        }
    }

    public class CoroutineList
    {
        public MonoBehaviour Behaviour { get; protected set; }

        public List<Data> List { get; protected set; }
        [Serializable]
        public class Data
        {
            public Coroutine Source { get; protected set; }

            public Coroutine Wrapper { get; protected set; }

            public void Stop(MonoBehaviour behaviour)
            {
                if(Wrapper != null)
                    behaviour.StopCoroutine(Wrapper);

                if (Source != null)
                    behaviour.StopCoroutine(Source);
            }

            public Data(IEnumerator source, Func<Data, IEnumerator> wrapper, MonoBehaviour behaviour)
            {
                this.Source = behaviour.StartCoroutine(source);
                this.Wrapper = behaviour.StartCoroutine(wrapper(this));
            }
        }

        public int Count { get { return List.Count; } }

        public Data Start(IEnumerator procedure)
        {
            var data = new Data(procedure, Wrapper, Behaviour);

            List.Add(data);

            return data;
        }
        public Coroutine Yield(IEnumerator procedure)
        {
            var data = Start(procedure);

            return data.Wrapper;
        }

        IEnumerator Wrapper(Data data)
        {
            yield return data.Source;

            List.Remove(data);
        }

        public void StopAll()
        {
            for (int i = 0; i < List.Count; i++)
                List[i].Stop(Behaviour);

            List.Clear();
        }

        public CoroutineList(MonoBehaviour behaiour)
        {
            this.Behaviour = behaiour;

            List = new List<Data>();
        }
    }
}