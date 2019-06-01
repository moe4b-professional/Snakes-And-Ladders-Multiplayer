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
	public class PlayGrid : MonoBehaviour
	{
        [SerializeField]
        protected ColorsData colors = ColorsData.Default;
        public ColorsData Colors { get { return colors; } }
        [Serializable]
        public struct ColorsData
        {
            [SerializeField]
            Color[] elements;
            public Color[] Elements { get { return elements; } }
            public Color GetElement(int index)
            {
                return elements[index % elements.Length];
            }
            
            [SerializeField]
            TransitionsData transitions;
            public TransitionsData Transitions { get { return transitions; } }
            [Serializable]
            public struct TransitionsData
            {
                [SerializeField]
                Color up;
                public Color Up { get { return up; } }

                [SerializeField]
                Color down;
                public Color Down { get { return down; } }

                public Color Get(PlayGridElementTransitionDirection direction)
                {
                    switch (direction)
                    {
                        case PlayGridElementTransitionDirection.Down:
                            return down;

                        case PlayGridElementTransitionDirection.Up:
                            return up;

                        case PlayGridElementTransitionDirection.Neutral:
                            return up;
                    }

                    throw new NotImplementedException();
                }

                public static TransitionsData Defaults
                {
                    get
                    {
                        return new TransitionsData()
                        {
                            down = Color.red,
                            up = Color.green
                        };
                    }
                }
            }

            public static ColorsData Default
            {
                get
                {
                    return new ColorsData()
                    {
                        elements = new Color[] { Color.blue, Color.green, Color.red, Color.yellow },
                        transitions = TransitionsData.Defaults
                    };
                }
            }
        }

        public const int Size = 10;

        List<PlayGridElement> elements;
        void InitElements()
        {
            elements = GetComponentsInChildren<PlayGridElement>().ToList();

            //Reverse every second row
            for (int i = 0; i < elements.Count / Size; i++)
            {
                if (i == 0) continue;

                if (i % 2 != 0)
                    elements.Reverse(Size * i, Size);
            }

            for (int i = 0; i < elements.Count; i++)
            {
                var row = i / Size;
                var column = i - (row * Size);

                elements[i].Configure(this, i);
            }

            for (int i = 0; i < elements.Count; i++)
                elements[i].Init();
        }
        public PlayGridElement this[int index]
        {
            get
            {
                return elements[index];
            }
        }

        public bool Contains(int index)
        {
            if (index < 0 || index >= elements.Count)
                return false;
            else
                return true;
        }

        public PlayGridElement Get(PlayGridElement element, int offset)
        {
            if (Contains(element.Index + offset))
                return elements[element.Index + offset];
            else
                return null;
        }

        public void Init()
        {
            InitElements();
        }

        void RebuildLayout()
        {
            InitElements();
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(PlayGrid))]
        public class Inspector : Editor
        {
            new PlayGrid target;

            protected virtual void OnEnable()
            {
                this.target = base.target as PlayGrid;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                RebuildLayout();
            }

            protected virtual void RebuildLayout()
            {
                if (GUILayout.Button("Rebuild Layout"))
                    target.RebuildLayout();
            }
        }
#endif
    }
}