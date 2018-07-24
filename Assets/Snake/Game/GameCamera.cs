
using UnityEngine;

public class GameCamera : MonoBehaviour
{


    public static uint FocusPlayerId = 0;

    public static void Create()
    {
                
    }

    void Start()
    {

    }

    void Update()
    {
        if (GameManager.Instance.IsRunning)
        {
            SnakePlayer player = GameManager.Instance.GetPlayer(FocusPlayerId);

            if (player != null)
            {
                Vector3 pos = player.Head.Position();
                pos.z = this.transform.position.z;
                this.transform.position = pos;
            }

        }
    }

}
