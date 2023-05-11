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
    public class FileSaveSample : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Text outputText;

#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    public void OnPointerDown(PointerEventData eventData) {
        var str = outputText.text + "_saved";

        if (str.Length > 0)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            DownloadFile(gameObject.name, "OnFileDownload", "sample_saved.txt", bytes, bytes.Length);
        }
    }

    public void OnFileDownload() {
        Debug.Log("CSV file saved");
        outputText.text = "File Saved";
    }

#else

        public void OnPointerDown(PointerEventData eventData) { }

        void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => SaveFile());
        }

        public void SaveFile()
        {
            var str = outputText.text + "_saved";

            if (str.Length > 0)
            {
                var path = StandaloneFileBrowser.SaveFilePanel("保存", "", "sample_saved", "txt");
                if (!string.IsNullOrEmpty(path))
                {
                    File.WriteAllText(path, str);
                    Debug.Log("File saved");
                    outputText.text = "File Saved";
                }
            }
        }

#endif

    }
}