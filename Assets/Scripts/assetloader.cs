using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Text;
using System.IO;
using UnityEngine.Networking;

public class assetloader : MonoBehaviour
{
    // Start is called before the first frame update

    //StartCoroutine(loadSelfTex("https://picsum.photos/720/1280"));
    const string baseURL = "https://12153-9.s.cdn12.com/imgs/";
    IEnumerator loadSelfTex(string filename)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(baseURL+filename.Replace("\\","/")))
        {
            // begin request:
            yield return www.SendWebRequest();

            // read results:
            if (www.isNetworkError || www.isHttpError)
            // if( www.result!=UnityWebRequest.Result.Success )// for Unity >= 2020.1
            {
                Debug.Log($"{www.error}, URL:{www.url}");

                // nothing to return on error:
                yield return null;
            }
            else
            {
                Texture2D result = DownloadHandlerTexture.GetContent(www);
                //GetComponent<Renderer>().material.SetTexture("_MainTex",result);
                //GetComponent<Image>().sprite = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
                Debug.Log("texture downloaded");
                byte[] textureBytes = result.EncodeToPNG();
                File.WriteAllBytes(Application.dataPath +"/SavedAssets/imgs/" +filename, textureBytes);
                Debug.Log("File Written On Disk!");
                yield return result;
            }
        }
        //eturn Texture2D.whiteTexture;
    }

    void DownloadFile(string filename) {
        StartCoroutine(loadSelfTex(filename));
    }
    void Start()
    {
        var path = Application.dataPath+ "/SavedAssets/hashes.json";//Path.Combine(Application.dataPath, "\\SavedAssets\\hashes.json");
        Debug.Log(path);
        string rawJson = File.ReadAllText(path);//Encoding.Default.GetString(webRequest.downloadHandler.data);
        JSONNode localJson = JSON.Parse(rawJson);
        JSONArray localFiles = localJson["files"].AsArray;


        path = Application.dataPath + "/SavedAssets/hashes2.json";
        rawJson = File.ReadAllText(path);
        JSONNode remoteJson = JSON.Parse(rawJson);
        JSONArray remoteFiles = remoteJson["files"].AsArray;
        for (int i = 0; i < remoteFiles.Count; i++) {
            bool found = false;
            for (int j = 0; j < localFiles.Count; j++)
            {
                if (remoteFiles[i]["filename"] == localFiles[j]["filename"])
                {
                    if (remoteFiles[i]["hash"] != localFiles[j]["hash"])
                    {
                        DownloadFile(remoteFiles[i]["filename"]);
                        found = true;
                        Debug.Log("downloading: " + remoteFiles[i]["filename"]);
                        break;
                    }
                    else {
                        Debug.Log("file was already on device: " + remoteFiles[i]["filename"]);
                        found = true;
                    }
                }
            }
            if (!found) {
                DownloadFile(remoteFiles[i]["filename"]);
                Debug.Log("downloading: " + remoteFiles[i]["filename"]);
            }
        }
        // save remoteJSON
        File.WriteAllText(Application.dataPath + "/SavedAssets/hashes.json", rawJson);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
