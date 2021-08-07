using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

/*
 * please don't use this code for sell a asset
 * user for free 
 * developed by Poya  @  http://gamesforsoul.com/
 * BLOB support by Jonathan Derrough @ http://jderrough.blogspot.com/
 * Modify and structure by Santiago Bustamante @ busta117@gmail.com
 * Android compatibility by Thomas Olsen @ olsen.thomas@gmail.com
 *
*/
namespace SQLiteUnity {

    /// <summary>
    /// 列型
    /// </summary>
    public enum SQLiteColumnType {
        SQLITE_UNKNOWN = 0,
        SQLITE_INTEGER = 1,
        SQLITE_FLOAT = 2,
        SQLITE_TEXT = 3,
        SQLITE_BLOB = 4,
        SQLITE_NULL = 5,
    }

    /// <summary>
    /// 結果コード
    /// </summary>
    public enum SQLiteResultCode {
        SQLITE_ABORT = 4,
        SQLITE_AUTH = 23,
        SQLITE_BUSY = 5,
        SQLITE_CANTOPEN = 14,
        SQLITE_CONSTRAINT = 19,
        SQLITE_CORRUPT = 11,
        SQLITE_DONE = 101,
        SQLITE_EMPTY = 16,
        SQLITE_ERROR = 1,
        SQLITE_FORMAT = 24,
        SQLITE_FULL = 13,
        SQLITE_INTERNAL = 2,
        SQLITE_INTERRUPT = 9,
        SQLITE_IOERR = 10,
        SQLITE_LOCKED = 6,
        SQLITE_MISMATCH = 20,
        SQLITE_MISUSE = 21,
        SQLITE_NOLFS = 22,
        SQLITE_NOMEM = 7,
        SQLITE_NOTADB = 26,
        SQLITE_NOTFOUND = 12,
        SQLITE_NOTICE = 27,
        SQLITE_OK = 0,
        SQLITE_PERM = 3,
        SQLITE_PROTOCOL = 15,
        SQLITE_RANGE = 25,
        SQLITE_READONLY = 8,
        SQLITE_ROW = 100,
        SQLITE_SCHEMA = 17,
        SQLITE_TOOBIG = 18,
        SQLITE_WARNING = 28,
        SQLITE_ABORT_ROLLBACK = 516,
        SQLITE_BUSY_RECOVERY = 261,
        SQLITE_BUSY_SNAPSHOT = 517,
        SQLITE_CANTOPEN_CONVPATH = 1038,
        SQLITE_CANTOPEN_DIRTYWAL = 1294,
        SQLITE_CANTOPEN_FULLPATH = 782,
        SQLITE_CANTOPEN_ISDIR = 526,
        SQLITE_CANTOPEN_NOTEMPDIR = 270,
        SQLITE_CONSTRAINT_CHECK = 275,
        SQLITE_CONSTRAINT_COMMITHOOK = 531,
        SQLITE_CONSTRAINT_FOREIGNKEY = 787,
        SQLITE_CONSTRAINT_FUNCTION = 1043,
        SQLITE_CONSTRAINT_NOTNULL = 1299,
        SQLITE_CONSTRAINT_PRIMARYKEY = 1555,
        SQLITE_CONSTRAINT_ROWID = 2579,
        SQLITE_CONSTRAINT_TRIGGER = 1811,
        SQLITE_CONSTRAINT_UNIQUE = 2067,
        SQLITE_CONSTRAINT_VTAB = 2323,
        SQLITE_CORRUPT_SEQUENCE = 523,
        SQLITE_CORRUPT_VTAB = 267,
        SQLITE_ERROR_MISSING_COLLSEQ = 257,
        SQLITE_ERROR_RETRY = 513,
        SQLITE_ERROR_SNAPSHOT = 769,
        SQLITE_IOERR_ACCESS = 3338,
        SQLITE_IOERR_BLOCKED = 2826,
        SQLITE_IOERR_CHECKRESERVEDLOCK = 3594,
        SQLITE_IOERR_CLOSE = 4106,
        SQLITE_IOERR_CONVPATH = 6666,
        SQLITE_IOERR_DELETE = 2570,
        SQLITE_IOERR_DELETE_NOENT = 5898,
        SQLITE_IOERR_DIR_CLOSE = 4362,
        SQLITE_IOERR_DIR_FSYNC = 1290,
        SQLITE_IOERR_FSTAT = 1802,
        SQLITE_IOERR_FSYNC = 1034,
        SQLITE_IOERR_GETTEMPPATH = 6410,
        SQLITE_IOERR_LOCK = 3850,
        SQLITE_IOERR_MMAP = 6154,
        SQLITE_IOERR_NOMEM = 3082,
        SQLITE_IOERR_RDLOCK = 2314,
        SQLITE_IOERR_READ = 266,
        SQLITE_IOERR_SEEK = 5642,
        SQLITE_IOERR_SHMLOCK = 5130,
        SQLITE_IOERR_SHMMAP = 5386,
        SQLITE_IOERR_SHMOPEN = 4618,
        SQLITE_IOERR_SHMSIZE = 4874,
        SQLITE_IOERR_SHORT_READ = 522,
        SQLITE_IOERR_TRUNCATE = 1546,
        SQLITE_IOERR_UNLOCK = 2058,
        SQLITE_IOERR_WRITE = 778,
        SQLITE_LOCKED_SHAREDCACHE = 262,
        SQLITE_LOCKED_VTAB = 518,
        SQLITE_NOTICE_RECOVER_ROLLBACK = 539,
        SQLITE_NOTICE_RECOVER_WAL = 283,
        SQLITE_OK_LOAD_PERMANENTLY = 256,
        SQLITE_READONLY_CANTINIT = 1288,
        SQLITE_READONLY_CANTLOCK = 520,
        SQLITE_READONLY_DBMOVED = 1032,
        SQLITE_READONLY_DIRECTORY = 1544,
        SQLITE_READONLY_RECOVERY = 264,
        SQLITE_READONLY_ROLLBACK = 776,
        SQLITE_WARNING_AUTOINDEX = 284,
    }

