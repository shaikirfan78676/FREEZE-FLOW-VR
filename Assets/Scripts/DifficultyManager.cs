using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public float gameTime;

    public bool fastUnlocked;
    public bool knifeUnlocked;
    public bool gunUnlocked;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        // 1 minute → Fast enemies
        if(gameTime > 60)
            fastUnlocked = true;

        // 2.5 minutes → Knife
        if(gameTime > 150)
            knifeUnlocked = true;

        // 4 minutes → Guns (panic mode 😈)
        if(gameTime > 240)
            gunUnlocked = true;
    }
}
