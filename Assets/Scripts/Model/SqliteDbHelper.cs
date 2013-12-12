using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System;
//.net framework2.0
using System.Data;

public class SqliteDBHelper
{
    /// <summary>
    /// declare a connection
    /// </summary>
    private SqliteConnection dbConnection;
    /// <summary>
    /// declare a dbCommonad
    /// </summary>
    private SqliteCommand dbCommand;
    /// <summary>
    /// declare reader
    /// </summary>
    private SqliteDataReader reader;

    /// <summary>
    /// connectionString to connect to an assigned db
    /// </summary>
    /// <param name="connectionString">connectionString to connect to an assigned db</param>
    public SqliteDBHelper (string connectionString, string password)
    {
        OpenDB (connectionString, password);
        LogHelper.Log(connectionString);
    }

    public void OpenDB (string connectionString, string password)
    {
        try
        {
            dbConnection = new SqliteConnection (connectionString);
            dbConnection.SetPassword(password);
            dbConnection.Open();
            LogHelper.Log ("Connected to db");
        }
        catch(Exception e)
        {
            string temp1 = e.ToString();
            LogHelper.Log(temp1);
        }
    }

    /// <summary>
    /// close the connection
    /// </summary>
    public void CloseSqlConnection ()
    {
        if (dbCommand != null)
        {
            dbCommand.Dispose();
        }
        dbCommand = null;
        if (reader != null)
        {
            reader.Dispose ();
        }
        reader = null;
        if (dbConnection != null)
        {
            dbConnection.Close ();
        }
        dbConnection = null;
        LogHelper.Log ("Disconnected from db.");
    }

    /// <summary>
    /// execute query 
    /// </summary>
    /// <param name="sqlQuery"></param>
    /// <returns></returns>
    public SqliteDataReader ExecuteQuery (string sqlQuery)
    {
        dbCommand = dbConnection.CreateCommand ();
        dbCommand.CommandText = sqlQuery;
        reader = dbCommand.ExecuteReader ();
        return reader;
    }

    /// <summary>
    /// query full table
    /// </summary>
    /// <param name="tableName">table name</param>
    /// <returns></returns>
    public SqliteDataReader ReadFullTable (string tableName)
    {
        string query = "SELECT * FROM " + tableName;
        return ExecuteQuery (query);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tableName">table name</param>
    /// <param name="values">col collections</param>
    /// <returns></returns>
    public SqliteDataReader InsertInto (string tableName, string[] values)
    {
        string query = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + values[i];
        }
        query += ")";
        return ExecuteQuery (query);
    }

    /// <summary>
    /// update
    /// </summary>
    /// <param name="tableName">table name</param>
    /// <param name="cols">cols</param>
    /// <param name="colsvalues">cols values</param>
    /// <param name="selectkey">select key</param>
    /// <param name="selectvalue">select values</param>
    /// <returns></returns>
    public SqliteDataReader UpdateInto (string tableName, string []cols,
                                        string []colsvalues, string selectkey, string selectvalue)
    {
        string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i) {
            query += ", " + cols + " =" + colsvalues[i];
        }
        query += " WHERE "+selectkey+" = "+selectvalue+" ";
        return ExecuteQuery (query);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tableName">table name</param>
    /// <param name="cols">cols</param>
    /// <param name="colsvalues">colsvalues</param>
    /// <returns></returns>
    public SqliteDataReader Delete(string tableName, string []cols, string []colsvalues)
    {
        string query = "DELETE FROM "+tableName + " WHERE " +cols[0] +" = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += " or " +cols+" = "+ colsvalues[i];
        }
        LogHelper.Log(query);
        return ExecuteQuery (query);
    }
    /// <summary>
    /// Insert
    /// </summary>
    /// <param name="tableName">table name</param>
    /// <param name="cols">cols</param>
    /// <param name="values">values</param>
    /// <returns></returns>
    public SqliteDataReader InsertIntoSpecific (string tableName, string[] cols,
                                                string[] values)
    {
        if (cols.Length != values.Length)
        {
            throw new SqliteException ("columns.Length != values.Length");
        }
        string query = "INSERT INTO " + tableName + "(" + cols[0];
        for (int i = 1; i < cols.Length; ++i)
        {
            query += ", " + cols;
        }
        query += ") VALUES (" + values[0];
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + values[i];
        }
        query += ")";
        return ExecuteQuery (query);
    }

    /// <summary>
    /// delete tables
    /// </summary>
    /// <param name="tableName">table name</param>
    /// <returns></returns>
    public SqliteDataReader DeleteContents (string tableName)
    {
        string query = "DELETE FROM " + tableName;
        return ExecuteQuery (query);
    }

    /// <summary>
    /// Creates the table.
    /// </summary>
    /// <returns>The table.</returns>
    /// <param name="name">Name.</param>
    /// <param name="col">Col.</param>
    /// <param name="colType">Col type.</param>
    public SqliteDataReader CreateTable (string name, string[] col, string[] colType)
    {
        if (col.Length != colType.Length)
        {
            throw new SqliteException ("columns.Length != colType.Length");
        }
        string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];
        for (int i = 1; i < col.Length; ++i)
        {
            query += ", " + col + " " + colType;
        }
        query += ")";
        LogHelper.Log(query);
        return ExecuteQuery (query);
    }


    /// <summary>
    /// query by filters
    /// </summary>
    /// <param name="tableName">table</param>
    /// <param name="items">query data collection</param>
    /// <param name="col">col</param>
    /// <param name="operation">operation</param>
    /// <param name="values">values</param>
    /// <returns></returns>
    public SqliteDataReader SelectWhere (string tableName, string[] items, string[] col, string[] operation, string[] values)
    {
        if (col.Length != operation.Length || operation.Length != values.Length)
        {
            throw new SqliteException ("col.Length != operation.Length != values.Length");
        }
        string query = "SELECT " + items[0];
        for (int i = 1; i < items.Length; ++i)
        {
            query += ", " + items;
        }
        query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
        for (int i = 1; i < col.Length; ++i)
        {
            query += " AND " + col + operation + "'" + values[0] + "' ";
        }
        return ExecuteQuery (query);
    }
}