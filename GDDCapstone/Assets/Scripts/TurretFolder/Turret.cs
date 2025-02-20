using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class Turret : MonoBehaviour
{
    private bool isSpawned = false;
    private bool inRange = false;

    [SerializeField]
    GameObject bullet;

    [SerializeField]
    GameObject myself;

    private GameObject thisBullet;

    private int cooldown = 2;
    private GameObject enemy;
    private Vector3 direction;
    private float angle;
    private float maxDistance;
    private GameObject closest;
    private GameObject[] enemies;
    private float distance;

    private PlayerControls controls;
    private InputAction place;
    private InputAction cancelPlace;

    private string enemyTag = "Enemy";

    private Timer fireSpeed;

    GameEvent bulletFired = new GameEvent();
    /// <summary>
    /// Runs When this object is enabled
    /// </summary>
    private void OnEnable()
    {
        controls.Enable();
        place.Enable();
        cancelPlace.Enable();
    }

    /// <summary>
    /// Runs When this object is disabled
    /// </summary>
    private void OnDisable()
    {
        controls.Disable();
        place.Disable();
        cancelPlace.Disable();
    }

    private void Awake()
    {
        controls = new PlayerControls();
        place = controls.Player.Place;
        cancelPlace = controls.Player.CancelPlace;


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        EventManager.AddListener(GameplayEvent.BulletSpawned, BulletSpawned);
        EventManager.AddInvoker(GameplayEvent.BulletFired, bulletFired);
        fireSpeed = gameObject.AddComponent<Timer>();
        fireSpeed.Duration = cooldown;
        fireSpeed.Run();
    }

    private void Update()
    {
        if (place.triggered)
        {
            isSpawned = true;
        }

        if (isSpawned == true)
        {
            enemy = FindClosestEnemy();
            if (enemy != null)
            {
                direction = enemy.transform.position - transform.position;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
                if (inRange == true && fireSpeed.Finished)
                {
                    thisBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    fireSpeed.Run();
                }
            }

        }

        

        if (isSpawned == false)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            transform.position = mousePos;
        }
    }

    private void BulletSpawned(Dictionary<System.Enum, object> data)
    {
        data.TryGetValue(GameplayEventData.Bullet, out object output);
        GameObject bullet = (GameObject)output;

        if (thisBullet == bullet)
        {
            bulletFired.AddData(GameplayEventData.Bullet, bullet);
            bulletFired.AddData(GameplayEventData.Enemy, enemy);
            bulletFired.Invoke(bulletFired.Data);
        }
    }

    private GameObject FindClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        closest = null;
        maxDistance = 7f;

        foreach (GameObject enemy in enemies)
        {
            distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);

            if (distance <= maxDistance)
            {
                inRange = true;
                maxDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

}
