/*************************************************
  * 名稱：FlexibleGridLayout
  * 作者：RyanHsu
  * 功能說明：延申自UI.LayoutGroup，更自由的排佈Group
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//禁止多載         [DisallowMultipleComponent]
//if UNITY_EDITOR
//執行階段執行     [ExecuteAlways]
//#endif
//依賴項           [RequireComponent(typeof CLASS)]
public class FlexibleGridLayout : LayoutGroup
{
    public FitStatus fitStatus;
    public Vector2 spacing;
    [ReadOnly("fitStatus", (int)FitStatus.FixedColumns)] public int columns;//行數
    [ReadOnly("fitStatus", (int)FitStatus.FixedRows)] public int rows;//列數
    [ReadOnly] public Vector2 cellSize;//大小
    [ReadOnly("fitStatus", (int)FitStatus.FixedColumns, (int)FitStatus.FixedRows)] public bool fitX;
    [ReadOnly("fitStatus", (int)FitStatus.FixedColumns, (int)FitStatus.FixedRows)] public bool fitY;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        int count = transform.childCount;
        if (count == 0) return;

        float sqrRt = Mathf.Sqrt(count);
        if (fitStatus == FitStatus.Uniform || fitStatus == FitStatus.Width || fitStatus == FitStatus.Height) {
            fitX = fitY = true;
            rows = columns = Mathf.CeilToInt(sqrRt);//由平旁根計算長寬cell個數
        }

        if (fitStatus == FitStatus.Width || fitStatus == FitStatus.FixedColumns) {
            rows = Mathf.CeilToInt(count / (float)columns);
        }

        if (fitStatus == FitStatus.Height || fitStatus == FitStatus.FixedRows) {
            columns = Mathf.CeilToInt(count / (float)rows);
        }

        Rect rect = rectTransform.rect;
        float cellWidth = (rect.width - spacing.x * (columns - 1) - padding.left - padding.right) / (float)columns;//單位尺寸，寬
        float cellHeight = (rect.height - spacing.y * (rows - 1) - padding.top - padding.bottom) / (float)rows;//單位尺寸，高
        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++) {
            rowCount = i / columns;//i所在的行數
            columnCount = i - rowCount * columns;//rowCount行的i餘列數
            RectTransform item = rectChildren[i];
            float xPos = (cellSize.x + spacing.x) * columnCount + padding.left;
            float yPos = (cellSize.y + spacing.y) * rowCount + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }

    }

    public override void CalculateLayoutInputVertical()
    {
        //throw new System.NotImplementedException();
    }

    public override void SetLayoutHorizontal()
    {
        //throw new System.NotImplementedException();
    }

    public override void SetLayoutVertical()
    {
        //throw new System.NotImplementedException();
    }
    public enum FitStatus
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns,
    }

#if UNITY_EDITOR
    //void OnValidate() { }
    //void OnDrawGizmos() { }
    //private void OnDrawGizmosSelected() { }
#endif
}
