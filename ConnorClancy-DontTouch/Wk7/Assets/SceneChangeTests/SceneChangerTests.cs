using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneChangerTests
{
    [Test]
    public void SceneChangeChecksDestination()
    {
        bool result = SceneChanger.changeScene("ThisSceneShouldNotExist");
        Assert.AreEqual(result, false, "Changing to a non existent scene should fail - instead suceeded");
    }
    [UnityTest]
    public IEnumerator SceneChangeResultsInNewScene()
    {
        Debug.Log (SceneManager.GetActiveScene().name + " " + SceneManager.sceneCount + " " + SceneManager.sceneCountInBuildSettings);
        string targetScene = "TestDestScene";

        Assert.AreNotEqual(SceneManager.GetActiveScene().name, targetScene, "Test is starting in the wrong scene.");
        yield return null;

        bool result = SceneChanger.changeScene("TestDestScene");
        Assert.AreEqual(result, true, "Attempt to change scene to " + targetScene + "failed");

        yield return null;
        yield return null;
        yield return null;

        Assert.AreEqual(SceneManager.GetActiveScene().name, targetScene, "Destination scene not reached after scene change");

    }
}
