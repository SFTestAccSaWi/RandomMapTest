using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance;
    public bool isPersistant;

    public virtual void Awake()
    {
        if (isPersistant)
        {
            if (!Instance)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance = this as T;
        }
    }
}