// EnemySpawnerEditor.cs

using Raydevs.Utils;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Editor
{
    [CustomEditor(typeof(EnemySpawner))]
    public class EnemySpawnerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EnemySpawner enemySpawner = (EnemySpawner)target;
            if(GUILayout.Button("Spawn Enemy"))
            {
                enemySpawner.SpawnEnemy();
            }
        }
    }
}
