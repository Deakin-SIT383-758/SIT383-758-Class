using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class SpeechManager : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;

    public TMP_Text commandText;

    public SpawnManager spawnManager;
    public EnvironmentManager environmentManager;
    public PortalManager portalManager;

    private Dictionary<string, System.Action> commands =
        new Dictionary<string, System.Action>();

    void Start()
    {
        commands.Add("spawn cube", SpawnCube);
        commands.Add("spawn sphere", SpawnSphere);
        commands.Add("spawn cylinder", SpawnCylinder);
        commands.Add("spawn capsule", SpawnCapsule);
        commands.Add("spawn Portal", SpawnPortal);
        commands.Add("spawn Ore", SpawnOre);

        commands.Add("lights off", LightsOff);
        commands.Add("lights on", LightsOn);

        commands.Add("make cube bigger", MakeCubeBigger);
        commands.Add("make cube smaller", MakeCubeSmaller);
        commands.Add("make sphere bigger", MakeSphereBigger);
        commands.Add("make sphere smaller", MakeSphereSmaller);
        commands.Add("make cylinder bigger", MakeCylinderBigger);
        commands.Add("make cylinder smaller", MakeCylinderSmaller);
        commands.Add("make capsule bigger", MakeCapsuleBigger);
        commands.Add("make capsule smaller", MakeCapsuleSmaller);
        commands.Add("make portal bigger", MakePortalBigger);
        commands.Add("make portal smaller", MakePortalSmaller);
        commands.Add("make ore bigger", MakeOreBigger);
        commands.Add("make ore smaller", MakeOreSmaller);

        keywordRecognizer = keywordRecognizer = new KeywordRecognizer(commands.Keys.ToArray());

        keywordRecognizer.OnPhraseRecognized +=
            OnPhraseRecognized;

        keywordRecognizer.Start();

        Debug.Log("Speech recognition started");
    }

    private void OnPhraseRecognized(
        PhraseRecognizedEventArgs args)
    {
        Debug.Log("Recognized: " + args.text);

        commandText.text =
            "Recognized: " + args.text;

        if (commands.ContainsKey(args.text))
        {
            commands[args.text].Invoke();
        }
    }

    void LightsOff()
    {
        environmentManager.LightsOff();
    }

    void LightsOn()
    {
        environmentManager.LightsOn();
    }

    void SpawnCube()
    {
        spawnManager.SpawnCube();
    }

    void SpawnSphere()
    {
        spawnManager.SpawnSphere();
    }

    void SpawnCylinder()
    {
        spawnManager.SpawnCylinder();
    }

    void SpawnCapsule()
    {
        spawnManager.SpawnCapsule();
    }
    
    void SpawnPortal()
    {
        spawnManager.SpawnPortal();
    }

    void SpawnOre()
    {
        spawnManager.SpawnOre();
    }


    void MakeCubeBigger()
    {
        spawnManager.MakeCubeBigger();
    }

    void MakeCubeSmaller()
    {
        spawnManager.MakeCubeSmaller();
    }

    void MakeSphereBigger()
    {
        spawnManager.MakeSphereBigger();
    }

    void MakeSphereSmaller()
    {
        spawnManager.MakeSphereSmaller();
    }

    void MakeCylinderBigger()
    {
        spawnManager.MakeCylinderBigger();
    }

    void MakeCylinderSmaller()
    {
        spawnManager.MakeCylinderSmaller();
    }

    void MakeCapsuleBigger()
    {
        spawnManager.MakeCapsuleBigger();
    }

    void MakeCapsuleSmaller()
    {
        spawnManager.MakeCapsuleSmaller();
    }

    void MakePortalBigger()
    {
        spawnManager.MakePortalBigger();
    }

    void MakePortalSmaller()
    {
        spawnManager.MakePortalSmaller();
    }

    void MakeOreBigger()
    {
        spawnManager.MakeOreBigger();
    }

    void MakeOreSmaller()
    {
        spawnManager.MakeOreSmaller();
    }


    private void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }
}