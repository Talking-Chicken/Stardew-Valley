using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public PlayerBehaviour player;
    public StateManager stateManager;

    public GameObject detectingObj; //this is the object that raycasting is detecting
    public GameObject holdingObj; // this is the object that player is holding

    public GameObject backpackText;
    public GameObject backpackBG;
    public GameObject shortcutBG;
    public GameObject indicator; //indicates which item player is selecting

    public List<GameObject> shortcut; //this is the first line of backpack. During normal mode, only this line will show at the bottom
    public List<GameObject> backpack; //this is the backpack that player can press [I] to open with
    public List<GameObject> icon; //this is the objects's icon that shows in shortcut & backpack, has the same index with them

    //backpack size
    public int backpackMax;
    public int shortcutMax;

    //raycasting
    public float sight;

    //backpack item drawing
    [SerializeField]
    [Range(0, 10.0f)]
    float initX, initY; //initY only exist in backpack mode, not shortcut
    Vector2 initPos; 

    [SerializeField]
    float gapX, gapY;

    int column, row; //indiate column and rows in backpack when selecting items

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> backpack = new List<GameObject>();
        List<GameObject> shortcut = new List<GameObject>();
        List<GameObject> icon = new List<GameObject>();

        initPos = new Vector2(shortcutBG.transform.position.x - initX, shortcutBG.transform.position.y);
        column = 0;
        row = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // in mormal mode
        detectItem();
        openPack();
        clearLastEmpty();
        
        if (stateManager.getCurrentState() == stateManager.state_normal)
        {
            pickUp();
            drawIconOnNormal();
            selectObjInShortcut();
            useHoldingObj();
        }

        // in backpack mode
        closePack();

        if (holdingObj != null)
        {
            holdingObj.transform.position = gameObject.transform.position;
        }
    }

    void openPack()
    {
        if (Input.GetKeyDown(KeyCode.I) && stateManager.getCurrentState() == stateManager.state_normal)
        {
            backpackText.SetActive(true);
            backpackBG.SetActive(true);
            shortcutBG.SetActive(false);
            stateManager.state_current = stateManager.state_changing;
        }

        if (Input.GetKeyUp(KeyCode.I) && stateManager.getCurrentState() == stateManager.state_changing)
        {
            stateManager.state_current = stateManager.state_backpack;
        }
    }

    void closePack()
    {
        if (Input.GetKeyDown(KeyCode.I) && stateManager.getCurrentState() == stateManager.state_backpack)
        {
            backpackText.SetActive(false);
            backpackBG.SetActive(false);
            shortcutBG.SetActive(true);
            stateManager.state_current = stateManager.state_changingBack;
        }

        if (Input.GetKeyUp(KeyCode.I) && stateManager.getCurrentState() == stateManager.state_changingBack)
        {
            stateManager.state_current = stateManager.state_normal;
        }
    }

    void detectItem()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, player.dir, sight);
        Debug.DrawRay(player.transform.position, player.dir, Color.green);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "item")
            {
                detectingObj = hit.collider.gameObject;
            }
        }
    }

    void pickUp()
    {
        detectItem();
        if (detectingObj != null && backpack.Count < backpackMax)
        {
            if (Input.GetKeyDown(KeyCode.C) || (Input.GetMouseButtonDown(0)))
            {
                if (holdingObj != null)
                {
                    holdingObj.SetActive(false);
                }
                holdingObj = detectingObj;
                holdingObj.layer = 2; //making object on hand ignore raycasting
                holdingObj.transform.position = gameObject.transform.position;

                //setting detectingObj to null 
                detectingObj = null;

                //when picking up, object also goes into backpack and shortcut
                int shortcutIndex = getNearestEmpty(shortcut, shortcutMax);
                if ( shortcutIndex > -1)
                {
                    shortcut.Insert(shortcutIndex, holdingObj);
                    clearInsert(shortcut, shortcutIndex);
                    drawIconToShort(holdingObj);
                }

                int backpackIndex = getNearestEmpty(backpack, backpackMax);
                if (backpackIndex > -1)
                {
                    backpack.Insert(backpackIndex, holdingObj);
                    clearInsert(backpack, backpackIndex);
                }

                /*
                backpack.Add(holdingObj);
                if (shortcut.Count < shortcutMax)
                {
                    shortcut.Add(holdingObj);
                    drawIconToShort(holdingObj);
                }*/
            }
        }
    }

    void drawIconToShort(GameObject g)
    {
        Vector2 pos = new Vector2(shortcutBG.transform.position.x - gapX, shortcutBG.transform.position.y + gapY);
        GameObject o = Instantiate(g, pos, transform.rotation);
        o.GetComponent<SpriteRenderer>().sortingLayerName = "Objects on Interface";
        icon.Insert(shortcut.IndexOf(holdingObj), o);
        clearInsert(icon, shortcut.IndexOf(holdingObj));
        o.transform.position = new Vector2(initPos.x + (gapX * icon.IndexOf(o)), initPos.y);
        indicator.transform.position = o.transform.position;
    }

    void drawIconOnNormal()
    {
        if (icon.Count >0)
        {
            for (int i = 0; i< icon.Count; i++)
            {
                Vector2 pos = new Vector2(initPos.x + (gapX * i), initPos.y);
                if (icon[i] != null)
                {
                    icon[i].transform.position = pos;
                }   
            }
        }
    }

    //this function probably has a easier way to do it.......
    void selectObjInShortcut() //allows player to use [1] ~ [0] to select items in shortcut
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int index = 0; //the index of shortcut list, which player is selecting

            indicator.transform.position = new Vector2(initPos.x + (gapX * index), initPos.y); //draw indicator to corrosponding position

            if (holdingObj != null)
            {
                holdingObj.SetActive(false);
            }

            if (shortcut.Count >= index + 1)
            {
                if (shortcut[index] != null)
                {

                    holdingObj = shortcut[index];
                    holdingObj.SetActive(true);
                }
            }
            else
            {
                holdingObj = null;
            }
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            int index = 1; //the index of shortcut list, which player is selecting

            indicator.transform.position = new Vector2(initPos.x + (gapX * index), initPos.y); //draw indicator to corrosponding position

            if (holdingObj != null)
            {
                holdingObj.SetActive(false);
            }

            if (shortcut.Count >= index + 1)
            {
                if (shortcut[index] != null)
                {

                    holdingObj = shortcut[index];
                    holdingObj.SetActive(true);
                }
            }
            else
            {
                holdingObj = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            int index = 2; //the index of shortcut list, which player is selecting

            indicator.transform.position = new Vector2(initPos.x + (gapX * index), initPos.y); //draw indicator to corrosponding position

            if (holdingObj != null)
            {
                holdingObj.SetActive(false);
            }

            if (shortcut.Count >= index + 1)
            {
                if (shortcut[index] != null)
                {

                    holdingObj = shortcut[index];
                    holdingObj.SetActive(true);
                }
            }
            else
            {
                holdingObj = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            int index = 3; //the index of shortcut list, which player is selecting

            indicator.transform.position = new Vector2(initPos.x + (gapX * index), initPos.y); //draw indicator to corrosponding position

            if (holdingObj != null)
            {
                holdingObj.SetActive(false);
            }

            if (shortcut.Count >= index + 1)
            {
                if (shortcut[index] != null)
                {

                    holdingObj = shortcut[index];
                    holdingObj.SetActive(true);
                }
            }
            else
            {
                holdingObj = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            int index = 4; //the index of shortcut list, which player is selecting

            indicator.transform.position = new Vector2(initPos.x + (gapX * index), initPos.y); //draw indicator to corrosponding position

            if (holdingObj != null)
            {
                holdingObj.SetActive(false);
            }

            if (shortcut.Count >= index + 1)
            {
                if (shortcut[index] != null)
                {

                    holdingObj = shortcut[index];
                    holdingObj.SetActive(true);
                }
            }
            else
            {
                holdingObj = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            int index = 5; //the index of shortcut list, which player is selecting

            indicator.transform.position = new Vector2(initPos.x + (gapX * index), initPos.y); //draw indicator to corrosponding position

            if (holdingObj != null)
            {
                holdingObj.SetActive(false);
            }

            if (shortcut.Count >= index + 1)
            {
                if (shortcut[index] != null)
                {

                    holdingObj = shortcut[index];
                    holdingObj.SetActive(true);
                }
            }
            else
            {
                holdingObj = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            int index = 6; //the index of shortcut list, which player is selecting

            indicator.transform.position = new Vector2(initPos.x + (gapX * index), initPos.y); //draw indicator to corrosponding position

            if (holdingObj != null)
            {
                holdingObj.SetActive(false);
            }

            if (shortcut.Count >= index + 1)
            {
                if (shortcut[index] != null)
                {

                    holdingObj = shortcut[index];
                    holdingObj.SetActive(true);
                }
            }
            else
            {
                holdingObj = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            int index = 7; //the index of shortcut list, which player is selecting

            indicator.transform.position = new Vector2(initPos.x + (gapX * index), initPos.y); //draw indicator to corrosponding position

            if (holdingObj != null)
            {
                holdingObj.SetActive(false);
            }

            if (shortcut.Count >= index + 1)
            {
                if (shortcut[index] != null)
                {

                    holdingObj = shortcut[index];
                    holdingObj.SetActive(true);
                }
            }
            else
            {
                holdingObj = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            int index = 8; //the index of shortcut list, which player is selecting

            indicator.transform.position = new Vector2(initPos.x + (gapX * index), initPos.y); //draw indicator to corrosponding position

            if (holdingObj != null)
            {
                holdingObj.SetActive(false);
            }

            if (shortcut.Count >= index + 1)
            {
                if (shortcut[index] != null)
                {

                    holdingObj = shortcut[index];
                    holdingObj.SetActive(true);
                }
            }
            else
            {
                holdingObj = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            int index = 9; //the index of shortcut list, which player is selecting

            indicator.transform.position = new Vector2(initPos.x + (gapX * index), initPos.y); //draw indicator to corrosponding position

            if (holdingObj != null)
            {
                holdingObj.SetActive(false);
            }

            if (shortcut.Count >= index + 1)
            {
                if (shortcut[index] != null)
                {

                    holdingObj = shortcut[index];
                    holdingObj.SetActive(true);
                }
            } else
            {
                holdingObj = null;
            }
        }
    }

    int getNearestEmpty(List<GameObject> li, int maxNum)
    {
        if (li.Count < maxNum)
        {
            for (int i = 0; i < li.Count; i++)
            {
                if (li[i] == null)
                {
                    return i;
                }
            }

            return li.Count;
        }

        return -1;
    }

    void useHoldingObj()
    {
        if (holdingObj != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                destroyHoldingObj();
            }
        }
    }

    public void destroyHoldingObj() //use/destroy item that player is holding 
    {
        if (holdingObj != null)
        {
            Destroy(icon[shortcut.IndexOf(holdingObj)]);
            Destroy(shortcut[shortcut.IndexOf(holdingObj)]);
        }
    }

    public void clearLastEmpty() //to clear empty space in last of the list
    {
        if (shortcut.Count >0)
        {
            for (int i = shortcut.Count - 1; i >= 0; i--)
            {
                if (shortcut[i] == null)
                {
                    shortcut.RemoveAt(i);
                } else
                {
                    break;
                }
            }
        }

        if (icon.Count > 0)
        {
            for (int i = icon.Count - 1; i >= 0; i--)
            {
                if (icon[i] == null)
                {
                    icon.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }

        if (backpack.Count > 0)
        {
            for (int i = backpack.Count - 1; i >= 0; i--)
            {
                if (backpack[i] == null)
                {
                    backpack.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }
    }

    public void clearInsert(List<GameObject> list, int num) //to clear empty space in a list when insert an item in
    {
        if (list.Count > num + 1)
        {
            if (list[num + 1] == null)
            {
                list.RemoveAt(num + 1);
            }
        }
    }
}
