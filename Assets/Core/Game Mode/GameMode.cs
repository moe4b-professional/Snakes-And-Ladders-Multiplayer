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
	public class GameMode : MonoBehaviour
	{
        public SingleplayerMode Singleplayer { get; protected set; }

        public void Init()
        {
            Singleplayer = Utility.GetDependancy<SingleplayerMode>();

            Singleplayer.Init(this);
        }

		public class Module : MonoBehaviour
        {
            public Core Core { get { return Core.Instance; } }

            public NetworkManager Network { get { return Core.Network; } }

            public PlayersManager Players { get { return Core.Players; } }

            public PlayGrid Grid { get { return Core.Grid; } }

            public GameMenu Menu { get { return Core.Menu; } }

            GameMode mode;
            public virtual void Init(GameMode mode)
            {
                this.mode = mode;
            }

            public virtual void Begin()
            {

            }
        }
	}
}