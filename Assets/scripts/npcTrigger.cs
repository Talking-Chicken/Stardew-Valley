using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class npcTrigger : MonoBehaviour
{
// this script is to trigger the conversation!

    bool talkYES; // to CONFIRM that the player can talk

    public GameObject NPCactivate; // to hold the conversation object (the actual dialogue and stuff!)

    // DIALOGUE VISUALS
        // good to copy + paste to other NPC triggers 

    // SETUP
        public Image txtbox; // the textbox
        public Text txt; // the txt IN the txtbox
        public Button button; // the button system
        public Image buttonIMG; // the button image
                                // a separate image allows us to enable when we need it and disable when we don't!

    // SPRITES
        public Text nametag; // the name of the character
        public Image portrait; // the portrait of the character

    // Start is called before the first frame update
    void Start()
    {
    // DIALOGUE UI
        // they are hidden until triggered ...
        txt.enabled = false;
        txtbox.enabled = false;

        portrait.enabled = false;
        nametag.enabled = false;

        button.enabled = false;
        buttonIMG.enabled = false;
        
        NPCactivate.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        NPChat();
    }

    void OnTriggerEnter2D(Collider2D other){ 
        if(other.gameObject.name == "player"){ // IF the player is IN range of the collider ,
            Debug.Log("player in range"); 
            talkYES = true; // player is at a good distance to talk!
        }
    }

    void NPChat(){
        if(talkYES && Input.GetMouseButtonDown(0)){ // if player is in RANGE + the player clicks ,
            NPCactivate.SetActive(true); // activate the conversation!
        }
    }

        void OnTriggerExit2D(Collider2D other){ 
        if(other.gameObject.name == "player"){ // IF the player is OUT of range of the collider , 
            Debug.Log("player out of range");
            talkYES = false; // player cannot talk anymore!
        }
    }
}
