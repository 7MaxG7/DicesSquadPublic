using CustomTypes;
using UnityEditor;
using UnityEngine;

namespace Leopotam.EcsLite.UnityEditor.Inspectors
{
    sealed class UnitSpecializationInspector : EcsComponentInspectorTyped<UnitSpecialization>
    {
        public override bool OnGuiTyped(string label, ref UnitSpecialization value, EcsEntityDebugView entityView)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            
            EditorGUILayout.SelectableLabel(value.ToString(), GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
            
            EditorGUILayout.EndHorizontal();
            return false;
        }
    }
}