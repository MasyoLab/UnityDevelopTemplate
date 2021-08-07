using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SQLiteUnity;

namespace SQLiteTest {

    public class Test : MonoBehaviour {

        // DB
        public static SQLite Database;

        // 出力先
        public static Text Console;

        // 準備
        private void Awake() {
            Console = GetComponentInChildren<Text>();
        }

        // テスト
        IEnumerator Start() {
            // 開始宣言
            Console.text = "SQLiteUnity Test Start\n\n";
            Debug.Log("Start");

            string uuid = string.Empty;

            var testController = new SQLiteTestController(new SQLite("TestDB.db"));
            yield return testController.CreateCoroutine("Test User", result => {
                Debug.Log($"UUID:{result["UUID"]}, Name:{result["Name"]}");
                uuid = $"{result["UUID"]}";
            });
            Debug.Log("==============================================================");

            yield return testController.GetUsersByNameCoroutine("Test User", result => {
                foreach (var item in result) {
                    Debug.Log($"UUID:{item["UUID"]}, Name:{item["Name"]}");
                }
            });
            Debug.Log("==============================================================");

            yield return testController.UpdateUserNameCoroutine(uuid, "新規プレイヤー");

            yield return testController.GetAllDataCoroutine(result => {
                foreach (var item in result) {
                    Console.text += $"ID:{item["UUID"]}, Name:{item["Name"]}\n";
                }
            });
        }
    }
}