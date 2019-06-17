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
	public class PawnAIController : Pawn.ControllerModule
	{
        public override string name { get { return "AI"; } }

        public override void Init(Pawn reference)
        {
            base.Init(reference);

            Turns.OnTurnInitiation += OnTurnInitiated;
        }

        void OnTurnInitiated(Pawn turnPlayer)
        {
            if(Pawn == turnPlayer)
            {
                Dice.Roll();

                Turns.Roll(Pawn, Dice.Value);
            }
        }

        void OnDestroy()
        {
            Turns.OnTurnInitiation -= OnTurnInitiated;
        }
    }
}