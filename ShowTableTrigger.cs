using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class ShowTableTrigger : UdonSharpBehaviour
{
    [SerializeField]
    private GameObject table;

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            table.SetActive(true);
        }
    }

    /*public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            table.SetActive(false);
        }
    }*/
}
