using UnityEngine;

// ReSharper disable StaticMemberInGenericType

/// <summary>
/// Singleton
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static object _lock = new object();
    private T[] objs;
    static bool isCreated;

    void Awake()
    {
        if (!Singleton<T>.isCreated)
        {
            Singleton<T>.isCreated = true;
            Object.DontDestroyOnLoad(gameObject);
        }
        else
        {
            Object.Destroy(gameObject);
        }
    }

    public static T Instance
    {
        get {
            lock (Singleton<T>._lock)
            {
                if (Singleton<T>._instance == null)
                {
                    Singleton<T>._instance = (T)Object.FindObjectOfType(typeof(T));

                    if (Object.FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " + " - there should never be more than 1 singleton!" + " Reopenning the scene might fix it.");
                        return Singleton<T>._instance;
                    }

                    if (Singleton<T>._instance == null)
                    {
                        GameObject singleton = new GameObject();
                        Singleton<T>._instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T);

                        Object.DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(T) + " is needed in the scene, so '" + singleton + "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        //Debug.Log("[Singleton] Using instance already created: " +_instance.gameObject.name);
                    }
                }

                return Singleton<T>._instance;
            }
        }
    }
}
