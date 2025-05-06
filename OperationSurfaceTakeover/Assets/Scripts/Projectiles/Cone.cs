using UnityEngine;

public class Cone : MonoBehaviour
{
    #region Fields
    [SerializeField]
    AudioClip flameAudio; //reference to the audio clip for the flame thrower

    AudioSource myAudioSource; //reference to this object's audio source

    [SerializeField]
    ParticleSystem thisProjectile; //reference to the partical system of this game object

    Color color; //reference to the color of this object

    int myDamage; //reference to th damage of this attack

    Timer destroyTimer; //reference to the timer for the life time of the attack

    [SerializeField]
    Collider2D areaCollider; //reference to the collider of the attack

    GameEvent damageEnemy = new GameEvent(); //support for events this script invokes
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference to this game object's audio source

        ParticleSystem.MainModule main = thisProjectile.main; //sets a referenct to the main module of the particle system attached to this game object

        main.startRotation = ((transform.rotation.z)); //sets the rotation of the particle system to that of this object

        color.a = 0f; //sets the alpha to 0

        gameObject.GetComponent<SpriteRenderer>().color = color; //sets this objects color alpha to 0

        EventManager.AddInvoker(GameplayEvent.DamageEnemy, damageEnemy); //events this script invokes

        //support for the destroy timer of this object
        destroyTimer = gameObject.AddComponent<Timer>();
        destroyTimer.Duration = 1;
        destroyTimer.Run();

        myAudioSource.PlayOneShot(flameAudio, 2f); //plays the audio of the flame thrower attack at 200 percent volume
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the destroy timer is finished
        if (destroyTimer.Finished)
        {
            //creates a list of all the objects inside the collider of this object
            Collider2D[] objectsInside = Physics2D.OverlapBoxAll(areaCollider.bounds.center, areaCollider.bounds.extents, 0);

            //goes through every object inside the collider
            foreach (Collider2D obj in objectsInside)
            {
                //checks if the object has the enemy tag
                if (obj.CompareTag("Enemy"))
                {
                    GameObject collidedObject = obj.gameObject; //sets the collided object to the enemy object

                    //invokes the damage enemy event while passing in the damage and the enemy in the collider as data
                    damageEnemy.AddData(GameplayEventData.Damage, myDamage);
                    damageEnemy.AddData(GameplayEventData.Enemy, collidedObject);
                    damageEnemy.Invoke(damageEnemy.Data);
                }
            }
            Destroy(gameObject); //destroys self
        }
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// initializes this objects stats
    /// </summary>
    /// <param name="damage"></param>
    public void Initialize(int damage)
    {
        myDamage = damage; //sets the damage amount
    }
    #endregion
}
