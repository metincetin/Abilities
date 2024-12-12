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
            DrawTags(t);
        }

        private void DrawActiveAbilities(AbilityComponent t)
        {
            DrawHeader("Active Abilities");

            EditorGUI.indentLevel++;

            var type = t.GetType();
            var field = type.GetField("_activeAbilities", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var abilityList = field.GetValue(t) as List<Ability>;
            EditorGUILayout.LabelField($"Active Abilities: {abilityList.Count}");

            foreach(var ability in abilityList)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(ability.name);

                var cooldown = t.GetCooldownRemaining(ability.Template);
                if (cooldown > 0)
                {
                    EditorGUILayout.LabelField(cooldown.ToString());
                }

                if (GUILayout.Button("End"))
                {
                    ability.End();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        private void DrawStackTree(AbilityComponent t)
        {
            DrawHeader("Stack Tree");

            EditorGUILayout.LabelField($"Active Effects: {t.EffectStack.ActiveEffectCount}");
            EditorGUI.indentLevel++;
            foreach (Effect effect in t.EffectStack)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(effect.name + ((effect.Stack > 1) ? $"x{effect.Stack}" : ""));

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
            DrawHeader("Attributes");
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

        private void DrawTags(AbilityComponent t)
        {

            var type = t.GetType();
            var field = type.GetField("_tags", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var tagContainer = field.GetValue(t) as TagContainer;

            Dictionary<string, int> tags = new Dictionary<string, int>();

            foreach(var tag in tagContainer)
            {
                if (tags.ContainsKey(tag)) tags[tag]++;
                else
                tags.Add(tag, 1);
            }

            EditorGUILayout.LabelField("Tags");

            EditorGUI.indentLevel++;
            foreach(var pair in tags)
            {
                EditorGUILayout.LabelField($"{pair.Key} ({pair.Value})");
            }
            EditorGUI.indentLevel--;
        }

        private void DrawHeader(string text)
        {
            GUILayout.Box(text, EditorStyles.toolbarButton);
        }
    }
}
