using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public  delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    #region Sigleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerStats>();
            return instance;
        }
    }
    #endregion

    [SerializeField]
    public static float health;
    [SerializeField]
    public static float maxHealth;
    [SerializeField]
    public static float maxTotalHealth;

    public static float Health { get { return health; } }
    public static float MaxHealth { get { return maxHealth; } }
    public static float MaxTotalHealth { get { return maxTotalHealth; } }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Heal(float health)
    {
        PlayerStats.health += health;
        ClampHealth();
    }

    public static void TakeDamage(float dmg)
    {
        health -= dmg;
        Debug.Log("y3");
        ClampHealth();
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }   
    }

    public static void  ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}
