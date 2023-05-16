using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SFB;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

/// <summary>
/// 文件打开和保存对话框
/// </summary>
public class FileOpenAndSave : MonoBehaviour
{
    public static FileOpenAndSave instance;
    private void Awake()
    {
        instance = this;
    }

    protected ExtensionFilter[] GetExtensions()
    {
        var extensions = new List<string>
        {
            "fbx",
            "gltf",
            "glb",
            "obj",
        };
        var extensionFilters = new List<ExtensionFilter>();
        var subExtensions = new List<string>();
        for (var i = 0; i < extensions.Count; i++)
        {
            var extension = extensions[i];
            extensionFilters.Add(new ExtensionFilter(null, extension));
            subExtensions.Add(extension);
        }

        subExtensions.Add("zip");
        extensionFilters.Add(new ExtensionFilter(null, new[] { "zip" }));
        extensionFilters.Add(new ExtensionFilter("All Files", new[] { "*" }));
        extensionFilters.Insert(0, new ExtensionFilter("Accepted Files", subExtensions.ToArray()));
        return extensionFilters.ToArray();
    }

    //OpenFileDlg pth;
    //SaveFileDlg spth;

    ///// <summary>
    ///// 打开视频对话框
    ///// </summary>
    ///// <param name="_callBack"></param>
    //public void OpenVideoSelectDialog(Action<string> _callBack)
    //{
    //    string filefilter = "Video Files (*.mp4)\0*.mp4\0*.mkv\0All Files (*.*)\0*.*\0";
    //    string defext = "mp4";
    //    OpenFile(_callBack, filefilter, defext);
    //}

    /// <summary>
    /// 打开图片选择对话框
    /// </summary>
    /// <param name="_callback"></param>
    public void OpenPicSelectDialog(Action<string> _callback)
    {
        //string filefilter = "Picture Files (*.jpg)\0*.jpg\0All Files (*.*)\0*.*\0";
        string filefilter = "(*.jpg)\0*.jpg\0All Files (*.*)\0*.*\0";
        string defext = "jpg";
        OpenImageFile(_callback, filefilter, defext);
    }


#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    public void OnFileUpload(string url) {
        if (selectCallback!=null)
        {
            selectCallback(url);
        }
    }

#endif

    Action<string> selectCallback;
    /// <summary>
    /// 打开文件对话框，默认json格式
    /// </summary>
    /// <param name="_callback">打开回调</param>
    /// <param name="_filter">文件格式过滤</param>
    /// <param name="_defExt">默认后缀</param>
    public void OpenImageFile(Action<string> _callback, string _filter = "", string _defExt = "json")
    {
        //var extensions = new[] {
        //    new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
        //    new ExtensionFilter("Sound Files", "mp3", "wav" ),
        //    new ExtensionFilter("All Files", "*" ),
        //};

        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            new ExtensionFilter("All Files", "*" ),
        };

        selectCallback = _callback;
#if UNITY_WEBGL && !UNITY_EDITOR
        UploadFile(gameObject.name, "OnFileUpload", ".", false);
#else
        var paths = StandaloneFileBrowser.OpenFilePanel("打开文件", "", extensions, false);
        if (paths != null && paths.Length > 0)
        {
            if (selectCallback != null)
            {
                selectCallback(paths[0]);
            }
        }
