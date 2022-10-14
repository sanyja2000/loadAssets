using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class loadimage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateImage();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateImage() {
        // try to save the picture: https://stackoverflow.com/questions/31942113/how-to-cache-images-in-unity-android
        StartCoroutine(loadSelfTex("https://picsum.photos/720/1280"));
    }
    IEnumerator loadSelfTex(string url) {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
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
                GetComponent<Image>().sprite = Sprite.Create(result,new Rect(0,0,result.width,result.height),new Vector2(0.5f,0.5f));
                Debug.Log("texture set");
                yield return result;
            }
        }
        //eturn Texture2D.whiteTexture;
    }

}
