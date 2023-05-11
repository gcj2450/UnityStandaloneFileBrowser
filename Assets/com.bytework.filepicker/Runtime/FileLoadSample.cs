using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SFB
{
    [RequireComponent(typeof(Button))]
    public class FileLoadSample : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Text outputText;

        private string _loadedText = "";

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //

    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".", false);
    }

    public void OnFileUpload(string url,string url2) {
        StartCoroutine(Load(url));
    }

#else

        public void OnPointerDown(PointerEventData eventData) { }

        void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => OpenFile());
        }

        public void OpenFile()
        {
            var extensions = new[] {
            new ExtensionFilter("All Files", "*" ),
        };

            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
            if (paths.Length > 0 && paths[0].Length > 0)
            {

                StartCoroutine(Load(new System.Uri(paths[0]).AbsoluteUri));

            }
        }

#endif
        private IEnumerator Load(string url)
        {
            var request = UnityWebRequest.Get(url);

            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                yield return null;
            }

            _loadedText = request.downloadHandler.text;
            Debug.Log(_loadedText);
            outputText.text = _loadedText;
        }

    }
}