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
	public class NetworkPlayersListElement : MonoBehaviour
	{
        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

        public Photon.Realtime.Player Player { get; protected set; }

        public void Init(Photon.Realtime.Player player)
        {
            this.Player = player;

            label.text = Player.NickName;
        }
	}
}