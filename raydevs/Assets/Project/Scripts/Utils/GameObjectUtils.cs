namespace Raydevs.Utils
{
    using UnityEngine;

    public static class GameObjectUtils
    {
        public static GameObject InstantiateGameObject(GameObject prefab)
        {
            // Instantiate the prefab under the parent transform
            GameObject instance = Object.Instantiate(prefab);

            // Set the instance to inactive initially
            instance.SetActive(false);

            // Return the instantiated GameObject
            return instance;
        }

        public static GameObject InstantiateGameObject(GameObject prefab, Vector3 position)
        {
            // Instantiate the prefab under the parent transform
            GameObject instance = Object.Instantiate(prefab, position, Quaternion.identity);

            // Set the instance to inactive initially
            instance.SetActive(false);

            // Return the instantiated GameObject
            return instance;
        }

        public static GameObject InstantiateGameObject(GameObject prefab, Transform parent)
        {
            // Instantiate the prefab under the parent transform
            GameObject instance = Object.Instantiate(prefab, parent);

            // Set the instance to inactive initially
            instance.SetActive(false);

            // Return the instantiated GameObject
            return instance;
        }

        /// <summary>
        /// Instantiates a GameObject and sets it inactive for future use.
        /// </summary>
        /// <param name="prefab">The prefab to instantiate.</param>
        /// <param name="position"></param>
        /// <param name="parent">The parent for the instantiated object.</param>
        /// <returns>The instantiated GameObject.</returns>
        public static GameObject InstantiateGameObject(GameObject prefab, Vector3 position, Transform parent)
        {
            // Instantiate the prefab under the parent transform
            GameObject instance = Object.Instantiate(prefab, position, Quaternion.identity, parent);

            // Set the instance to inactive initially
            instance.SetActive(false);

            // Return the instantiated GameObject
            return instance;
        }

        public static (GameObject, T) InstantiateAndGetComponent<T>(GameObject prefab)
            where T : Component
        {
            // Instantiate the prefab under the parent transform
            GameObject instance = InstantiateGameObject(prefab);

            // Get the component of the type specified
            T component = instance.GetComponent<T>();

            if (component == null)
            {
                Debug.LogWarning(
                    $"The instantiated GameObject {prefab.name} does not contain a component of type {typeof(T).Name}. Returning null for the component.");
            }

            // Return both the instantiated GameObject and its component
            return (instance, component);
        }

        public static (GameObject, T) InstantiateAndGetComponent<T>(GameObject prefab, Transform parent)
            where T : Component
        {
            // Instantiate the prefab under the parent transform
            GameObject instance = InstantiateGameObject(prefab, parent);

            // Get the component of the type specified
            T component = instance.GetComponent<T>();

            if (component == null)
            {
                Debug.LogWarning(
                    $"The instantiated GameObject {prefab.name} does not contain a component of type {typeof(T).Name}. Returning null for the component.");
            }

            // Return both the instantiated GameObject and its component
            return (instance, component);
        }

        /// <summary>
        /// Instantiates a GameObject and its component, setting it inactive and storing it for future use.
        /// </summary>
        /// <param name="prefab">The prefab to instantiate.</param>
        /// <param name="position">The parent for the instantiated object.</param>
        /// <typeparam name="T">The type of the component to retrieve from the instantiated object.</typeparam>
        /// <returns>A tuple containing the instantiated GameObject and its component of type T.</returns>
        public static (GameObject, T) InstantiateAndGetComponent<T>(GameObject prefab, Vector3 position)
            where T : Component
        {
            // Instantiate the prefab under the parent transform
            GameObject instance = InstantiateGameObject(prefab, position);

            // Get the component of the type specified
            T component = instance.GetComponent<T>();

            if (component == null)
            {
                Debug.LogWarning(
                    $"The instantiated GameObject {prefab.name} does not contain a component of type {typeof(T).Name}. Returning null for the component.");
            }

            // Return both the instantiated GameObject and its component
            return (instance, component);
        }

        public static (GameObject, T) InstantiateAndGetComponent<T>(GameObject prefab, Vector3 position,
            Transform parent)
            where T : Component
        {
            // Instantiate the prefab under the parent transform
            GameObject instance = InstantiateGameObject(prefab, position, parent);

            // Get the component of the type specified
            T component = instance.GetComponent<T>();

            if (component == null)
            {
                Debug.LogWarning(
                    $"The instantiated GameObject {prefab.name} does not contain a component of type {typeof(T).Name}. Returning null for the component.");
            }

            // Return both the instantiated GameObject and its component
            return (instance, component);
        }
    }
}