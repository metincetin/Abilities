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

            DrawAttributes(t);
            DrawActiveAbilities(t);
            DrawStackTree(t);


        }

        private void DrawActiveAbilities(AbilityComponent t)
        {
            EditorGUILayout.LabelField("Active Abilities");

            EditorGUI.indentLevel++;

            var type = t.GetType();
            var field = type.GetField("_activeAbilities", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var abilityList = field.GetValue(t) as List<Ability>;
            EditorGUILayout.LabelField($"Active Abilities: {abilityList.Count}");

            foreach(var ability in abilityList)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(ability.name);

                var cooldown = t.GetCooldown(ability.Template);
                if (cooldown > 0)
                {
                    EditorGUILayout.LabelField(cooldown.ToString());
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        private void DrawStackTree(AbilityComponent t)
        {
            EditorGUILayout.LabelField("Stack Tree");
            EditorGUILayout.LabelField($"Active Effects: {t.StackTree.ActiveEffectCount}");
            EditorGUI.indentLevel++;
            foreach (Effect effect in t.StackTree)
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

        private void DrawAttributes(AbilityComponent t)
        {
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
        }
    }
}
