using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLiteUnity;
using UnityEngine.Events;

public class SQLiteTestController {
    const string TABLE_NAME = "test_table";
    SQLite _sqLite = null;

    public SQLiteTestController(SQLite sqLite) {
        _sqLite = sqLite;
    }

    public IEnumerator CreateCoroutine(string name, UnityAction<SQLiteRow> result = null) {
        var handle = new SQLiteRow {
            { "UUID", System.Guid.NewGuid() },
            { "Name", name },
        };
        var table = _sqLite.ExecuteQuery($"INSERT INTO {TABLE_NAME}(UUID, Name) VALUES(:UUID, :Name);", handle);

        if (table != null) {
            result?.Invoke(handle);
        }

        yield break;
    }

    public IEnumerator GetUsersByNameCoroutine(string userName, UnityAction<IList<SQLiteRow>> result = null) {
        var query = _sqLite.ExecuteQuery($"SELECT * FROM {TABLE_NAME} WHERE Name = '{userName}'");
        var resultData = new SQLiteRow();

        result?.Invoke(query.Rows);
        yield break;
    }

    public IEnumerator GetAllDataCoroutine(UnityAction<IList<SQLiteRow>> result = null) {
        var query = _sqLite.ExecuteQuery($"SELECT * FROM {TABLE_NAME}");
        var resultData = new SQLiteRow();
        result?.Invoke(query.Rows);
        yield break;
    }

    public IEnumerator UpdateUserNameCoroutine(string uuid, string userName) {
        _sqLite.ExecuteNonQuery($"UPDATE {TABLE_NAME} SET Name = '{userName}' WHERE UUID = '{uuid}'");
        yield break;
    }

    public IEnumerator DeleteCoroutine(string uuid, UnityAction<SQLiteRow> result = null) {
        _sqLite.ExecuteNonQuery($"DELETE FROM {TABLE_NAME} WHERE UUID = '{uuid}'");

        yield break;
    }

    public IEnumerator DeleteAll() {
        _sqLite.ExecuteNonQuery($"DELETE FROM {TABLE_NAME}");
        yield break;
    }
}