    public class SQLite : IDisposable {

        #region Plugin Access
        [DllImport("sqlite3", EntryPoint = "sqlite3_open")]
        private static extern SQLiteResultCode sqlite3_open(string filename, out IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_close")]
        private static extern SQLiteResultCode sqlite3_close(IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_prepare_v2")]
        private static extern SQLiteResultCode sqlite3_prepare_v2(IntPtr db, string zSql, int nByte, out IntPtr ppStmpt, IntPtr pzTail);

        [DllImport("sqlite3", EntryPoint = "sqlite3_step")]
        private static extern SQLiteResultCode sqlite3_step(IntPtr statement);

        [DllImport("sqlite3", EntryPoint = "sqlite3_finalize")]
        private static extern SQLiteResultCode sqlite3_finalize(IntPtr statement);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_count")]
        private static extern int sqlite3_column_count(IntPtr statement);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_name")]
        private static extern IntPtr sqlite3_column_name(IntPtr statement, int iCol);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_type")]
        private static extern SQLiteColumnType sqlite3_column_type(IntPtr statement, int iCol);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_int")]
        private static extern int sqlite3_column_int(IntPtr statement, int iCol);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_text")]
        private static extern IntPtr sqlite3_column_text(IntPtr statement, int iCol);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_double")]
        private static extern double sqlite3_column_double(IntPtr statement, int iCol);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_blob")]
        private static extern IntPtr sqlite3_column_blob(IntPtr statement, int iCol);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_bytes")]
        private static extern int sqlite3_column_bytes(IntPtr statement, int iCol);

        [DllImport("sqlite3", EntryPoint = "sqlite3_exec")]
        private static extern SQLiteResultCode sqlite3_exec(IntPtr db, string sql, IntPtr callback, IntPtr args, out IntPtr errorMessage);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_parameter_index")]
        private static extern int sqlite3_bind_parameter_index(IntPtr statement, string key);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_int")]
        private static extern SQLiteResultCode sqlite3_bind_int(IntPtr statement, int index, int val);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_text")]
        private static extern SQLiteResultCode sqlite3_bind_text(IntPtr statement, int index, byte[] value, int length, IntPtr freeType);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_blob")]
        private static extern SQLiteResultCode sqlite3_bind_blob(IntPtr statement, int index, byte[] value, int length, IntPtr freeType);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_double")]
        private static extern SQLiteResultCode sqlite3_bind_double(IntPtr statement, int index, double value);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_null")]
        private static extern SQLiteResultCode sqlite3_bind_null(IntPtr statement, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_free")]
        private static extern SQLiteResultCode sqlite3_free(IntPtr memory);

        [DllImport("sqlite3", EntryPoint = "sqlite3_errmsg")]
        private static extern IntPtr sqlite3_errmsg(IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_errcode")]
        private static extern SQLiteResultCode sqlite3_errcode(IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_extended_errcode")]
        private static extern SQLiteResultCode sqlite3_extended_errcode(IntPtr db);

        #endregion

        /// <summary>
        /// コネクションがある
        /// </summary>
        public bool IsOpen {
            get => _dbHandle != IntPtr.Zero;
            private set { if (!value) { _dbHandle = IntPtr.Zero; } }
        }

        /// <summary>
        /// DBハンドル
        /// </summary>
        IntPtr _dbHandle;

        /// <summary>
        /// DBファイルパス
        /// </summary>
        string _pathDB;

        /// <summary>
        /// 新規生成 (初期化クエリ) (既にあれば単に使う、元があればコピーして使う)
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="query"></param>
        /// <param name="path"></param>
        public SQLite(string dbName, string query = null, string path = null) {
            _dbHandle = IntPtr.Zero;
            _pathDB = System.IO.Path.Combine(path ?? Application.persistentDataPath, dbName);

            // 既に存在する
            if (System.IO.File.Exists(_pathDB)) {
                return;
            }
            // 存在しない場合は複製
            else {
                string sourcePath = System.IO.Path.Combine(Application.streamingAssetsPath, dbName);
                if (sourcePath.Contains("://")) {
                    // Android
                    UnityWebRequest www = UnityWebRequest.Get(sourcePath);
                    www.SendWebRequest();
                    while (!www.isDone && !www.isNetworkError && !www.isHttpError) { }
                    if (!www.isNetworkError && !www.isHttpError) {
                        System.IO.File.WriteAllBytes(_pathDB, www.downloadHandler.data);
                        return;
                    }
                }
                else if (System.IO.File.Exists(sourcePath)) {
                    // Mac, Windows, Iphone
                    System.IO.File.Copy(sourcePath, _pathDB, true);
                    return;
                }
            }
            if (string.IsNullOrEmpty(query)) {
                throw new ArgumentNullException("no query");
            }
            // 新規
            TransactionQueries(query);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose() {
            // 開いていない
            if (!IsOpen)
                return;
            Close();
        }

        /// <summary>DBを開く</summary>
        void Open() {
            // 既に開いている
            if (IsOpen)
                return;

            var result = sqlite3_open(_pathDB, out _dbHandle);
            if (result != SQLiteResultCode.SQLITE_OK) {
                IsOpen = false;
                throw new SQLiteException($"Could not open database file: {_pathDB} {result}");
            }
        }

        /// <summary>
        /// DBを閉じる
        /// </summary>
        void Close() {
            // 開いていない
            if (!IsOpen)
                return;

            sqlite3_close(_dbHandle);
            IsOpen = false;
        }

        /// <summary>
        /// 結果の不要なステートメントを実行する
        /// </summary>
        /// <param name="statement"></param>
        void ExecuteNonQuery(Statement statement) {
            if (!IsOpen)
                return;

            var result = sqlite3_step(statement.Pointer);
            if (result != SQLiteResultCode.SQLITE_DONE) {
                throw new SQLiteException($"Could not execute SQL statement. {result}");
            }
        }

        /// <summary>
        /// ステートメントを実行して結果行列を取得
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        SQLiteTable ExecuteQuery(Statement statement) {
            if (!IsOpen)
                return null;

            var pointer = statement.Pointer;
            var dataTable = new SQLiteTable();

            // 列の生成
            int columnCount = sqlite3_column_count(pointer);
            for (int i = 0; i < columnCount; i++) {
                string columnName = Marshal.PtrToStringAnsi(sqlite3_column_name(pointer, i));
                dataTable.AddColumn(columnName, SQLiteColumnType.SQLITE_UNKNOWN);
            }

            // 行の生成
            object[] row = new object[columnCount];

            while (sqlite3_step(pointer) == SQLiteResultCode.SQLITE_ROW) {
                for (int i = 0; i < columnCount; i++) {
                    if (dataTable.Columns[i].Type == SQLiteColumnType.SQLITE_UNKNOWN || dataTable.Columns[i].Type == SQLiteColumnType.SQLITE_NULL) {
                        dataTable.Columns[i].Type = sqlite3_column_type(pointer, i);
                    }
                    switch (dataTable.Columns[i].Type) {
                        case SQLiteColumnType.SQLITE_INTEGER:
                            row[i] = sqlite3_column_int(pointer, i);
                            break;
                        case SQLiteColumnType.SQLITE_TEXT:
                            IntPtr text = sqlite3_column_text(pointer, i);
                            row[i] = Marshal.PtrToStringAnsi(text);
                            break;
                        case SQLiteColumnType.SQLITE_FLOAT:
                            row[i] = sqlite3_column_double(pointer, i);
                            break;
                        case SQLiteColumnType.SQLITE_BLOB:
                            IntPtr blob = sqlite3_column_blob(pointer, i);
                            int size = sqlite3_column_bytes(pointer, i);
                            byte[] data = new byte[size];
                            Marshal.Copy(blob, data, 0, size);
                            row[i] = data;
                            break;
                        case SQLiteColumnType.SQLITE_NULL:
                            row[i] = null;
                            break;
                        default:
                            row[i] = null;
                            break;
                    }
                }
                dataTable.AddRow(row);
            }
            return dataTable;
        }

        /// <summary>
        /// トランザクション実行
        /// </summary>
        /// <param name="query"></param>
        void ExecuteTransaction(string query) {
            if (!IsOpen)
                return;

            IntPtr errorMessage;

            var result = sqlite3_exec(_dbHandle, query, IntPtr.Zero, IntPtr.Zero, out errorMessage);
            if (result != SQLiteResultCode.SQLITE_OK || errorMessage != IntPtr.Zero) {
                var str = $"Could not execute SQL statement. {result} '{errorMessage}' {sqlite3_extended_errcode(_dbHandle)}";
                sqlite3_free(errorMessage);
                throw new SQLiteException(str);
            }
        }

        #region HighLevelAPI

        /// <summary>
        /// 単文の変数を差し替えながら順に実行
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        public void ExecuteNonQuery(string query, SQLiteTable param) {
            foreach (SQLiteRow row in param) {
                ExecuteNonQuery(query, row);
            }
        }

        /// <summary>
        /// 単文を実行
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        public void ExecuteNonQuery(string query, SQLiteRow param = null) {
            // 元の状態
            var close = !IsOpen;

            Open();

            try {
                using (Statement statement = new Statement(this, query, param)) {
                    if (statement.Pointer != IntPtr.Zero) {
                        ExecuteNonQuery(statement);
                    }
                }
            }
            catch (SQLiteException e) {
                Debug.LogError($"SQLite: Can't ExecuteNonQuery {e}");
            }
            finally {
                // 元に戻す
                if (close)
                    Close();
            }
        }

        /// <summary>
        /// 単文を実行して結果を返す
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public SQLiteTable ExecuteQuery(string query, SQLiteRow param = null) {
            SQLiteTable result = null;
            var close = !IsOpen; // 元の状態

            Open();

            try {
                using (Statement statement = new Statement(this, query, param)) {
                    if (statement.Pointer != IntPtr.Zero) {
                        result = ExecuteQuery(statement);
                    }
                }
            }
            catch (SQLiteException e) {
                Debug.LogError($"SQLite: Can't ExecuteQuery {e}");
            }
            finally {
                // 元に戻す
                if (close)
                    Close();
            }
            return result;
        }

        /// <summary>
        /// 複文を一括実行し、誤りがあれば巻き戻す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool TransactionQueries<T>(T query) where T : IEnumerable<string> {
            return TransactionQueries(string.Join("\n", query));
        }

        /// <summary>
        /// 複文を一括実行し、誤りがあれば巻き戻す
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool TransactionQueries(string query) {
            var close = !IsOpen; // 元の状態

            try {
                Open();
                ExecuteTransaction($"BEGIN TRANSACTION;\n{query};\nCOMMIT;");
                return true;
            }
            catch (SQLiteException e) {
                ExecuteTransaction("ROLLBACK;");
                Debug.LogError($"SQLite: Can't Transaction, rollbacked {e}");
                return false;
            }
            finally {
                // 元に戻す
                if (close)
                    Close();
            }
        }

        #endregion

        /// <summary>
        /// SQLステートメント
        /// </summary>
        class Statement : IDisposable {
            SQLite _sqLite = null;

            IntPtr _pointer;
            public IntPtr Pointer => _pointer;

            /// <summary>
            /// SQLステートメントの生成
            /// </summary>
            /// <param name="database"></param>
            /// <param name="query"></param>
            /// <param name="param"></param>
            public Statement(SQLite database, string query, SQLiteRow param = null) {
                _sqLite = database;
                _pointer = IntPtr.Zero;
                if (_sqLite != null && !_sqLite.IsOpen) {
                    throw new SQLiteException("SQLite database is not open.");
                }
                if (sqlite3_prepare_v2(_sqLite._dbHandle, query, System.Text.Encoding.GetEncoding("UTF-8").GetByteCount(query), out _pointer, IntPtr.Zero) != SQLiteResultCode.SQLITE_OK) {
                    IntPtr errorMsg = sqlite3_errmsg(_sqLite._dbHandle);
                    throw new SQLiteException(Marshal.PtrToStringAnsi(errorMsg));
                }
                if (param != null) {
                    BindParameter(param);
                }
            }

            /// <summary>
            /// 破棄
            /// </summary>
            public void Dispose() {
                if (_sqLite != null && _pointer != IntPtr.Zero) {
                    var result = sqlite3_finalize(_pointer);
                    if (result != SQLiteResultCode.SQLITE_OK) {
                        throw new SQLiteException($"Could not finalize SQL statement. {result} {sqlite3_extended_errcode(_sqLite._dbHandle)}");
                    }
                }
            }

            /// <summary>
            /// ステートメントにSQL引数をバインドする 必要なら':'が補われる
            /// </summary>
            /// <param name="param"></param>
            void BindParameter(SQLiteRow param) {
                if (param == null)
                    return;

                foreach (string key in param.Keys) {
                    object val = param[key];
                    string name = (key[0] == ':' || key[0] == '@' || key[0] == '$') ? key : $":{key}";
                    if (val == null) {
                        sqlite3_bind_null(_pointer, sqlite3_bind_parameter_index(_pointer, name));
                    }
                    else if (val is string) {
                        sqlite3_bind_text(_pointer, sqlite3_bind_parameter_index(_pointer, name), System.Text.Encoding.UTF8.GetBytes((string)val), System.Text.Encoding.GetEncoding("UTF-8").GetByteCount((string)val), new IntPtr(-1));
                    }
                    else if (val is byte[]) {
                        sqlite3_bind_text(_pointer, sqlite3_bind_parameter_index(_pointer, name), (byte[])val, ((byte[])val).Length, new IntPtr(-1));
                    }
                    else if (val is float) {
                        sqlite3_bind_double(_pointer, sqlite3_bind_parameter_index(_pointer, name), (double)(float)val);
                    }
                    else if (val is double) {
                        sqlite3_bind_double(_pointer, sqlite3_bind_parameter_index(_pointer, name), (double)val);
                    }
                    else if (val.IsInt32()) {
                        sqlite3_bind_int(_pointer, sqlite3_bind_parameter_index(_pointer, name), (int)val);
                    }
                    // その他の型
                    else {
                        // デフォルト値ならNULL
                        if (val.Equals(val.GetType().GetDefaultValue())) {
                            sqlite3_bind_null(_pointer, sqlite3_bind_parameter_index(_pointer, name));
                        }
                        // 既定の文字列化
                        else {
                            // 日時の文字列化書式を制御
                            val = (val is DateTime) ? ((DateTime)val).ToString("yyyy-MM-dd HH:mm:ss") : val.ToString();
                            sqlite3_bind_text(_pointer, sqlite3_bind_parameter_index(_pointer, name), System.Text.Encoding.UTF8.GetBytes((string)val), System.Text.Encoding.GetEncoding("UTF-8").GetByteCount((string)val), new IntPtr(-1));
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 例外
    /// </summary>
    public class SQLiteException : Exception {
        public SQLiteException(string message) : base(message) { }
    }

    /// <summary>
    /// 列の定義
    /// </summary>
    public class ColumnDefinition {
        public string Name;
        public SQLiteColumnType Type;

        /// <summary>
        /// 要素を指定して生成
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public ColumnDefinition(string name, SQLiteColumnType type) {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return $"{Name} {ColumnTypeName[Type]}";
        }

        /// <summary>
        /// SQLite列型をSQL型名に変換
        /// </summary>
        public static readonly Dictionary<SQLiteColumnType, string> ColumnTypeName = new Dictionary<SQLiteColumnType, string> {
            { SQLiteColumnType.SQLITE_UNKNOWN,  "" },
            { SQLiteColumnType.SQLITE_INTEGER,  "INTEGER" },
            { SQLiteColumnType.SQLITE_FLOAT,    "REAL" },
            { SQLiteColumnType.SQLITE_TEXT,     "TEXT" },
            { SQLiteColumnType.SQLITE_BLOB,     "BLOB" },
            { SQLiteColumnType.SQLITE_NULL,     "" },
        };
    }

    /// <summary>
    /// 行のデータ / バインドパラメータ
    /// </summary>
    public class SQLiteRow : Dictionary<string, object> {

        #region Static
        /// <summary>
        /// 行がnullまたは空
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(SQLiteRow row) {
            return row == null || row.Count <= 0;
        }
        #endregion

        /// <summary>
        /// 列にアクセスするインデクサ (列名)
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public new object this[string columnName] {
            get {
                if (ContainsKey(columnName)) {
                    return base[columnName];
                }
                return null;
            }
            set {
                if (ContainsKey(columnName)) {
                    base[columnName] = value;
                }
            }
        }

        /// <summary>
        /// 要素の連結
        /// </summary>
        /// <param name="addition"></param>
        /// <returns></returns>
        public SQLiteRow AddRange(SQLiteRow addition) {
            foreach (var item in addition) {
                if (!ContainsKey(item.Key)) {
                    Add(item.Key, item.Value);
                }
            }
            return this;
        }

        public override string ToString() {
            var keyval = new List<string>();
            foreach (var key in Keys) {
                keyval.Add($"{{ {key}, {ToString(this[key])} }}");
            }
            return $"{{ {string.Join(", ", keyval)} }}";
        }
        static string ToString(object val) {
            if (val == null) {
                return "null";
            }
            else if (val is string || val is byte[]) {
                return $"\"{val}\"";
            }
            else {
                return val.ToString();
            }
        }
    }

    /// <summary>
    /// テーブルのデータ
    /// </summary>
    public class SQLiteTable {

        List<ColumnDefinition> _columns = new List<ColumnDefinition>();
        public IList<ColumnDefinition> Columns => _columns;

        List<SQLiteRow> _rows = new List<SQLiteRow>();
        public IList<SQLiteRow> Rows => _rows;

        #region Static
        /// <summary>
        /// テーブルがnullまたは空
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(SQLiteTable table) {
            return table == null || table._rows.Count <= 0;
        }
        #endregion

        public SQLiteTable() { }

        /// <summary>
        /// 列一覧からの生成
        /// </summary>
        /// <param name="columns"></param>
        public SQLiteTable(params ColumnDefinition[] columns) : this() {
            for (var i = 0; i < columns.Length; i++) {
                _columns.Add(columns[i]);
            }
        }

        /// <summary>
        /// 先頭行
        /// </summary>
        public SQLiteRow Top => (_rows.Count > 0) ? _rows[0] : null;

        /// <summary>
        /// 行にアクセスするインデクサ
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SQLiteRow this[int index] => (index >= 0 && index < _rows.Count) ? _rows[index] : null;

        /// <summary>
        /// セルにアクセスするインデクサ (行番号と列番号)
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public object this[int rowIndex, int columnIndex] {
            get {
                if (columnIndex >= 0 && columnIndex < _columns.Count) {
                    return _rows[rowIndex][_columns[columnIndex].Name];
                }
                return null;
            }
            set {
                if (columnIndex >= 0 && columnIndex < _columns.Count) {
                    _rows[rowIndex][_columns[columnIndex].Name] = value;
                }
            }
        }

        /// <summary>
        /// セルにアクセスするインデクサ (行番号と列名)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public object this[int index, string columnName] {
            get {
                return _rows[index][columnName];
            }
            set {
                _rows[index][columnName] = value;
            }
        }

        /// <summary>
        /// コレクション
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator() {
            for (var i = 0; i < _rows.Count; i++) {
                yield return _rows[i];
            }
        }

        /// <summary>
        /// 値を指定して行を加える
        /// </summary>
        /// <param name="values"></param>
        public void AddRow(object[] values) {
            if (values.Length != _columns.Count) {
                throw new IndexOutOfRangeException("The number of values in the row must match the number of column");
            }
            var row = new SQLiteRow();
            for (int i = 0; i < values.Length; i++) {
                row.Add(_columns[i].Name, values[i]);
            }
            _rows.Add(row);
        }

        /// <summary>
        /// 名前と型を指定して列を加える
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="values"></param>
        public void AddColumn(string name, SQLiteColumnType type, object[] values = null) {
            AddColumn(new ColumnDefinition(name, type), values);
        }

        /// <summary>
        /// 列の定義を指定して列を加える
        /// </summary>
        /// <param name="column"></param>
        /// <param name="values"></param>
        public void AddColumn(ColumnDefinition column, object[] values = null) {
            if (_columns.Exists(col => col.Name == column.Name)) {
                throw new ArgumentException($"The column name is already exist. '{column.Name}'");
            }
            _columns.Add(column);
            if (values != null) {
                if (values.Length != _rows.Count) {
                    throw new IndexOutOfRangeException("The number of values in the table must match the number of row");
                }
                for (var i = 0; i < _rows.Count; i++) {
                    _rows[i].Add(column.Name, values[i]);
                }
            }
        }

        public override string ToString() {
            return $"({string.Join(", ", _columns.ConvertAll(column => column.ToString()))})\n{string.Join("\n", _rows.ConvertAll(row => row.ToString()))}";
        }
    }
}