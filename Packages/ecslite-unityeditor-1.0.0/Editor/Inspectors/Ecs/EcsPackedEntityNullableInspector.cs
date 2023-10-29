using UnityEditor;
using UnityEngine;

namespace Leopotam.EcsLite.UnityEditor.Inspectors
{
    sealed class EcsPackedEntityNullableInspector : EcsComponentInspectorTyped<EcsPackedEntity?>
    {
        public override bool OnGuiTyped(string label, ref EcsPackedEntity? value, EcsEntityDebugView entityView)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            if (value.HasValue)
            {
                if (value.Value.Unpack(entityView.World, out var unpackedEntity))
                {
                    if (GUILayout.Button($"Ping {unpackedEntity}"))
                        EditorGUIUtility.PingObject(entityView.DebugSystem.GetEntityView(unpackedEntity));
                }
                else
                    EditorGUILayout.SelectableLabel("<Invalid entity>", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
            }
            else
                EditorGUILayout.SelectableLabel("<Empty entity>", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));

            EditorGUILayout.EndHorizontal();
            return false;
        }
    }
}