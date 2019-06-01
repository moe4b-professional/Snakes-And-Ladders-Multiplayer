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
        protected virtual void InitConnectionPrefab()
        {
            var instance = Instantiate(connectionPrefab, transform, true);

            var line = instance.GetComponent<LineRenderer>();

            line.startColor = line.endColor = Grid.Colors.Transitions.Get(Direction);

            line.SetPosition(0, element.Position);
            line.SetPosition(1, target.Position);
        }

        public PlayGridElementTransitionDirection Direction { get; protected set; }
        protected virtual void InitDirection()
        {
            if(target.Index > element.Index)
            {
                Direction = PlayGridElementTransitionDirection.Up;
            }
            else if(target.Index < element.Index)
            {
                Direction = PlayGridElementTransitionDirection.Down;
            }
            else
            {
                Direction = PlayGridElementTransitionDirection.Neutral;
            }
        }

        PlayGridElement element;
        public PlayGrid Grid { get { return element.Grid; } }

        public virtual void Init(PlayGridElement element)
        {
            this.element = element;

            if (target == null)
                throw new NullReferenceException("No Target specified for transition: " + this.name);

            InitDirection();

            InitConnectionPrefab();
        }

        protected virtual void OnDrawGizmos()
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