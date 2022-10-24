using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text nameText;

    public void Back()
    {
        MenuManager.Instance.ReturnMenu();
        SceneManager.UnloadSceneAsync("ShopScene");
    }

    public void Gatcha()
    {
        string get;

        int pay = PayCost();
        if(pay == -1)
        {
            return;
        }
        get = GetCharacterRandom();
        image.sprite = Resources.Load<Sprite>("Sprites/" + get);
        image.gameObject.SetActive(true);
        AddCharacterForUser(get);
    }

    private int PayCost()
    {
        int jewel = 0;
        int result = 0;
        DBManager.Instance.Open();
        MySqlDataReader rdr = DBManager.Instance.ExcuteCommand("select memberjewel from membertbl where memberid = '{0}';", DBManager.Instance.memeberId);
        while (rdr.Read())
        {
            jewel = rdr.GetInt32(0);
            if (jewel < 50)
            {
                result = -1;
                break;
            }
            jewel -= 50;
        }
        DBManager.Instance.ExecuteNonQuery("update membertbl set memberjewel = '{0}' where memberid = '{1}'", jewel, DBManager.Instance.memeberId);
        DBManager.Instance.Close();
        MenuManager.Instance.UpdateJewel();
        return result;
    }

    private string GetCharacterRandom()
    {
        string character = "";

        DBManager.Instance.Open();
        MySqlDataReader rdr = DBManager.Instance.ExcuteCommand("call gatcha();");

        while (rdr.HasRows)
        {
            while (rdr.Read())
            {
                character = rdr.GetString(0);
            }
            rdr.NextResult();
        }
        DBManager.Instance.Close();

        return character;
    }

    private void AddCharacterForUser(string character)
    {
        DBManager.Instance.Open();

        DBManager.Instance.ExecuteNonQuery("insert into `gamedb`.`datatbl` (`datauser`, `dataproduct`) values ('{0}', '{1}');", DBManager.Instance.memeberId, character);

        DBManager.Instance.Close();
    }
}
