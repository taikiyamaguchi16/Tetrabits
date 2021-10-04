using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class TetraInput : MonoBehaviour
{
    [SerializeField] TetraButton tetraButton;
    [SerializeField] TetraLever tetraLever;
    [SerializeField] TetraPad tetraPad;

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

    private void OnGUI()
    {
        List<string> guis = new List<string>();
        guis.Add("tetra button press : " + tetraButton.GetPress());
        guis.Add("tetra lever powered on : " + tetraLever.GetPoweredOn());
        guis.Add("tetra pad vector : " + tetraPad.GetVector());

        GUI.color = Color.black;
        for (int i = 0; i < guis.Count; i++)
        {
            GUI.Label(new Rect(10, 50 + 12 * i, 500, 50), guis[i]);
        }
    }
}
