using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;

public class LogInManager : MonoBehaviour
{
    [SerializeField] private Text idText;
    [SerializeField] private InputField passText;

    [SerializeField] private GameObject registerPanel;
    [SerializeField] private Text newIdText;
    [SerializeField] private InputField newPasswordText;

    [SerializeField] private Text errorMsg;
    [SerializeField] private Text registerErrorMsg;

    [SerializeField] private string nextScene;

    public void LogIn()
    {
        string id = idText.text;
        string pass = passText.text;

        if (string.IsNullOrEmpty(id))
        {
            registerErrorMsg.text = "'User' is empty!";
            registerErrorMsg.gameObject.SetActive(true);
            return;
        }
        else if (string.IsNullOrEmpty(pass))
        {
            registerErrorMsg.text = "'Password' is empty!";
            registerErrorMsg.gameObject.SetActive(true);
            return;
        }

        DBManager.Instance.Open();

        MySqlDataReader rdr = DBManager.Instance.ExcuteCommand("select memberid from membertbl where memberid='{0}';", id);
        if (!rdr.Read())
        {
            registerErrorMsg.text = "No matching account found!";
            registerErrorMsg.gameObject.SetActive(true);
            DBManager.Instance.Close();
            return;
        }

        rdr = DBManager.Instance.ExcuteCommand("select memberid from membertbl where memberid='{0}' and memberpass='{1}';", id, pass);
        if (!rdr.Read())
        {
            registerErrorMsg.text = "The password is incorrect!";
            registerErrorMsg.gameObject.SetActive(true);
            DBManager.Instance.Close();
            return;
        }

        DBManager.Instance.Close();

        Debug.Log("로그인 성공! 계정 : " + id);
        DBManager.Instance.memeberId = id;
        SceneManager.LoadScene(nextScene);
    }

    public void Register()
    {
        registerPanel.SetActive(true);
    }

    public void RegisterNewAccount()
    {
        string id = newIdText.text;
        string pass = newPasswordText.text;

        if (id.Length == 0)
        {
            registerErrorMsg.text = "'User' is empty!";
            registerErrorMsg.gameObject.SetActive(true);
            return;
        }
        else if (pass.Length == 0)
        {
            registerErrorMsg.text = "'Password' is empty!";
            registerErrorMsg.gameObject.SetActive(true);
            return;
        }

        DBManager.Instance.Open();

        MySqlDataReader rdr = DBManager.Instance.ExcuteCommand("select memberid from membertbl;");
        while (rdr.Read())
        {
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                if ((string)rdr[i] == id)
                {
                    registerErrorMsg.text = "This account already exists!";
                    registerErrorMsg.gameObject.SetActive(true);
                    DBManager.Instance.Close();
                    return;
                }
            }
        }

        DBManager.Instance.ExecuteNonQuery("insert into `gamedb`.`membertbl` (`memberid`, `memberpass`, `memberjewel`) values ('{0}', '{1}', '{2}');", id, pass, 100);

        DBManager.Instance.Close();

        CloseRegisterPenel();
    }

    public void CloseRegisterPenel()
    {
        registerErrorMsg.gameObject.SetActive(false);
        registerPanel.SetActive(false);

        newIdText.text = "";
        newPasswordText.text = "";
    }
}
