using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class TELEPORTACIONN : MonoBehaviour
{
    [SerializeField]
    List<TeleportationAnchor> anchors;

    [SerializeField]
    int index = 0;

    InputTracker ipt;

    // Start is called before the first frame update
    void Start()
    {
        ipt = FindAnyObjectByType<InputTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ipt.GetInputDown(ControllerButton.aButton))
        {
            TeletransportarNext();
        }
    }

    public void TeletransportarNext()
    {
        anchors[index].RequestTeleport();
        index++;
        if(index > anchors.Count)
        {
            index = 0;
        }
    }


    public void Teletransportar(int i)
    {
        anchors[i].RequestTeleport();
    }


}
