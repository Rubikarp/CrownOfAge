using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WrappingHorizontalLayoutGroup : LayoutGroup
{
    public Vector2 Spacing = Vector2.one * 10f;
    private Vector2 contentSize = Vector2.zero;
    private Vector2 containerSize = Vector2.zero;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        ArrangeChildren();
    }
    public override void CalculateLayoutInputVertical()
    {
        ArrangeChildren();
    }

    public override void SetLayoutHorizontal()
    {
        ArrangeChildren();
    }
    public override void SetLayoutVertical()
    {
        ArrangeChildren();
    }

    private void ArrangeChildren()
    {
        contentSize = Vector2.zero;
        containerSize = new Vector2(
            rectTransform.rect.width - (padding.left + padding.right),
            rectTransform.rect.height - (padding.top + padding.bottom)
        );
        Vector2 currentPos = new Vector2(padding.left, padding.top);
        Vector2 currentLineMaxSize = Vector2.zero;

        // Store children in the current line
        var currentLineChildren = new List<RectTransform>();

        RectTransform child;
        for (int i = 0; i < rectChildren.Count; i++)
        {
            child = rectChildren[i];
            if (child == null || !child.gameObject.activeSelf) continue;

            float childWidth = child.rect.width;
            float childHeight = child.rect.height;

            // Check if the child fits in the current row
            if (currentPos.x + childWidth > containerSize.x)
            {
                currentLineMaxSize.x -= Spacing.x; // Remove last spacing

                // Align the current line's children
                AlignLine(currentLineChildren, currentLineMaxSize.x, containerSize.x);

                // Move to the next row
                currentPos.x = padding.left;
                currentPos.y += currentLineMaxSize.y + Spacing.y;

                // Update content size for the completed row
                contentSize.x = Mathf.Max(contentSize.x, currentLineMaxSize.x);
                contentSize.y = currentPos.y;
                currentLineMaxSize = Vector2.zero;

                // Clear the current line's children
                currentLineChildren.Clear();
            }

            // Add the child to the current line
            currentLineChildren.Add(child);

            // Set the child's position
            SetChildAlongAxis(child, 0, currentPos.x);
            SetChildAlongAxis(child, 1, currentPos.y);

            // Update the current position and row height
            currentPos.x += childWidth + Spacing.x;

            currentLineMaxSize.x = Mathf.Max(currentLineMaxSize.x, currentPos.x - padding.left);
            currentLineMaxSize.y = Mathf.Max(currentLineMaxSize.y, childHeight);
        }

        // Final alignment for the last row
        currentLineMaxSize.x -= Spacing.x; // Remove last spacing
        AlignLine(currentLineChildren, currentLineMaxSize.x, containerSize.x);

        // Final update for the last row
        contentSize.x = Mathf.Max(contentSize.x, currentLineMaxSize.x);
        contentSize.y = Mathf.Max(contentSize.y, currentPos.y + currentLineMaxSize.y);

        // Offset children based on vertical alignment
        AlignementBloc();
        SetPreferedSize();
    }

    private void SetPreferedSize()
    {
        // Utilise la méthode SetLayoutInputForAxis pour définir les tailles préférées
        // Axe 0 = horizontal, Axe 1 = vertical
        Vector2 totalPreferredSize = new Vector2(
            containerSize.x + (padding.left + padding.right), 
            containerSize.y + (padding.top + padding.bottom)
            );

        SetLayoutInputForAxis(containerSize.x, totalPreferredSize.x, totalPreferredSize.x, 0);
        SetLayoutInputForAxis(containerSize.y, totalPreferredSize.y, totalPreferredSize.y,  1);
    }

    private void AlignLine(List<RectTransform> lineChildren, float lineWidth, float containerWidth)
    {
        float remainingSpace = containerWidth - lineWidth;
        float offsetX = 0;

        switch (childAlignment)
        {
            case TextAnchor.UpperLeft:
            case TextAnchor.MiddleLeft:
            case TextAnchor.LowerLeft:
                // No offset needed for left alignment
                offsetX = 0;
                break;

            case TextAnchor.UpperCenter:
            case TextAnchor.MiddleCenter:
            case TextAnchor.LowerCenter:
                offsetX = remainingSpace / 2;
                break;

            case TextAnchor.UpperRight:
            case TextAnchor.MiddleRight:
            case TextAnchor.LowerRight:
                offsetX = remainingSpace;
                break;
        }

        // Apply the horizontal offset to each child in the line
        foreach (var child in lineChildren)
        {
            child.anchoredPosition += new Vector2(offsetX, 0);
        }
    }
    private void AlignementBloc()
    {
        Vector2 remainingSpace = containerSize - contentSize;

        //Don't have to horizontaly align, already done in AlignLine
        foreach (var child in rectChildren)
        {
            Vector2 offset = Vector2.zero;
            switch (childAlignment)
            {
                case TextAnchor.UpperLeft:
                case TextAnchor.UpperCenter:
                case TextAnchor.UpperRight:
                    break;

                case TextAnchor.MiddleLeft:
                case TextAnchor.MiddleCenter:
                case TextAnchor.MiddleRight:
                    offset = Vector2.down * remainingSpace.y / 2;
                    break;

                case TextAnchor.LowerLeft:
                case TextAnchor.LowerCenter:
                case TextAnchor.LowerRight:
                    offset = Vector2.down * remainingSpace.y;
                    break;
            }

            // Apply the offset to the child's position
            child.anchoredPosition += offset;
        }
    }
}
