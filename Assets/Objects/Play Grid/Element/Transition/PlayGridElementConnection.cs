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
	public class PlayGridElementConnection : MonoBehaviour
	{
        public void Connect(PlayGridElementTransition transition)
        {
            foreach (var graphic in GetComponentsInChildren<Graphic>())
                graphic.color = graphic.color = transition.Color;

            RectTransform rect = transform as RectTransform;

            rect.position = transition.Origin.Position;

            var vector = transition.Target.Position - transition.Origin.Position;

            rect.sizeDelta = new Vector2(rect.sizeDelta.x, vector.magnitude / transform.parent.lossyScale.x);

            rect.eulerAngles = new Vector3(0f, 0f, -Utility.Vector2Angle(vector));
        }
	}
}