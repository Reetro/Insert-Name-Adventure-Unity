using UnityEngine;

public class GameplayObjectID : MonoBehaviour
{
    private int ID = 0;
    /// <summary>
    /// Generate this Gameobjects ID
    /// </summary>
    public void ConstructID()
    {
        ID = GeneralFunctions.GenID();
    }
    /// <summary>
    /// Get This Gameobjects ID
    /// </summary>
    public int GetID()
    {
        return ID;
    }
}
