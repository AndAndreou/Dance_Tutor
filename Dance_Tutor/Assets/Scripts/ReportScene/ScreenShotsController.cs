using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenShotsController : MonoBehaviour {

    public int resWidth = 2550;
    public int resHeight = 3300;

    private bool takeShot = false;

    private int shotCounter = 0;

    private Camera camera;

    private List<Sprite> screenShotsSprites;

    private ReportSceneManager sceneManager;

    private void Start()
    {
        sceneManager = FindObjectOfType<ReportSceneManager>();
        screenShotsSprites = new List<Sprite>();

        camera = this.GetComponent<Camera>();
        CleanScreenShots();
    }

    public static string ScreenShotName(int shotNo)
    {
        return string.Format("{0}/Resources/ScreenShots/Shot_{1}.png",
                                Application.dataPath,
                                shotNo);
    }

    public void CreateScreenShotFolder()
    {
        if (!Directory.Exists(Application.dataPath + "/Resources/ScreenShots"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources/ScreenShots");
            RefreshEditorProjectWindow();
        }
    }

    public void CleanScreenShots()
    {
        Directory.Delete(Application.dataPath + "/Resources/ScreenShots/",true);
        RefreshEditorProjectWindow();
        CreateScreenShotFolder();
    }

    public void TakeShot()
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 32);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        //// Dont needed this
        //string filename = ScreenShotName(shotCounter);
        //File.WriteAllBytes(filename, bytes);

        // Create sprite
        Texture2D texture2d = new Texture2D(2,2);
        texture2d.LoadImage(bytes);
        Sprite newSprite = new Sprite();
        newSprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0, 0));
        screenShotsSprites.Add(newSprite);

        sceneManager.AddFigureImg(shotCounter, newSprite);

        shotCounter++;
    }

    void RefreshEditorProjectWindow()
    {
    #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
    #endif
    }
}
