using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager instance;

    public float sendInterval = 1;
    public long time;


    private float countdown;
    public float clouds;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(instance);
        }

        instance = this;
    }

    /// <summary>
    /// Sends time every [countdown] seconds.
    /// </summary>
    private void FixedUpdate()
    {
        countdown += Time.fixedDeltaTime;
        if (countdown >= sendInterval)
        {
            time++;
            countdown = 0;
            SendTime();
        }
        //if (time > 24) time = 0;
    }
    /// <summary>
    /// Sends time to client
    /// </summary>
    public void SendTime()
    {
        ServerSend.Time(time, 0.4f);
    }
}
