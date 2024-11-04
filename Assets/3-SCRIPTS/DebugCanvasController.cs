using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class DebugCanvasController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    SaltBagController saltBag;

    [SerializeField]
    CrossController crossMimic;

    // Update is called once per frame
    void Update()
    {
        List<bool> answers = saltBag.CanSpawnSalt();
        float[] val = saltBag.ActionValues();

        if(saltBag.SaltPosition() != null)
        {
            foreach (Vector3 a in crossMimic.GetLineRendererPoints())
            {
                text.text += a.x + " " + a.y + " " + a.z + "\n";
            }
            //text.text = "saltPosition\nx: " + saltBag.SaltPosition().Value.x + "\ny: " + saltBag.SaltPosition().Value.y + "\nz: " + saltBag.SaltPosition().Value.z;
        }
        else
        {
            text.text = "salt null";
        }
        //text.text = "leftActionValue: " + val[0] + "\nrightActionValue: " + val[1] + "\ncrossActionValue: " + crossMimic.ActionValue();
        //text.text = "grabstate no null: " + answers[0] + "\ncanpullsalt: " + answers[1] + "\nsaltRef null: " + answers[2] + "\ninput pressed: " + answers[3];
    }
}
