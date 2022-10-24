using UnityEngine;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;

public class GalleryManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private Transform odd;
    [SerializeField] private Transform even;

    void Start()
    {
        canvas.worldCamera = Camera.main;

        DBManager.Instance.Open();

        MySqlDataReader rdr = DBManager.Instance.ExcuteCommand("select productname, producthealth, productdamage, producthit, productdodge, productshot, productspeed from datatbl inner join producttbl on dataproduct = productname where datauser = '{0}' order by productname;", DBManager.Instance.memeberId);

        int i = 0;
        while (rdr.Read())
        {
            GameObject temp = Instantiate(panelPrefab, i % 2 == 0 ? even : odd);
            GalleryPanel gal = temp.GetComponent<GalleryPanel>();

            gal.image.sprite = Resources.Load<Sprite>("Sprites/" + rdr.GetString(0));
            gal.nameText.text = rdr.GetString(0);
            gal.healtText.text = rdr.GetInt32(1).ToString();
            gal.damageText.text = rdr.GetInt32(2).ToString();
            gal.hitText.text = rdr.GetInt32(3).ToString();
            gal.dodgeText.text = rdr.GetInt32(4).ToString();
            gal.shotSpeedText.text = rdr.GetInt32(5).ToString();
            gal.speedText.text = rdr.GetInt32(6).ToString();

            i++;
        }

        DBManager.Instance.Close();
    }

    public void Back()
    {
        MenuManager.Instance.ReturnMenu();

        SceneManager.UnloadSceneAsync("GalleryScene");
    }
}
