using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelection : MonoBehaviour
{
    [SerializeField] private GameObject buttonOne, buttonTwo, buttonThree, buttonFour;
    [SerializeField] private GameObject selectionHighlightItem, positionButtonOne, positionButtonTwo, positionButtonThree, positionButtonFour;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonOne != null) {
            if (buttonOne == EventSystem.current.currentSelectedGameObject) {
                selectionHighlightItem.transform.position = positionButtonOne.transform.position;
            }
        } if (buttonTwo != null) {
            if (buttonTwo == EventSystem.current.currentSelectedGameObject) {
                selectionHighlightItem.transform.position = positionButtonTwo.transform.position;
            }
        } if (buttonThree != null) {
            if (buttonThree == EventSystem.current.currentSelectedGameObject) {
                selectionHighlightItem.transform.position = positionButtonThree.transform.position;
            }
        } if (buttonFour != null) {
            if (buttonFour == EventSystem.current.currentSelectedGameObject) {
                selectionHighlightItem.transform.position = positionButtonFour.transform.position;
            }
        }
    }
}
