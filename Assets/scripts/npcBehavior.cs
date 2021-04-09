using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class npcBehavior : MonoBehaviour
{
    // to easily ACTIVATE or DEACTIVATE the conversation
    bool NPCtalk = false;

    // to ENABLE or DISABLE the object easily !
    public GameObject self;
    
    // DIALOGUE SYSTEM
        // good to copy + paste to other NPC's with dialogue

    // SETUP
        public Image txtbox; // the textbox
        public Text txt; // the txt IN the txtbox
        public Button button; // the button system
        public Image buttonIMG; // the button image

    // SPRITES
        public Text nametag; // the name of the character
        public Image portrait; // the portrait of the character

    // DIALOGUE
        public float CONVERSATION; // conversation manager !!!

        public string[] dialogueLines; // txt my beloved
        public int currentLine; // takes note of the dialogue #
        public float waitSPEED; // amount of time we have to wait for the letters to appear!

        public AudioSource TXTsfx; // the typing sound >:)

    // Start is called before the first frame update
    void Start()
    {
    // DIALOGUE UI
        // they are hidden until triggered ...
        txt.enabled = false;
        txtbox.enabled = false;

        portrait.enabled = false;
        nametag.enabled = false;

        buttonIMG.enabled = false;

        TXTsfx = GetComponent<AudioSource>(); // assigns the sound!
        
    }

    // Update is called once per frame
    void Update()
    {
        dialogue();
        if(txt.text == dialogueLines[currentLine]){ // if the text completes typing the dialogue ,
            // enable the button to progress!
            button.enabled =  true;
            buttonIMG.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name == "player"){ // IF the player is IN RANGE of the box collider ,
            Debug.Log("mrow");
            NPCtalk = true; // player is at a good distance to trigger dialogue !

            // since the player is at the right talking range ,
                // start typing !
            StartCoroutine(Type(dialogueLines[currentLine]));
        }
    }

// THE TYPING SYSTEM ...!
    public IEnumerator Type(string dialogueLines){

        TXTsfx.Play(); // plays the typing SFX ,
        txt.text = ""; // makes sure the text is presenting the right text
        
        foreach (var letter in dialogueLines.ToCharArray()){
            txt.text += letter;
            yield return new WaitForSeconds(waitSPEED);
        }

        TXTsfx.Stop(); // stop playing the typing SFX !!
        buttonIMG.enabled = true; // enable the button to progress !
    }

// to handle dialogue VISUALS
    // it enables + disables the UI
    // when the time comes, we can change the sprites of different characters for specific dialogue!
        // e.g make the character angry when needed.
    void dialogue(){
        if (NPCtalk){
        // ENABLES the UI visuals
            txtbox.enabled = true;
            txt.enabled = true;

            portrait.enabled = true;
            nametag.enabled = true;
            

            if (CONVERSATION == 3){ // IF the player reaches the END of the dialogue ...
                Debug.Log("hatsune miku"); // lmk !

            // DISABLE everything !
                NPCtalk = false;
                txt.enabled = false;
                txtbox.enabled = false;

                portrait.enabled = false;
                nametag.enabled = false;

                button.enabled = false;
                buttonIMG.enabled = false;

            // RESET the conversation
                currentLine = 0;
                CONVERSATION = 0;
            
            // DISABLE the event until the player manually triggers it again !
                self.SetActive(false); 
            }
        }
    }

// FOR THE BUTTON SYSTEM ...     
    public void NXT(){ // when the player presses next , 
    
        buttonIMG.enabled = false; // enable the button to progress !

    // make sure the CONVERSATION tracker is the same as the dialogue lines ,
        CONVERSATION = currentLine + 1;
        
        if(currentLine < dialogueLines.Length - 1){
            currentLine++;
            txt.text = "";
            StartCoroutine(Type(dialogueLines[currentLine]));
            }
    }
}
