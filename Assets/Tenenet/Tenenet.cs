using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Tenenet;
using CarterGames.Assets.LeaderboardManager;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;





public class Tenenet : MonoBehaviour
{
    public Text loadingText;
    public static bool loading = false;
    public static bool loadingLeaderboard = false;
    private const string URI = "http://api.tenenet.net";
    private const string token = "b01108d7d279d8c9ab00ec6edf686832";

    public LeaderboardDisplay displayScript;
    // Start is called before the first frame update

    public void Start()
    {
        //StartCoroutine(RegisterPlayer());
    }
    public static IEnumerator RegisterPlayer()
    {
        UnityWebRequest www = UnityWebRequest.Get(URI + "/createPlayer" + "?token=" + token + "&alias=" + LoggedInUser.alias + "&id=" + LoggedInUser.id + "&fname=" + LoggedInUser.fname + "&lname=" + LoggedInUser.lname);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log(www.error);
        }
    }
    public void gameOver()
    {
        StartCoroutine(InsertPlayerActivity());
    }
    public IEnumerator InsertPlayerActivity()
    {
        if (!loading)
        {
            loading = true;
            var score = GameManager.getScore();
            UnityWebRequest www = UnityWebRequest.Get(URI + "/insertPlayerActivity" + "?token=" + token + "&alias=" + LoggedInUser.alias + "&id=MatchScoreID&operator=add&value=" + score);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                loading = false;
                SceneManager.LoadScene("Login");
                
            }
            else
            {
                loading = false;
                Debug.Log(www.error);
            }
        }
        
    }

    public static IEnumerator GetPlayer(string alias,int rank)
    {
        UnityWebRequest www = UnityWebRequest.Get(URI + "/getPlayer" + "?token=" + token + "&alias=" + alias);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Assets.Tenenet.Player.Root myDeserializedClass = JsonConvert.DeserializeObject<Assets.Tenenet.Player.Root>(www.downloadHandler.text);
            var scoreValue =myDeserializedClass.Message.Score.FirstOrDefault().Value;
            LeaderboardManager.AddToLeaderboard(alias, float.Parse(scoreValue));
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    public void Leaderboard()
    {
        StartCoroutine(GetLeaderboard());
    }

    public IEnumerator GetLeaderboard()
    {
        if (!loadingLeaderboard)
        {
            loadingLeaderboard = true;
            LeaderboardManager.ClearLeaderboardData();
            UnityWebRequest www =
                UnityWebRequest.Get(URI + "/getLeaderboard" + "?token=" + token + "&id=GameLeaderboard");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Assets.Tenenet.Rank.Root myDeserializedClass =
                    JsonConvert.DeserializeObject<Assets.Tenenet.Rank.Root>(www.downloadHandler.text);
                foreach (var rank in myDeserializedClass.message.data)
                {
                    yield return StartCoroutine(GetPlayer(rank.alias, rank.rank));
                }

                displayScript.UpdateLeaderboardDisplay();
                loadingLeaderboard = false;
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }
}
