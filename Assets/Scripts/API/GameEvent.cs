[System.Serializable]
public class GameEvent
{
    public string playerId;
    public string sessionId;
    public string eventType;

    public int score;
    public int slams;

    public float x;
    public float y;

    public string description;
    public string timestamp;
}