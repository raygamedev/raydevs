namespace Raydevs.Editor
{
    using Utils;
    using UnityEditor;
    using UnityEngine;

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
