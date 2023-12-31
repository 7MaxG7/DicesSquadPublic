// ----------------------------------------------------------------------------
// The MIT License
// UnityEditor integration https://github.com/Leopotam/ecslite-unityeditor
// for LeoECS Lite https://github.com/Leopotam/ecslite
// Copyright (c) 2021-2022 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Leopotam.EcsLite.UnityEditor.Inspectors
{
    sealed class EcsPackedEntityWithWorldInspector : EcsComponentInspectorTyped<EcsPackedEntityWithWorld>
    {
        public override bool OnGuiTyped(string label, ref EcsPackedEntityWithWorld value, EcsEntityDebugView entityView)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            if (value.Unpack(out var unpackedWorld, out var unpackedEntity))
                if (unpackedWorld == entityView.World)
                {
                    if (GUILayout.Button($"Ping {unpackedEntity}"))
                        EditorGUIUtility.PingObject(entityView.DebugSystem.GetEntityView(unpackedEntity));
                }
                else
                    EditorGUILayout.SelectableLabel("<External entity>", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
            else if (value.EqualsTo(default))
                EditorGUILayout.SelectableLabel("<Empty entity>", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
            else
                EditorGUILayout.SelectableLabel("<Invalid entity>", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));

            EditorGUILayout.EndHorizontal();
            return false;
        }
    }
}