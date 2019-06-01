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
    [SelectionBase]
	public class PlayGridElement : MonoBehaviour
	{
        [SerializeField]
        protected Image background;
        public Image Background { get { return background; } }

        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

        PlayGrid grid;
        public PlayGrid Grid { get { return grid; } }

        int index;
        public int Index { get { return index; } }

        public Vector2 Position { get { return transform.position; } }

        public PlayGridElementTransition Transition { get; protected set; }
        protected virtual void InitTransition()
        {
            Transition = GetComponent<PlayGridElementTransition>();

            if(Transition != null) Transition.Init(this);
        }

        public void Configure(PlayGrid grid, int index)
        {
            this.grid = grid;
            this.index = index;
        }

        public void Init()
        {
            InitTransition();

            gameObject.name = "Element " + (index + 1).ToString(); 
            label.text = (index + 1).ToString();
            background.color = grid.Colors.GetElement(index);
        }

        public event Action<Player> OnLanded;
        public void Land(Player player)
        {
            if (OnLanded != null) OnLanded(player);
        }
    }
}