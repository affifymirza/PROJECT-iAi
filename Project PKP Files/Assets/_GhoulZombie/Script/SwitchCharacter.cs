using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    public GameObject character1, character2;
    int whichCharacterIsOn = 1;

    // Start is called before the first frame update
    void Start()
    {
        character1.gameObject.SetActive(true);
        character2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            switchCharacter();
        }
    }

    public void switchCharacter()
    {
        switch (whichCharacterIsOn)
        {
            case 1:

                whichCharacterIsOn = 2;

                character1.gameObject.SetActive(true);
                character2.gameObject.SetActive(false);
                break;

            case 2:

                whichCharacterIsOn = 1;

                character1.gameObject.SetActive(false);
                character2.gameObject.SetActive(true);
                break;
        }
    }
}
