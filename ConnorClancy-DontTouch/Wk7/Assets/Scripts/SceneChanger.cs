using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneChanger : MonoBehaviour
{
    private static int findSceneNameInBuild(string name)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (Path.GetFileNameWithoutExtension (SceneUtility.GetScenePathByBuildIndex (i)).Equals (name))
            {
                return i;
            }
        }
        return -1;

    }
    public static bool changeScene (string destSceneName)
    {
        if (findSceneNameInBuild (destSceneName) < 0)
        {
            return false;
        }
        SceneManager.LoadScene(destSceneName);
        return true;
    }
}
