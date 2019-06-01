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

using UnityEngine.EventSystems;

namespace Game
{
    public class Dice : MonoBehaviour, IPointerClickHandler
    {
        public const int Max = 6;

        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

        public void OnPointerClick(PointerEventData eventData)
        {
            Roll();
        }

        public event Action<int> OnRoll;
        public void Roll()
        {
            var value = Random.Range(1, Max + 1);

            label.text = value.ToString();

            if (OnRoll != null) OnRoll(value);
        }
    }
}