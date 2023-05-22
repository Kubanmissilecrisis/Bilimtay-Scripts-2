using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyButton : MonoBehaviour
{
    public TongueMech tongueMech;
    public lizQuestionMechanics lizQuestionMech;

    public GameObject clickedCandy;// this one to send the object itself as a parameter

    private void Start()
    {
       // tongueMech = FindObjectOfType<TongueMech>();
    }

    public void OnClick()
    {
       // Vector3 targetPosition = transform.position;
        tongueMech.LaunchTongue(transform.position, clickedCandy);
        //Passes the Clickedcandy itself to the CheckAnswer method to get the initial pos of a candy to return it back to initial pos
        lizQuestionMech.ReceiveCandyPos(transform.position, clickedCandy);
    }
}
