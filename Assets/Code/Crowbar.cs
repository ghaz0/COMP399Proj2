using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : MonoBehaviour
{
    public PlayerCtrl wielder;
    //I don't like coupling like this, a more improved approach would 
    // involve using the observer pattern--a more ideal structure would
    // forego this situation entirely via better game/project architecture

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null && other.gameObject.GetComponent<NPC>() != null)
        {
            print("WHACK!");
            other.gameObject.GetComponent<NPC>().SetHealth(-wielder.punchPower);
        }
    }


}
