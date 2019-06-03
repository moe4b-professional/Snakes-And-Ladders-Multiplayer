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
	public class Player : MonoBehaviourPun
	{
        public Core Core { get { return Core.Instance; } }

        public PlayGrid Grid { get { return Core.Grid; } }

        public PlayersManager Manager { get { return Core.Players; } }

		[SerializeField]
        protected float speed = 10f;
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

        public Photon.Realtime.Player Owner { get { return photonView.Owner; } }

        new public string name { get { return Owner.NickName; } }

        public void Init(PlayGridElement elment)
        {
            Progress = elment.Index;

            transform.position = Grid[Progress].Position;
        }

        void Awake()
        {
            Manager.Add(this);
        }

        void Start()
        {
            gameObject.name = name;
        }

        public IEnumerator Transition(PlayGridElementTransition transition)
        {
            yield return Move(transition.Target.Position);

            Progress = transition.Target.Index;

            yield return new WaitForSeconds(0.1f);
        }

        public IEnumerator Move(PlayGridElement target)
        {
            while(Progress != target.Index)
            {
                var direction = Math.Sign(target.Index - CurrentElement.Index);

                var nextElement = Grid[Progress + direction];

                yield return Move(nextElement.Position);

                Progress = nextElement.Index;

                yield return new WaitForSeconds(0.05f);
            }
        }

        public IEnumerator Move(Vector2 target)
        {
            while(Position != target)
            {
                Position = Vector2.MoveTowards(Position, target, speed * Time.deltaTime);

                yield return null;
            }
        }

        public void Sync()
        {
            photonView.RPC(nameof(SyncRPC), RpcTarget.Others, Progress);
        }
        [PunRPC]
        void SyncRPC(int snycProgress)
        {
            this.Progress = snycProgress;

            Position = Grid[Progress].Position;
        }

        void OnDestroy()
        {
            Manager.Remove(this);
        }
    }
}