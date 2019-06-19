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
	public class PawnPlayerController : Pawn.ControllerModule
    {
        public override string name
        {
            get
            {
                if (Mode.Current == Mode.Singleplayer)
                    return "Local Player";
                else if(Mode.Current == Mode.Versus)
                    return "Player " + (Manager.List.IndexOf(Pawn) + 1).ToString();
                else
                    return Pawn.Owner.NickName;
            }
        }

        [SerializeField]
        protected GameObject model1;
        public GameObject Model1 { get { return model1; } }

        [SerializeField]
        protected GameObject model2;
        public GameObject Model2 { get { return model2; } }

        protected virtual void SetModels()
        {
            if(Mode.Current == Mode.Versus)
            {
                Model1.SetActive(Manager.List.IndexOf(Pawn) == 0);
                Model2.SetActive(Manager.List.IndexOf(Pawn) == 1);
            }
            else
            {
                Model1.SetActive(photonView.IsMine);
                Model2.SetActive(!photonView.IsMine);
            }
        }

        public override void Init(Pawn reference)
        {
            base.Init(reference);

            Turns.OnTurnInitiation += OnTurnInitiated;
        }

        void Start()
        {
            SetModels();
        }

        void OnTurnInitiated(Pawn turnPawn)
        {
            if (turnPawn == Pawn)
            {
                Dice.OnRoll += OnDiceRoll;

                turnTimerCoroutine = StartCoroutine(TurnTimerProcedure());
            }
        }

        Coroutine turnTimerCoroutine;
        public const float TurnTimerDuration = 5f;
        IEnumerator TurnTimerProcedure()
        {
            var timer = TurnTimerDuration;

            while(timer != 0f)
            {
                timer = Mathf.MoveTowards(timer, 0f, Time.deltaTime);

                Dice.Progress.Value = timer / TurnTimerDuration;

                yield return new WaitForEndOfFrame();
            }

            turnTimerCoroutine = null;

            Core.Dice.Roll();
        }

        void OnDiceRoll(int roll)
        {
            Dice.OnRoll -= OnDiceRoll;

            if (turnTimerCoroutine != null) StopCoroutine(turnTimerCoroutine);

            Dice.Progress.Value = 0f;

            Turns.Roll(Pawn, roll);
        }

        void OnDestroy()
        {
            Turns.OnTurnInitiation -= OnTurnInitiated;

            Dice.OnRoll -= OnDiceRoll;
        }
    }
}