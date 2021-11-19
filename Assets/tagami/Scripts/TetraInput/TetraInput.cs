using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraInput : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] TetraButton tetraButton;
    [SerializeField] TetraLever tetraLever;
    [SerializeField] TetraPad tetraPad;

    [Header("Debug Option")]
    [SerializeField] bool dispGUI = true;
    [SerializeField] Color guiColor;
    Rect guiWindowRect = new Rect(0, 0, Screen.width / 2, Screen.height / 2);


    public static TetraButton sTetraButton { private set; get; }
    public static TetraLever sTetraLever { private set; get; }
    public static TetraPad sTetraPad { private set; get; }

    // Start is called before the first frame update
    void Start()
    {
        sTetraButton = tetraButton;
        sTetraLever = tetraLever;
        sTetraPad = tetraPad;
    }

  
    public void OnGUIWindow()
    {
        if (dispGUI)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 25;
            GUILayout.Label(nameof(TetraInput),style);

            List<string> guis = new List<string>();
            guis.Add("tetra button press : " + tetraButton.GetPress());
            guis.Add("tetra lever powered on : " + tetraLever.GetPoweredOn());
            guis.Add("tetra pad vector : " + tetraPad.GetVector());
            guis.Add("objects on pad");

            foreach (var go in tetraPad.GetObjectsOnPad())
            {
                if (go)
                    guis.Add(go.name);
            }

            GUI.color = guiColor;
            for (int i = 0; i < guis.Count; i++)
            {
                GUILayout.Label(guis[i]);
            }

            if (GUILayout.Button("dead battery debug local"))
            {
                tetraButton.deadBatteryDebug = true;
                tetraLever.deadBatteryDebug = true;
                tetraPad.deadBatteryDebug = true;
            }
        }

    }
}
