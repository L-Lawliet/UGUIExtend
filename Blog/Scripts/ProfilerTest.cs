using UnityEngine;
using System.Collections;
using Waiting.UGUI.Effects;
using System.Collections.Generic;

public class ProfilerTest : MonoBehaviour {

    public Mirror mirror;

    private List<Mirror> _list = new List<Mirror>();

    void OnGUI()
    {
        if (GUILayout.Button("Copy"))
        {
            for (int i = 0; i < 100; i++)
            {
                Mirror copy = Instantiate<Mirror>(mirror);

                copy.transform.SetParent(mirror.transform.parent, false);

                float x = Random.Range(-960, 960);

                float y = Random.Range(-540, 540);

                copy.rectTransform.anchoredPosition = new Vector2(x, y);

                _list.Add(copy);
            }
        }

        if(GUILayout.Button("Enabled"))
        {
            for (int i = 0; i < _list.Count; i++)
            {
                Mirror mirror = _list[i];

                Profiler.BeginSample("#Enabled");

                mirror.enabled = true;

                Profiler.EndSample();
            }
            
        }

        if (GUILayout.Button("Disabled"))
        {
            for (int i = 0; i < _list.Count; i++)
            {
                Mirror mirror = _list[i];

                Profiler.BeginSample("#Disabled");

                mirror.enabled = false;

                Profiler.EndSample();
            }
        }
    }
}
