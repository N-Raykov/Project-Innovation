using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrangeContent : MonoBehaviour{

    [SerializeField] GridLayoutGroup group;
    [SerializeField] int columns;
    [SerializeField] float spacing;//from 0 to 1; 0 is no distance 1 is the entire width of the grid

    private void Awake(){

        RectTransform groupTransform = group.GetComponent<RectTransform>();
        group.constraintCount = columns;
        group.spacing = new Vector2(spacing * groupTransform.rect.width, spacing * groupTransform.rect.width);

        float remainingWidth = groupTransform.rect.width * (1f - spacing*(columns-1));

        group.cellSize = new Vector2(remainingWidth / columns, remainingWidth / columns);

    }


}
