using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpaceShipParams", order = 1)]
public class SpaceShipParams : ScriptableObject
{

    /**
     * SHIP
     */
     public float maxShipLife = 100f;
    public float shipLife = 100f;

    public float lifeRecovery = 3f;

    /**
     * Movement
     */
    public float speed = 5f;

    /**
     *  SHOOTING
     */
    public int gunDamage = 1;
    
    public float fireRate = 0.25f;
    
    public float weaponRange = 50f;

    public float hitForce = 100f;   

    public float bulletSpeed = 40f;

    /**
     * Barrel Roll
     */
    public float barrelRollTime = 1f;
}