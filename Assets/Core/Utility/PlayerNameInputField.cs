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
    [RequireComponent(typeof(InputField))]
	public class PlayerNameInputField : MonoBehaviour
	{
		public InputField InputField { get; protected set; }

        void Awake()
        {
            InputField = GetComponent<InputField>();

            InputField.onValueChanged.AddListener(OnChange);
        }

        void OnEnable()
        {
            InputField.text = Core.PlayerName.Value;
        }

        void OnChange(string newValue)
        {
            Core.PlayerName.Value = newValue;

            if (Core.PlayerName.Value != InputField.text)
                InputField.text = Core.PlayerName.Value;
        }
    }
}