using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SQLiteUnity {

    /// <summary>
    /// パーサのデリゲート
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <returns></returns>
    public delegate T Parser<T>(string str);

    /// <summary>
    /// SQLiteユーティリティ
    /// </summary>
    public static partial class SQLiteUtility {

        /// <summary>
        /// TableCell → 文字列 (なければ空文字列)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetColumn(this SQLiteRow row, string name) {
            if (!string.IsNullOrEmpty(name) && row != null && row[name] != null) {
                var val = row[name] as string;
                if (!string.IsNullOrEmpty(val)) {
                    return string.Copy(val);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// TableCell → インスタンスまたはEnum (なければ指定値)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetColumn<T>(this SQLiteRow row, string name, T defaultValue) {
            if (!string.IsNullOrEmpty(name) && row != null && row[name] != null) {
                if (typeof(T).IsEnum) {
                    var val = row[name] as string;
                    if (!string.IsNullOrEmpty(val)) {
                        return (T)System.Enum.Parse(typeof(T), val);
                    }
                }
                else {
                    var cell = row[name];
                    return cell.IsScalar() ? (T)cell : (T)Activator.CreateInstance(typeof(T), cell as string);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// TableCell → Parse → 値 (なければ指定値)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="parser"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetColumn<T>(this SQLiteRow row, Parser<T> parser, string name, T defaultValue = default) {
            if (!string.IsNullOrEmpty(name) && row != null && row[name] != null && parser != null) {
                var val = row[name] as string;
                if (!string.IsNullOrEmpty(val)) {
                    return parser(val);
                }
            }
            parser = null;
            return defaultValue;
        }

        /// <summary>
        /// 拡張バインド (トランザクション用)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>        
        public static string SQLiteBind(this string query, SQLiteRow param) {
            foreach (string key in param.Keys) {
                object val = param[key];
                string name = (key[0] == ':' || key[0] == '@' || key[0] == '$') ? key : $":{key}";
                string str;
                if (val == null) {
                    str = "NULL";
                }
                else if (val.IsScalar()) {
                    str = val.ToString();
                }
                else if (val.Equals(val.GetType().GetDefaultValue())) {
                    str = "NULL";
                }
                else if (val is DateTime) {
                    str = $"'{((DateTime)val).ToString("yyyy-MM-dd HH:mm:ss")}'";
                }
                else {
                    str = $"'{val.ToString().Replace("'", "''")}'";
                }
                query = query.Replace(name, str);
            }
            return query;
        }

        /// <summary>
        /// スカラー型か判定
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsScalar<T>(this T val) {
            return val is int || val is uint || val is short || val is ushort || val is long || val is ulong || val is byte || val is sbyte || val is float || val is double;
        }

        /// <summary>
        /// 型のデフォルト値を得る
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type) {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// 32bitまでの整数型か判定
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsInt32<T>(this T val) {
            return val is int || val is uint || val is short || val is ushort || val is byte || val is sbyte;
        }
    }
}
