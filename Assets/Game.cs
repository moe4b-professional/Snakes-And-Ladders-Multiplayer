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
    [DefaultExecutionOrder(ExecutionOrder)]
	public class Game : MonoBehaviour
	{
        public const int ExecutionOrder = -200;

        public static Game Instance { get; protected set; }

		[SerializeField]
        protected PlayGrid grid;
        public PlayGrid Grid { get { return grid; } }

        [SerializeField]
        protected PlayersManager players;
        public PlayersManager Players { get { return players; } }

        [SerializeField]
        protected TurnsManager turns;
        public TurnsManager Turns { get { return turns; } }

        [SerializeField]
        protected Dice dice;
        public Dice Dice { get { return dice; } }

        void Awake()
        {
            Instance = this;

            StartCoroutine(Init());
        }

        IEnumerator Init()
        {
            yield return new WaitForEndOfFrame();

            grid.Init();

            players.Init();

            turns.Init();
        }
    }
}