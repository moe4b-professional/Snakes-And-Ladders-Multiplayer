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
    [RequireComponent(typeof(PhotonView))]
	public class MultiplayerMode : GameMode.Module
	{
        public const int MaxPlayers = 4;

        public MultiplayerModeEntry Entry { get; protected set; }

        public PhotonView View { get; protected set; }

        public override void Init()
        {
            base.Init();

            View = GetComponent<PhotonView>();

            Entry = GetComponent<MultiplayerModeEntry>();

            Entry.Init();
        }

        public override void Begin()
        {
            base.Begin();

            Entry.OnEnd += OnEntryEnd;
            Entry.Begin();
        }

        void OnEntryEnd()
        {
            Menu.Multiplayer.Hide();

            Menu.Room.Show();

            Network.OnBeginMatch += OnBeginMatch;
        }

        void OnBeginMatch()
        {
            Network.OnBeginMatch -= OnBeginMatch;

            Core.Players.Spawn();

            Menu.Room.Hide();

            Menu.Dice.Show();
        }

        public class Module : MonoBehaviour
        {
            public Core Core { get { return Core.Instance; } }

            public NetworkManager Network { get { return Core.Network; } }

            public PlayersManager Players { get { return Core.Players; } }

            public PlayGrid Grid { get { return Core.Grid; } }

            public GameMenu Menu { get { return Core.Menu; } }

            public GameMode Mode { get { return Core.Mode; } }

            public MultiplayerMode Multiplayer { get { return Mode.Multiplayer; } }

            public virtual void Init()
            {

            }

            public virtual void Begin()
            {

            }

            public event Action OnEnd;
            protected virtual void End()
            {
                if (OnEnd != null) OnEnd();
            }
        }
    }
}