using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        GameEvent _GE = target as GameEvent;

        if (GUILayout.Button("Raise"))
        {
            _GE.Raise();
        }
    }
}

//References https://youtu.be/raQ3iHhE_Kk?t=1964 , https://github.com/NeoDragonCP/Unity-ScriptableObjects-Game-Events-
