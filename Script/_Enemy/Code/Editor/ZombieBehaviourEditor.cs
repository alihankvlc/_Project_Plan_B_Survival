using _Zombie_System_.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace _Project_Plan_B_Survival.Script._Enemy.Code.Editor
{
    [CustomEditor(typeof(ZombieBehaviour))]
    public class ZombieBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ZombieBehaviour zombieBehaviour = (ZombieBehaviour)target;

            if (!Application.isPlaying)
            {
                return;
            }

            float baseTime = zombieBehaviour.ActiveStateType == ZombieStateType.Idle
                ? zombieBehaviour.IdleStateTimer
                : zombieBehaviour.PatrolStateTimer;

            float minutes = Mathf.Floor(baseTime / 60);
            float seconds = baseTime % 60;

            string idleMessage =
                $"Will transition to {SetStateName(zombieBehaviour.ActiveStateType)} state in {minutes:00}:{seconds:00} seconds";

            EditorGUILayout.HelpBox(idleMessage, MessageType.None);

            Repaint();
        }

        private string SetStateName(ZombieStateType type)
        {
            string stateName = type switch
            {
                ZombieStateType.Idle => "Patrol",
                ZombieStateType.Patrol => "Idle",
                _ => "NULL"
            };

            return stateName;
        }
    }
}