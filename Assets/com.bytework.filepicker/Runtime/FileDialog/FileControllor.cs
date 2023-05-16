using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace SFB
{
    public class FileControllor : MonoBehaviour
    {
        public Button OpenBtn;
        public Button SaveBtn;

        void Awake()
        {
            OpenBtn.onClick.AddListener(OpenProject);
            SaveBtn.onClick.AddListener(SaveProject);
        }

        public void OpenProject()
        {
            OpenFileDlg pth = new OpenFileDlg();
            pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
            pth.filter = "txt (*.txt)"; //= "JPG Files (*.jpg)\0*.jpg\0All Files (*.*)\0*.*\0";
            pth.file = new string(new char[256]);
            pth.maxFile = pth.file.Length;
            pth.fileTitle = new string(new char[64]);
            pth.maxFileTitle = pth.fileTitle.Length;
            pth.initialDir = Application.dataPath;  // default path  

            //对话框标题
            pth.title = "打开项目";
            pth.defExt = "txt"; //= "JPG";//显示文件的类型
                                //pth_EXPLORER|pth_FILEMUSTEXIST|pth_PATHMUSTEXIST| pth_ALLOWMULTISELECT|pth_NOCHANGEDIR
                                //注意 一下项目不一定要全选 但是0x00000008项不要缺少
            pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
            if (OpenFileDialog.GetOpenFileName(pth))
            {
                string filepath = pth.file;//选择的文件路径;  
                Debug.Log(filepath);
                //StartCoroutine(WaitLoad(filepath));//加载图片到panle
            }
        }
        public void SaveProject()
        {
            SaveFileDlg pth = new SaveFileDlg();
            pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
            pth.filter = "txt (*.txt)";
            pth.file = new string(new char[256]);
            pth.maxFile = pth.file.Length;
            pth.fileTitle = new string(new char[64]);
            pth.maxFileTitle = pth.fileTitle.Length;
            pth.initialDir = Application.dataPath;  // default path  
            pth.title = "保存项目";
            pth.defExt = "txt";
            pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
            if (SaveFileDialog.GetSaveFileName(pth))
            {
                string filepath = pth.file;//选择的文件路径;  
                Debug.Log(filepath);
            }
        }

        //加载图片
        IEnumerator WaitLoad(string fileName)
        {
            Texture tex;
            UnityWebRequest wwwTexture = new UnityWebRequest("file://" + fileName);
            var dwonHandler = new DownloadHandlerTexture(true);
            wwwTexture.downloadHandler = dwonHandler;
            Debug.Log(wwwTexture.url);

            yield return wwwTexture.SendWebRequest();
            if (wwwTexture.isDone)
            {
                tex = dwonHandler.texture;
            }
        }

    }
}