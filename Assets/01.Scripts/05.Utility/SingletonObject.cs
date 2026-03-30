using UnityEngine;

namespace _01.Scripts._05.Utility
{
    public class SingletonObject<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _isShuttingDown;

        public static T Instance
        {
            get
            {
                if (_isShuttingDown)
                {
                    return null;
                }

                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();
                    
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
                        
                        Debug.Log($"[Singleton] {typeof(T)} 인스턴스 생성");
                    }
                    
                    if (_instance.transform.parent != null)
                    {
                        _instance.transform.SetParent(null);
                    }

                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                
                if (transform.parent != null)
                {
                    transform.SetParent(null);
                }
                
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"[Singleton] {typeof(T)}: 중복된 인스턴스");
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _isShuttingDown = true;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}