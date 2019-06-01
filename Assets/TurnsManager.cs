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
	public class TurnsManager : MonoBehaviour
	{
        public Game Game { get { return Game.Instance; } }

        public PlayGrid Grid { get { return Game.Grid; } }

        public PlayersManager Players { get { return Game.Players; } }

        public void Init()
        {
            Game.Dice.OnRoll += OnDiceRoll;
        }

        void OnDiceRoll(int value)
        {
            if (InTurn) return;

            coroutine = StartCoroutine(Process(Players[0], value));
        }

        Coroutine coroutine;
        public virtual bool InTurn { get { return coroutine != null; } }
        IEnumerator Process(Player player, int roll)
        {
            var targetElement = Grid.Get(player.CurrentElement, roll);

            if(targetElement == null)
            {

            }
            else
            {
                yield return player.Move(targetElement);

                while(player.CurrentElement.Transition != null)
                    yield return player.Transition(player.CurrentElement.Transition);

                player.Land();
            }

            coroutine = null;
            yield break;
        }
    }
}