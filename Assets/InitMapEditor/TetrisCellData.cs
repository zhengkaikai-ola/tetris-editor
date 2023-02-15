using System;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{


    public class TetrisCellData : MonoBehaviour
    {
        public Image Image;
        public int X;
        public int Y;
        public int typeId;

        public BlockType BlockType;

        private void Start()
        {
        }

        private void Update()
        {
        }

        public void SetBlockData(int mtypeId, BlockType blockType)
        {
            this.typeId = mtypeId;
            this.BlockType = blockType;
            // BlockType = blockType;
            // this.Arg0 = marg0;
            // this.Arg1 = marg1;
            // this.Arg2 = marg2;
            // this.Arg3 = marg3;
            if (blockType == BlockType.None)
            {
                Image.color = new Color(255, 255, 255, 1);
                var existTransform0 = this.Image.rectTransform.Find("TypeId");
                if (existTransform0 != null)
                {
                    GameObject.DestroyImmediate(existTransform0.gameObject);
                }

                return;
            }

            Image.color = new Color(0, 0, 255, 255);
            GameObject newObject = null;
            RectTransform rectTransform = null;
            Text text = null;
            var existTransform = this.Image.rectTransform.Find("TypeId");
            if (existTransform != null)
            {
                newObject = existTransform.gameObject;
                rectTransform = newObject.GetComponent<RectTransform>();
                text = newObject.GetComponent<Text>();
            }
            else
            {
                newObject = new GameObject("TypeId");

                rectTransform = newObject.AddComponent<RectTransform>();
                text = newObject.AddComponent<Text>();
                rectTransform.SetParent(this.Image.transform);
            }

            rectTransform.sizeDelta = new Vector2(20, 20);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.localPosition = new Vector3(0, 0);
            text.text = typeId.ToString();
            text.fontSize = 12;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.alignment = TextAnchor.MiddleCenter;
            text.color = new Color(255, 255, 255, 1);
        }
    }
}