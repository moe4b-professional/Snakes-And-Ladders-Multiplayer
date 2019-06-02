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
        public Core Core { get { return Core.Instance; } }

        public PlayGrid Grid { get { return Core.Grid; } }

        public PlayersManager Players { get { return Core.Players; } }

        public void Init()
        {
            Core.Dice.OnRoll += OnDiceRoll;
        }

        void OnDiceRoll(int value)
        {
            if (InTurn) return;

            coroutine = StartCoroutine(Process(Players[0], value));
        }

        Coroutine coroutine;
        public bool InTurn { get { return coroutine != null; } }
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

            if(player.CurrentElement == Grid.Elements.Last())
            {
                Debug.Log(player.name + " Won");

                Core.Popup.Show("You Won", Utility.ReloadScene, "Reload");
            }

            coroutine = null;
            yield break;
        }
    }
}