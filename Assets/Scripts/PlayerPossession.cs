using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossession : MonoBehaviour
{
    public static PlayerPossession instance;

    PlayerController currentPossessedCharacter = null;

    public PlayerController playerCharacterOnStart = null;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else{
            Destroy(this);
        }

        PlayerController[] listOfAllPlayableObjects = FindObjectsOfType<PlayerController>();
        if (playerCharacterOnStart == null)
        {
            playerCharacterOnStart = listOfAllPlayableObjects[0];
        }
        for (int i = 0; i < listOfAllPlayableObjects.Length; i++)
        {
            listOfAllPlayableObjects[i].enabled = false;
        }
        PossessCharacter(playerCharacterOnStart);
    }

    public void PossessCharacter(PlayerController newCharacterToPossess)
    {
        if(currentPossessedCharacter != null)
        {
            currentPossessedCharacter.enabled = false;
        }
        newCharacterToPossess.enabled = true;
        currentPossessedCharacter = newCharacterToPossess;
    }
}