#endif

        //if(pth==null)
        //    pth = new OpenFileDlg();
        //pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        //if (!string.IsNullOrEmpty(_filter))
        //    pth.filter = _filter;
        //else
        //    pth.filter = "JSON Files (*.json)\0*.json\0All Files (*.*)\0*.*\0";
        //pth.file = new string(new char[256]);
        //pth.maxFile = pth.file.Length;
        //pth.fileTitle = new string(new char[64]);
        //pth.maxFileTitle = pth.fileTitle.Length;
        //pth.initialDir = Application.dataPath;  // default path  

        ////对话框标题
        //pth.title = "打开";
        //if (!string.IsNullOrEmpty(_defExt))
        //    pth.defExt = _defExt;
        //else
        //    pth.defExt = "json"; //= "JPG";//显示文件的类型

        ////pth_EXPLORER|pth_FILEMUSTEXIST|pth_PATHMUSTEXIST| pth_ALLOWMULTISELECT|pth_NOCHANGEDIR
        ////注意 一下项目不一定要全选 但是0x00000008项不要缺少
        //pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        //if (OpenFileDialog.GetOpenFileName(pth))
        //{
        //    string filepath = pth.file;//选择的文件路径;  
        //    Debug.Log(filepath);
        //    if(_callback != null)
        //        _callback(filepath);
        //}
    }

    /// <summary>
    /// 打开文件对话框，默认json格式
    /// </summary>
    /// <param name="_callback">打开回调</param>
    /// <param name="_filter">文件格式过滤</param>
    /// <param name="_defExt">默认后缀</param>
    public void OpenSceneFile(Action<string> _callback, string _filter = "", string _defExt = "json")
    {
        //var extensions = new[] {
        //    new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
        //    new ExtensionFilter("Sound Files", "mp3", "wav" ),
        //    new ExtensionFilter("All Files", "*" ),
        //};

        var extensions = new[] {
            new ExtensionFilter("Scene Files", "json"),
            new ExtensionFilter("All Files", "*" ),
        };

        selectCallback = _callback;
#if UNITY_WEBGL && !UNITY_EDITOR
        UploadFile(gameObject.name, "OnFileUpload", ".", false);
#else
        var paths = StandaloneFileBrowser.OpenFilePanel("打开文件", "", extensions, false);
        if (paths != null && paths.Length > 0)
        {
            if (selectCallback != null)
            {
                selectCallback(paths[0]);
            }
        }
#endif

        //if(pth==null)
        //    pth = new OpenFileDlg();
        //pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        //if (!string.IsNullOrEmpty(_filter))
        //    pth.filter = _filter;
        //else
        //    pth.filter = "JSON Files (*.json)\0*.json\0All Files (*.*)\0*.*\0";
        //pth.file = new string(new char[256]);
        //pth.maxFile = pth.file.Length;
        //pth.fileTitle = new string(new char[64]);
        //pth.maxFileTitle = pth.fileTitle.Length;
        //pth.initialDir = Application.dataPath;  // default path  

        ////对话框标题
        //pth.title = "打开";
        //if (!string.IsNullOrEmpty(_defExt))
        //    pth.defExt = _defExt;
        //else
        //    pth.defExt = "json"; //= "JPG";//显示文件的类型

        ////pth_EXPLORER|pth_FILEMUSTEXIST|pth_PATHMUSTEXIST| pth_ALLOWMULTISELECT|pth_NOCHANGEDIR
        ////注意 一下项目不一定要全选 但是0x00000008项不要缺少
        //pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        //if (OpenFileDialog.GetOpenFileName(pth))
        //{
        //    string filepath = pth.file;//选择的文件路径;  
        //    Debug.Log(filepath);
        //    if(_callback != null)
        //        _callback(filepath);
        //}
    }

    public void OpenModelSelectDialog(Action<string> _callback)
    {
        selectCallback = _callback;
        //webgl下只需要设置选择的文件格式，所有文件格式的选项会自动加上
#if UNITY_WEBGL && !UNITY_EDITOR
        string filter = ".fbx, .gltf, .glb, .obj, .stl, .ply, .3mf, .zip";
        UploadFile(gameObject.name, "OnFileUpload", filter, false);
#else
        var paths = StandaloneFileBrowser.OpenFilePanel("打开文件", "", GetExtensions(), false);
        if (paths!=null&&paths.Length > 0)
        {
            if (selectCallback != null)
            {
                selectCallback(paths[0]);
            }
        }
#endif

    }

    public void OnFileDownload()
    {
        Debug.Log("File Successfully Downloaded");
        if (saveCallBack != null)
            saveCallBack("save successfull");
    }

    Action<string> saveCallBack;
    /// <summary>
    /// 保存文件对话框，默认json格式
    /// </summary>
    /// <param name="_callback">回调选择的路径</param>
    /// <param name="_filter">格式过滤</param>
    /// <param name="_defExt">默认后缀</param>
    public void SaveSceneJson(string contents, Action<string> _callback, string _filter = "", string _defExt = "json")
    {
        saveCallBack = _callback;
        var bytes = Encoding.UTF8.GetBytes(contents);
#if UNITY_WEBGL && !UNITY_EDITOR
        DownloadFile(gameObject.name, "OnFileDownload", "sample.json", bytes, bytes.Length);
#else
        var path = StandaloneFileBrowser.SaveFilePanel("保存", "", "sample", _defExt);
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, bytes);
            if (saveCallBack != null)
                saveCallBack(path);
        }
#endif


        //if (spth == null) spth = new SaveFileDlg();
        //spth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(spth);
        //if (!string.IsNullOrEmpty(_filter))
        //    spth.filter = _filter;
        //else
        //    spth.filter = "JSON Files (*.json)\0*.json\0All Files (*.*)\0*.*\0";
        //spth.file = new string(new char[256]);
        //spth.maxFile = spth.file.Length;
        //spth.fileTitle = new string(new char[64]);
        //spth.maxFileTitle = spth.fileTitle.Length;
        //spth.initialDir = Application.dataPath;  // default path  
        //spth.title = "保存";
        //if (!string.IsNullOrEmpty(_defExt))
        //    spth.defExt = _defExt;
        //else
        //    spth.defExt = "json";
        //spth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        //if (SaveFileDialog.GetSaveFileName(spth))
        //{
        //    string filepath = spth.file;//选择的文件路径;
        //    Debug.Log(filepath);
        //    if (_callback != null)
        //        _callback(filepath);
        //}
    }

}
