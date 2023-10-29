using CustomTypes;
using UnityEditor;
using UnityEngine;

namespace Leopotam.EcsLite.UnityEditor.Inspectors
{
    sealed class BattleCellInspector : EcsComponentInspectorTyped<BattleCell>
    {
        public override bool OnGuiTyped(string label, ref BattleCell value, EcsEntityDebugView entityView)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            
            EditorGUILayout.SelectableLabel(value.ToString(), GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
            
            EditorGUILayout.EndHorizontal();
            return false;
        }
    }
}