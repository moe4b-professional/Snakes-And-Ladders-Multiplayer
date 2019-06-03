﻿using System;
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
    [DefaultExecutionOrder(ExecutionOrder)]
    public class Core : MonoBehaviour
    {
        public const int ExecutionOrder = -200;

        public static Core Instance { get; protected set; }

        public NetworkManager Network { get; protected set; }

        public PlayGrid Grid { get; protected set; }

        public PlayersManager Players { get; protected set; }

        public TurnsManager Turns { get; protected set; }

        public GameMenu Menu { get; protected set; }
        public Dice Dice { get { return Menu.Dice; } }
        public Popup Popup { get { return Menu.Popup; } }

        public GameMode Mode { get; protected set; }

        void Awake()
        {
            Instance = this;

            Network = Utility.GetDependancy<NetworkManager>();
            Grid = Utility.GetDependancy<PlayGrid>();
            Players = Utility.GetDependancy<PlayersManager>();
            Turns = Utility.GetDependancy<TurnsManager>();
            Menu = Utility.GetDependancy<GameMenu>();
            Mode = Utility.GetDependancy<GameMode>();
        }

        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            Network.Init();
            Grid.Init();
            Players.Init();
            Turns.Init();
            Menu.Init();
            Mode.Init();
        }

        void OnApplicationQuit()
        {
            Network.Stop();
        }

        public static class PlayerName
        {
            public static string Default { get { return "Player " + Random.Range(1, 999).ToString(); } }

            public static string PrefKey { get { return nameof(PlayerName); } }

            public static string Value
            {
                get
                {
                    return PlayerPrefs.GetString(PrefKey, Default);
                }
                set
                {
                    PlayerPrefs.SetString(PrefKey, value);

                    if (OnChange != null) OnChange(Value);
                }
            }

            public static event Action<string> OnChange;
        }
    }
}