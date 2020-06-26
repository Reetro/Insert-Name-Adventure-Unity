using UnityEngine;

public class GameplayObjectID : MonoBehaviour
{
    private int ID = 0;

    void Start()
    {
        ID = GeneralFunctions.GenID();
    }

    public int GetID()
    {
        return ID;
    }
}
