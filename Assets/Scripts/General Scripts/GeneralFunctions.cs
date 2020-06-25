using UnityEngine;

public class GeneralFunctions
{
    /// <summary>
    ///  will check to see if the given object has a Rigidbody2D and if it does will see if it's velocity is greater than zero
    /// </summary>
    /// <param name="objectToTest"></param>
    /// <returns>True if the given object is moving false if not</returns>
    public static bool IsObjectMovingHorizontaly(GameObject objectToTest)
    {
        var rigidBody = objectToTest.GetComponent<Rigidbody2D>();

        if (rigidBody)
        {
            return Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        }
        else
        {
            Debug.LogError(objectToTest.ToString() + "didn't have a a Rigidbody2D was unable to test it horizontally");
            return false;
        }
    }
    // Checks to see if the given number is negative
    public static bool IsNumberNegative(float number)
    {
        return number < 0;
    }
    /// <summary>
    ///  Will rotate the object the opposite way it's currently facing
    /// </summary>
    /// <param name="objectToFlip"></param>
    public static void FlipObject(GameObject objectToFlip)
    {
        objectToFlip.transform.Rotate(0f, 180f, 0f);
    }
    /// <summary>
    ///  Gets the directional vector of an angle 
    /// </summary>
    /// <param name="angle"></param>
    /// <returns>A vector2 that is pointing a direction</returns>
    public static Vector2 GetDirectionVector2DFromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }
    /// <summary>
    ///  Get the distance between to vectors
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns>The distance between the two given vectors</returns>
    public static Vector2 GetDistanceBetweenVectors(Vector2 vector1, Vector2 vector2)
    {
        return vector1 - vector2;
    }
    /// <summary>
    /// Gets the directional vector of 2 vectors 
    /// </summary>
    /// <param name="position1"></param>
    /// <param name="position2"></param>
    /// <returns>A vector2 that is pointing a direction</returns>
    public static Vector2 GetDirectionVectroFrom2Vectors(Vector2 position1, Vector2 position2)
    {
        return position1 - position2.normalized;
    }
    /// <summary>
    ///  Rotate rigid body to a specified point at a specified speed
    /// </summary>
    /// <param name="target">Target location</param>
    /// <param name="rigidBody">RigidBody to rotate</param>
    /// <param name="speed">Speed to rotate at</param>
    /// <param name="gameObject">Game object the rigidBody is on</param>
    public static void RotateRigidBody(Vector2 target, Rigidbody2D rigidBody, float speed, GameObject gameObject)
    {
        Vector2 direction = (Vector2)target - rigidBody.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, gameObject.transform.up).z;

        rigidBody.angularVelocity = rotateAmount * speed;
    }
    /// <summary>
    /// Checks to see if position 1 is above position 2
    /// </summary>
    /// <param name="position1"></param>
    /// <param name="position2"></param>
    /// <returns>A bool that determines if position 1 is above or below position 2</returns>
    public static bool IsObjectAbove(Vector2 position1, Vector2 position2)
    {
        return (position1.normalized.y >= position2.normalized.y) ? true : false;
    }
    /// <summary>
    /// Will rotate a given object to face a given vector
    /// </summary>
    /// <param name="currentLocation">Current objects location</param>
    /// <param name="LookAt">The look at location</param>
    /// <param name="objectToRotate">Object to rotate</param>
    public static void LookAt2D(Vector2 currentLocation, Vector2 LookAt, GameObject objectToRotate)
    {
        var dir = currentLocation - LookAt;

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        objectToRotate.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    /// <summary>
    /// Spawns a leech and attaches it to the given transform then applies the leeching debuff to player
    /// </summary>
    /// <param name="auraManager">The aura manager to spawn the leech debuff on</param>
    /// <param name="attachedLeech">The leech to attach</param>
    /// <param name="transform">Transform of the parent object</param>
    /// <param name="health">Incoming leech's health</param>
    /// <param name="player">Player reference</param>
    /// <returns>The spawned leech attached to the player</returns>
    public static AttachedLeech SpawnLeechAttach(AuraManager auraManager, AttachedLeech attachedLeech, Transform transform, float health,  GameObject player)
    {
        AttachedLeech localLeech = GameObject.Instantiate(attachedLeech, transform.position, Quaternion.identity);

        localLeech.transform.parent = transform;

        localLeech.OnLeechSpawn(health, auraManager, player);

        return localLeech;
    }
    /// <summary>
    ///  Finds a point to attach a leech to by look for the point by tag
    /// </summary>
    /// <param name="tag">The leech collision tag</param>
    /// <returns>The transfrom of the found leeh attach point</returns>
    public static Transform GetLeechAttachPointByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag).transform;
    }
    /// <summary>
    /// Checks to see if leech is already attached on the given tag
    /// </summary>
    /// <param name="tag">the collision tag the leech is attaching to</param>
    /// <returns>A bool that determines if a leech can attach to a given point</returns>
    public static bool CanLeechAttach(string tag)
    {
        return (GameObject.FindGameObjectWithTag(tag).transform.childCount <= 0) ? true : false;
    }
    /// <summary>
    ///  Checks to see if leech that is trying to attach to player is current dead
    /// </summary>
    /// <param name="leech">The leech to check</param>
    /// <returns>A bool that determines if the given leech is dead</returns>
    public static bool IsLeechDead(GameObject leech)
    {
        var leechHelthComp = leech.GetComponent<HealthComponent>();

        if (leechHelthComp)
        {
            return leechHelthComp.GetIsDead();
        }
        else
        {
            Debug.LogError("Failed to check leech " + leech.name.ToString() + " did not have a health component");
            return false;
        }
    }
    /// <summary>
    /// Calls the construct health component on the given game object
    /// </summary>
    /// <param name="gameObject">Game object with the health component</param>
    public static void ConstructHPComponent(GameObject gameObject)
    {
        var health = gameObject.GetComponent<HealthComponent>();

        if (health)
        {
            health.ConstructHealthComponent();
        }
        else
        {
            Debug.LogError("Failed to construct Health Component on " + gameObject.name.ToString() + " does not have a health component");
        }
    }
    /// <summary>
    ///  Gets the targets health component then heals target
    /// </summary>
    public static void HealTarget(GameObject target, float amount)
    {
        var health = target.GetComponent<HealthComponent>();

        if (health)
        {
            health.AddHealth(amount);
        }
        else
        {
            Debug.LogError("Failed to heal " + target.name.ToString() + " does not have a health component");
        }
    }
    /// <summary>
    /// Pauses the game
    /// </summary>
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
    /// <summary>
    /// Resumes the game
    /// </summary>
    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }
    /// <summary>
    /// Creates random Vector 2 coordinates
    /// </summary>
    /// <param name="minX">minimum amount on the x coordinate</param>
    /// <param name="maxX">maximum amount on the x coordinate</param>
    /// <param name="minY">minimum amount on the y coordinate</param>
    /// <param name="maxY">maximum amount on the y coordinate</param>
    /// <returns>A a random vector 2</returns>
    public static Vector2 CreateRandomVector2(float minX, float maxX, float minY, float maxY)
    {
        var randomX = Random.Range(minX, maxX);
        var randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }
    /// <summary>
    /// Creates a random Vector 2 but only on the Y axis
    /// </summary>
    /// <param name="minY">minimum amount on the y coordinate</param>
    /// <param name="maxY">>maximum amount on the y coordinate</param>
    /// <returns>A vector2 with a random y coordinate</returns>
    public static Vector2 CreateRandomVector2OnlyY(float minY, float maxY)
    {
        var randomY = Random.Range(minY, maxY);

        return new Vector2(0, randomY);
    }
    /// <summary>
    /// Creates a random Vector 2 but only on the X axis
    /// </summary>
    /// <param name="minX">minimum amount on the x coordinate</param>
    /// <param name="maxX">maximum amount on the x coordinate</param>
    /// <returns>A vector2 with a random x coordinate</returns>
    public static Vector2 CreateRandomVector2OnlyX(float minX, float maxX)
    {
        var randomX = Random.Range(minX, maxX);

        return new Vector2(randomX, 0);
    }
    /// <summary>
    /// Finds all the enemies in the current scene
    /// </summary>
    /// <returns>An array of all enemy ID objects</returns>
    public static GameObject[] GetAllEnimesInScene()
    {
        var objects = GameObject.FindGameObjectsWithTag("Enemy");

        return objects;
    }
    /// <summary>
    /// Finds all the enemies in the current scene
    /// </summary>
    /// <param name="idObject">the ID game object found on enemies</param>
    /// <returns>The found enemy Gameobject</returns>
    public static GameObject GetEnemyByID(GameObject idObject)
    {
        return idObject.transform.parent.gameObject;
    }
    /// <summary>
    ///  Returns the player GameObject
    /// </summary>
    /// <returns>The player Gameobject</returns>
    public static GameObject GetPlayerGameObject()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }
    /// <summary>
    /// Gets the health component from the given object
    /// </summary>
    /// <param name="objectToGet">The object you want to get component on</param>
    /// <returns>The objects health component</returns>
    public static HealthComponent GetGameObjectHealthComponent(GameObject objectToGet)
    {
        return objectToGet.GetComponent<HealthComponent>();
    }
}