using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject menu;

    [SerializeField] private Text welcomeMsg;
    [SerializeField] private Text jewel;

    private void Start()
    {
        welcomeMsg.text = "Welcome! " + DBManager.Instance.memeberId + "!";
        UpdateJewel();
    }

    public void SceneChange(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }
    public void SceneChangeAdd(string nextScene)
    {
        menu.SetActive(false);

        SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
    }

    public void ReturnMenu()
    {
        menu.SetActive(true);
    }

    public void UpdateJewel()
    {
        DBManager.Instance.Open();

        MySqlDataReader rdr = DBManager.Instance.ExcuteCommand("select memberjewel from membertbl where memberid = '{0}';", DBManager.Instance.memeberId);
        while (rdr.Read())
        {
            jewel.text = rdr.GetInt32(0).ToString();
        }

        DBManager.Instance.Close();
    }
}
