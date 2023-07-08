using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Abilities.Editor
{
	[CustomEditor(typeof(AbilityComponent))]
	public class AbilityComponentEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			var t = target as AbilityComponent;

      if (!t.AttributeSet)
      {
                var rect = EditorGUILayout.GetControlRect();
                EditorGUI.HelpBox(rect, "Debugging is disabled for this component as AttributeSet is null", MessageType.Info);
                return;
      }


			EditorGUILayout.LabelField("Attributes");
			EditorGUI.indentLevel++;

			foreach (Attribute attr in t.AttributeSet)
			{
				var val = attr.GetType().GetProperty("Value");
				if (val != null)
				{
					EditorGUILayout.BeginHorizontal();

					var attributeValue = val.GetValue(attr);

					EditorGUILayout.PrefixLabel(attr.name);
					EditorGUILayout.LabelField(attributeValue != null ? attributeValue.ToString() : "Invalid Value");


					EditorGUILayout.EndHorizontal();

				}
			}
			EditorGUI.indentLevel--;


			EditorGUILayout.LabelField("Stack Tree");
			EditorGUILayout.LabelField($"Active Effects: {t.StackTree.ActiveEffectCount}");
			EditorGUI.indentLevel++;
			foreach(Effect effect in t.StackTree)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel(effect.name);

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(effect.DurationType.ToString(), GUILayout.Width(64));
				EditorGUILayout.LabelField($"{(effect.Duration - effect.Time).ToString("0.00")}/{effect.Duration}");
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUI.indentLevel--;
		}
	}
}
