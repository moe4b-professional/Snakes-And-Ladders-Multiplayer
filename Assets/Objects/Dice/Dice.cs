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

using Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Game
{
    [RequireComponent(typeof(Button))]
    public class Dice : UIElement
    {
        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

        [SerializeField]
        protected ProgressController progress;
        public ProgressController Progress { get { return progress; } }
        [Serializable]
        public class ProgressController
        {
            [SerializeField]
            protected Image image;
            public Image Image { get { return image; } }

            public float Value
            {
                get
                {
                    return image.fillAmount;
                }
                set
                {
                    image.fillAmount = value;
                }
            }
        }

        [SerializeField]
        protected Button button;
        public Button Button { get { return button; } }
        public bool Interactable
        {
            set
            {
                Button.interactable = value;
            }
        }

        protected int _value;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                value = Mathf.Clamp(value, 0, MaxValue);

                this._value = value;

                label.text = Value.ToString();
            }
        }

        public const int MaxValue = 6;

        public Core Core { get { return Core.Instance; } }

        public PhotonView View { get; protected set; }

        void Awake()
        {
            Button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            Roll();
        }
        
        public event Action<int> OnRoll;
        public void Roll()
        {
            Value = Random.Range(1, MaxValue + 1);

            if (OnRoll != null) OnRoll(Value);
        }

        void OnDestroy()
        {
            OnRoll = null;
        }
    }
}