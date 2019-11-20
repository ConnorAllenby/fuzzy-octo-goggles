using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace DebugSystemCollections
{
    public class DebugLogging : MonoBehaviour
    {

        // Use this for initialization

        public static TextMeshProUGUI Debug_PlayerMovement;
        public static TextMeshProUGUI Debug_Line02;
        public static TextMeshProUGUI Debug_Line03;
        void Awake()
        {
            Debug_PlayerMovement = GameObject.Find("DebugTextLine_01").GetComponent<TextMeshProUGUI>();
            Debug_Line02= GameObject.Find("DebugTextLine_02").GetComponent<TextMeshProUGUI>();
            Debug_Line03 = GameObject.Find("DebugTextLine_03").GetComponent<TextMeshProUGUI>();
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public static void UpdateTextline(TextMeshProUGUI textline, string newtext)
        {
            textline = textline.GetComponent<TextMeshProUGUI>();
            textline.text = newtext;
        }


    }
}
