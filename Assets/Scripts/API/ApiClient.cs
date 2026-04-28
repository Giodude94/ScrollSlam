using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ApiClient : MonoBehaviour
{
    public static ApiClient Instance;

    public string baseUrl = "http://localhost:5000/api/v1";

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SendEvent(string json)
    {
        StartCoroutine(PostRequest("/events", json));
    }

    IEnumerator PostRequest(string endpoint, string json)
    {
        var request = new UnityWebRequest(baseUrl + endpoint, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("API Error: " + request.error);
        }
        else
        {
            Debug.Log("Event sent successfully");
        }
    }
}