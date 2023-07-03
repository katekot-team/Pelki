using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.SimpleLocalization
{
    /// <summary>
    /// UnityWebRequest downloader (creates instance automatically).
    /// </summary>
    [ExecuteInEditMode]
    public class Downloader : MonoBehaviour
    {
        public static event Action OnNetworkReady = () => { }; 

        private static Downloader _instance;

	    public static Downloader Instance
        {
            get
            {
                if (_instance == null) _instance = new GameObject(typeof(Downloader).Name).AddComponent<Downloader>();

                return _instance;            
            }
        }

        public void OnDestroy()
        {
            _instance = null;
        }
       
        public static void Download(string url, Action<UnityWebRequest> callback)
        {
            Debug.Log($"Downloading {url}");
            Instance.StartCoroutine(Coroutine(url, callback));
        }

        private static IEnumerator Coroutine(string url, Action<UnityWebRequest> callback)
        {
            var request = new UnityWebRequest(url);

            yield return request;

            Debug.Log($"Downloaded: {url}, text = {CleanupText(request.downloadHandler.text)}, error = {request.error}");

            if (request.error == null)
            {
                OnNetworkReady();
            }

            callback(request);
        }

        private static string CleanupText(string text)
        {
            return text.Replace("\n", " ").Replace("\t", null);
        }
    }
}