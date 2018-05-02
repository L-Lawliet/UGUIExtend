using UnityEngine;
using System.Collections;

/// <summary>
///
/// name:ScreenShot
/// author:Administrator
/// date:2016/11/11 13:53:26
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Assets.Scripts.ArtScripts
{
    public class ScreenShot : MonoBehaviour
    {
        public Camera camera;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnGUI()
        {
            if (GUILayout.Button("播放"))
            {
                Time.timeScale = 1;
            }

            if (GUILayout.Button("暂停"))
            {
                Time.timeScale = 0;
            }

            if (GUILayout.Button("截屏"))
            {
#if !UNITY_WEBPLAYER
                RenderTexture rt = new RenderTexture(1920, 1080, 32, RenderTextureFormat.ARGB32);

                rt.filterMode = FilterMode.Trilinear;

                // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
                camera.targetTexture = rt;  
                camera.Render();

                RenderTexture.active = rt;
                Texture2D screenShot = new Texture2D(1920, 1080, TextureFormat.ARGB32, false);
                screenShot.ReadPixels(camera.pixelRect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
                screenShot.Apply();
                screenShot.filterMode = FilterMode.Trilinear;

                // 重置相关参数，以使用camera继续在屏幕上显示  
                camera.targetTexture = null;
                //ps: camera2.targetTexture = null;  
                RenderTexture.active = null; // JC: added to avoid errors  
                GameObject.Destroy(rt);
                // 最后将这些纹理数据，成一个png图片文件  
                byte[] bytes = screenShot.EncodeToPNG();
                string filename = Application.dataPath + "/Screenshot.png";
                System.IO.File.WriteAllBytes(filename, bytes);
                Debug.Log(string.Format("截屏了一张照片: {0}", filename));  
#endif
            }
        }
    }
}
