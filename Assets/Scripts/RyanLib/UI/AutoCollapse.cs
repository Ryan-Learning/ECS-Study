/*************************************************
  * 名稱：AutoCollapse
  * 作者：RyanHsu
  * 功能說明：UI邊緣自動收合器
  * ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ExtensionMethods;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RyanLib.UI
{

    public class AutoCollapse : MonoBehaviour
    {
        [ReadOnly] public Vector2 pivotPosition;
        public CanvasGroup mark;
        public float OpenArea;
        public float CloseArea;
        public float CollapseOffset;
        public areaType area;
        moveStatus status = moveStatus.Open;
        public float delaytime = 100f;
        float timeCount = 100f;
        RectTransform rect;

        private void Start()
        {
            rect = transform.rect();
            Cloase();
        }

        void Update()
        {
            if (status == moveStatus.Close) {
                switch (area) {
                    case areaType.Top:
                        if (rect.WorldToAnchorsPosition(Input.mousePosition).y > -OpenArea) Open();
                        break;
                    case areaType.Bottom:
                        if (rect.WorldToAnchorsPosition(Input.mousePosition).y < OpenArea) Open();
                        break;
                    case areaType.Left:
                        if (rect.WorldToAnchorsPosition(Input.mousePosition).x < OpenArea) Open();
                        break;
                    case areaType.Right:
                        if (rect.WorldToAnchorsPosition(Input.mousePosition).x > -OpenArea) Open();
                        break;
                }
            } else {
                switch (area) {
                    case areaType.Top:
                        if (rect.WorldToAnchorsPosition(Input.mousePosition).y < -CloseArea) Cloase();
                        break;
                    case areaType.Bottom:
                        if (rect.WorldToAnchorsPosition(Input.mousePosition).y > CloseArea) Cloase();
                        break;
                    case areaType.Left:
                        if (rect.WorldToAnchorsPosition(Input.mousePosition).x > CloseArea) Cloase();
                        break;
                    case areaType.Right:
                        if (rect.WorldToAnchorsPosition(Input.mousePosition).x < -CloseArea) Cloase();
                        break;
                }
            }
        }

        void Open()
        {
            rect.DOMove(pivotPosition, 0.5f);

            status = moveStatus.Open;
            timeCount = 0;
            if (mark) mark.DOAlpha(0f, 0.5f);
        }

        void Cloase()
        {
            if (timeCount < delaytime) {
                ++timeCount;
                return;
            }
            switch (area) {
                case areaType.Right:
                    rect.DOMove(pivotPosition + new Vector2(CollapseOffset, 0f), 0.5f).OnComplete(CompletedClose);
                    break;
                case areaType.Left:
                    rect.DOMove(pivotPosition - new Vector2(CollapseOffset, 0f), 0.5f).OnComplete(CompletedClose);
                    break;
                case areaType.Bottom:
                    rect.DOMove(pivotPosition - new Vector2(0f, CollapseOffset), 0.5f).OnComplete(CompletedClose);
                    break;
                case areaType.Top:
                    rect.DOMove(pivotPosition + new Vector2(0f, CollapseOffset), 0.5f).OnComplete(CompletedClose);
                    break;
                    void CompletedClose() { if (mark) mark.DOAlpha(1f, 0.5f); }
            }
            status = moveStatus.Close;
        }

        Vector3 areaVector
        {
            get {
                switch (area) {
                    case areaType.Top: return Vector3.up;
                    case areaType.Bottom: return Vector3.down;
                    case areaType.Left: return Vector3.left;
                    case areaType.Right: return Vector3.right;
                };
                return Vector3.zero;
            }
        }
#if UNITY_EDITOR
        public void OnValidate()
        {
            pivotPosition = transform.rect().anchoredPosition;
        }
#endif
    }

    public enum areaType { Top, Bottom, Left, Right }
    public enum moveStatus { Open, Close }

}
