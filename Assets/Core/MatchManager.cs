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
	public class MatchManager : MonoBehaviourPun
    {
        public Core Core { get { return Core.Instance; } }

        public bool Active { get; protected set; }

		public void Init()
        {
            Active = false;
        }

        public void Begin()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            photonView.RPC(nameof(BeginRPC), RpcTarget.AllBuffered);
        }
        public event Action OnBegin;
        [PunRPC]
        void BeginRPC()
        {
            Active = true;

            Core.Dice.Show();

            if (OnBegin != null) OnBegin();
        }

        public void End(Pawn winner)
        {
            photonView.RPC(nameof(EndRPC), RpcTarget.AllBuffered, winner.ID);
        }
        public event Action<Pawn> OnEnd;
        [PunRPC]
        void EndRPC(int winnerID)
        {
            var winner = Core.Pawns.Get(winnerID);

            ProcessWin(winner);

            Active = false;

            if (OnEnd != null) OnEnd(winner);
        }

        void ProcessWin(Pawn winner)
        {
            var text = "";

            if (Core.Mode.Singleplayer.Active)
                text += "You " + ((winner.Controller is PawnAIController) ? "Lost" : "Won");
            else if (Core.Mode.Versus.Active)
                text += winner.name + " Wins";
            else
                text += "You " + (Core.Pawns.IsLocal(winner) ? "Won" : "Lost");

            Core.Popup.Show(text, Core.Reload, "Reload");
        }

        void OnDestroy()
        {
            OnBegin = null;
            OnEnd = null;
        }
    }
}