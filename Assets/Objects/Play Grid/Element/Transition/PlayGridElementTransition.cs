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
    [RequireComponent(typeof(PlayGridElement))]
	public class PlayGridElementTransition : MonoBehaviour
	{
        [SerializeField]
        protected PlayGridElement target;
        public PlayGridElement Target { get { return target; } }

        [SerializeField]
        protected GameObject connectionPrefab;
        public GameObject ConnectionPrefab { get { return connectionPrefab; } }
        void InstantiateConnection()
        {
            var instance = Instantiate(connectionPrefab, Grid.transform);

            instance.name = connectionPrefab.name;

            var connection = instance.GetComponent<PlayGridElementConnection>();

            connection.Connect(this);
        }

        public PlayGridElementTransitionDirection Direction { get; protected set; }
        void InitDirection()
        {
            if(target.Index > origin.Index)
            {
                Direction = PlayGridElementTransitionDirection.Up;
            }
            else if(target.Index < origin.Index)
            {
                Direction = PlayGridElementTransitionDirection.Down;
            }
            else
            {
                Direction = PlayGridElementTransitionDirection.Neutral;
            }
        }
        public Color Color { get { return Grid.Colors.Transitions.Get(Direction); } }

        PlayGridElement origin;
        public PlayGridElement Origin { get { return origin; } }
        public PlayGrid Grid { get { return origin.Grid; } }

        public void Init(PlayGridElement origin)
        {
            this.origin = origin;

            if (target == null)
                throw new NullReferenceException("No Target specified for transition: " + this.name);

            InitDirection();

            InstantiateConnection();
        }

        void OnDrawGizmos()
        {
            if(target != null)
            {
                Gizmos.color = Color.magenta;

                Gizmos.DrawLine(transform.position, target.transform.position);
            }
        }
    }

    public enum PlayGridElementTransitionDirection
    {
        Down, Neutral, Up
    }
}