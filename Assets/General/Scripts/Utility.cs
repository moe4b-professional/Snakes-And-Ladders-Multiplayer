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
using System.Text;

namespace Game
{
	public static class Utility
	{
        public static StringBuilder StringBuilder { get; private set; } = new StringBuilder();

        public static class RichText
        {
            public static string Format(string text, string code, object value)
            {
                return ("<" + code + (value == null ? "" : "=" + value.ToString()) + ">") + text + ("</" + code + ">");
            }
            public static string Format(string text, string code)
            {
                return Format(text, code, null);
            }

            public static string Bold(string text)
            {
                return Format(text, "b");
            }

            public static string Italics(string text)
            {
                return Format(text, "i");
            }

            public static string Size(string text, int value)
            {
                return Format(text, "size", value);
            }

            public static string Color(string text, string value)
            {
                return Format(text, "color", value);
            }
        }

        public static T GetDependancy<T>()
                    where T : Object
        {
            var dependancy = Object.FindObjectOfType<T>();

            if (dependancy == null)
                throw new NullReferenceException("Missing Dependancy: " + typeof(T).Name);

            return dependancy;
        }

        public static float Vector2Angle(Vector2 vector)
        {
            return Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
        }

        public static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public static string FormatCaps(string text)
        {
            StringBuilder.Clear();

            for (int i = 0; i < text.Length; i++)
            {
                StringBuilder.Append(text[i]);

                if (i == text.Length - 1) break;

                if (!char.IsUpper(text[i]) && char.IsUpper(text[i + 1]))
                    StringBuilder.Append(' ');
            }

            return StringBuilder.ToString();
        }
    }
}