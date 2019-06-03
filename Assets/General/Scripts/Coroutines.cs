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
    public class Coroutines
    {
        public MonoBehaviour Behaviour { get; protected set; }

        public List<Data> List { get; protected set; }
        [Serializable]
        public class Data
        {
            public Coroutine Source { get; protected set; }

            public Coroutine Wrapper { get; protected set; }

            public void Stop(MonoBehaviour behaviour)
            {
                if (Wrapper != null)
                    behaviour.StopCoroutine(Wrapper);

                if (Source != null)
                    behaviour.StopCoroutine(Source);
            }

            public Data(IEnumerator source, Func<Data, IEnumerator> wrapper, MonoBehaviour behaviour)
            {
                this.Source = behaviour.StartCoroutine(source);
                this.Wrapper = behaviour.StartCoroutine(wrapper(this));
            }
        }

        public int Count { get { return List.Count; } }

        public Data Start(IEnumerator procedure)
        {
            var data = new Data(procedure, Wrapper, Behaviour);

            List.Add(data);

            return data;
        }
        public Coroutine Yield(IEnumerator procedure)
        {
            var data = Start(procedure);

            return data.Wrapper;
        }

        IEnumerator Wrapper(Data data)
        {
            yield return data.Source;

            List.Remove(data);
        }

        public void StopAll()
        {
            for (int i = 0; i < List.Count; i++)
                List[i].Stop(Behaviour);

            List.Clear();
        }

        public Coroutines(MonoBehaviour behaiour)
        {
            this.Behaviour = behaiour;

            List = new List<Data>();
        }
    }
}