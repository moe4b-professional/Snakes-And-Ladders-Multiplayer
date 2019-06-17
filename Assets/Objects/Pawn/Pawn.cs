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
    public class Pawn : MonoBehaviourPun
    {
        public const string Player = "Player-Pawn";
        public const string AI = "AI-Pawn";

        [SerializeField]
        protected float speed = 5f;
        public float Speed { get { return speed; } }

        public Vector2 Position
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = new Vector3(value.x, value.y, transform.position.z);
            }
        }

        public int Progress { get; protected set; }
        public PlayGridElement CurrentElement { get { return Grid[Progress]; } }
        public void Land()
        {
            CurrentElement.Land(this);
        }

        public int ID { get { return photonView.ViewID; } }
        public Player Owner { get { return photonView.Owner; } }

        public ControllerModule Controller { get; protected set; }
        public abstract class ControllerModule : MonoBehaviourPun
        {
            new public abstract string name { get; }

            public Pawn Pawn { get; protected set; }
            public virtual void Init(Pawn reference)
            {
                Pawn = reference;
            }

            public Core Core { get { return Core.Instance; } }

            public NetworkManager Network { get { return Core.Network; } }

            public PawnsManager Manager { get { return Core.Pawns; } }

            public Dice Dice { get { return Core.Dice; } }

            public PlayGrid Grid { get { return Core.Grid; } }

            public GameMenu Menu { get { return Core.Menu; } }

            public GameMode Mode { get { return Core.Mode; } }

            public TurnsManager Turns { get { return Core.Turns; } }
        }

        new public string name { get { return Controller.name; } }

        public Core Core { get { return Core.Instance; } }
        public PlayGrid Grid { get { return Core.Grid; } }
        public PawnsManager Manager { get { return Core.Pawns; } }
        public TurnsManager Turns { get { return Core.Turns; } }

        void Awake()
        {
            Controller = GetComponent<ControllerModule>();
            Controller.Init(this);

            Manager.Add(this);
        }

        void Start()
        {
            gameObject.name = name;
        }

        public IEnumerator Transition(PlayGridElementTransition transition)
        {
            yield return MoveTo(transition.Target.Position);

            Progress = transition.Target.Index;

            yield return new WaitForSeconds(0.1f);
        }

        public IEnumerator Move(PlayGridElement target)
        {
            while(Progress != target.Index)
            {
                var direction = Math.Sign(target.Index - CurrentElement.Index);

                var nextElement = Grid[Progress + direction];

                yield return MoveTo(nextElement.Position);

                Progress = nextElement.Index;

                yield return new WaitForSeconds(0.05f);
            }
        }

        public IEnumerator MoveTo(Vector2 target)
        {
            while(Position != target)
            {
                Position = Vector2.MoveTowards(Position, target, speed * Time.deltaTime);

                yield return null;
            }
        }

        public void SyncProgress(int index)
        {
            this.Progress = index;

            Position = Grid[Progress].Position;
        }

        void OnDestroy()
        {
            Manager.Remove(this);
        }
    }
}