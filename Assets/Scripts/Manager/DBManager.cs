using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;

public class DBManager : Singleton<DBManager>
{
    string ipAddress = "localhost";
    string db_id = "root";
    string db_pw = "doopaang";
    string db_name = "gamedb";

    MySqlConnection conn = null;
    MySqlDataReader rdr = null;

    public string memeberId = null;

    public void Open()
    {
        string strConn = string.Format("server={0};uid={1};pwd={2};database={3};charset=utf8 ; Allow User Variables=true;", ipAddress, db_id, db_pw, db_name);

        conn = new MySqlConnection(strConn);
        conn.Open();
    }

    public void Close()
    {
        if (conn == null)
        {
            return;
        }
        conn.Close();
    }

    public MySqlDataReader ExcuteCommand(string command, params object[] parameter)
    {
        if (rdr != null)
        {
            rdr.Close();
        }
        MySqlCommand comm = new MySqlCommand(string.Format(command, parameter), conn);
        rdr = comm.ExecuteReader();
        return rdr;
    }

    public void ExecuteNonQuery(string query, params object[] parameter)
    {
        if (rdr != null)
        {
            rdr.Close();
        }
        MySqlCommand comm = new MySqlCommand(string.Format(query, parameter), conn);
        comm.ExecuteNonQuery();
    }
}
