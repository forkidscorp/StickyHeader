using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LeaderboardItem))]
public class LeaderboardItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LeaderboardItem item = (LeaderboardItem)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);

        bool newIsMyPlayer = GUILayout.Toggle(item.IsMyPlayer, "Is My Player");

        if (newIsMyPlayer != item.IsMyPlayer)
        {
            SetThisAsMyPlayer(item, newIsMyPlayer);
        }
    }

    private void SetThisAsMyPlayer(LeaderboardItem selectedItem, bool newValue)
    {
        LeaderboardItem[] allItems = FindObjectsOfType<LeaderboardItem>();

        foreach (var item in allItems)
        {
            item.SetIsMyPlayer(item == selectedItem && newValue);
            EditorUtility.SetDirty(item);
        }
    }
}
