using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class letter : MonoBehaviour
{
    //I recommend you watch the full tutorial to know how this script works :)
    
    //The UI version of the letter that appears after picking up the letter.
    public GameObject letterUI;

    //The toggle bool determines if the letter is being picked up or put down.
    bool toggle;

    //The player's script. My player's script is called "SC_FPSController", change the name of the player script if your player's script name is different.
    public Player player;

    //The Mesh Renderer component of your letter that disables after picking up the letter and enables when putting it back down.
    public Renderer letterMesh;

    //Function to open and close the letter.
    public void openCloseLetter()
    {
        //Toggle will equal to the opposite of what it currently equals to.
        toggle = !toggle;

        //If toggle equals false, that means the player is putting down the letter.
        if(toggle == false)
        {
            letterUI.SetActive(false);
            letterMesh.enabled = true;
            player.enabled = true;
        }

        //If toggle equals true, that means the player is picking up the letter.
        if (toggle == true)
        {
            letterUI.SetActive(true);
            letterMesh.enabled = false;
            player.enabled = false;
        }
    }
}
