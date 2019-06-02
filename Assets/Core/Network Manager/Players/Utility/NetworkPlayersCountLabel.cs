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
    [RequireComponent(typeof(Text))]
	public class NetworkPlayersCountLabel : MonoBehaviour
	{
        [SerializeField]
        protected string prefix = "Players ";
        public string Prefix { get { return prefix; } }

        Text label;

        public NetworkPlayers Players { get { return Core.Instance.Network.Players; } }

        void Awake()
        {
            label = GetComponent<Text>();
        }

        void OnEnable()
        {
            UpdateState();

            Players.OnChange += OnChange;
        }

        void OnChange()
        {
            UpdateState();
        }

        void UpdateState()
        {
            label.text = prefix + "(" + Players.Count.ToString() + "/" + MultiplayerMode.MaxPlayers + ")";
        }

        void OnDisable()
        {
            Players.OnChange -= OnChange;
        }
	}
}