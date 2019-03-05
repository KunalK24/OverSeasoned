﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FloatUnityEvent : UnityEvent<float> { }
public class Table : MonoBehaviour
{
    private Vector3 sphereCastPos;
    public Vector3 direction;
    public float radius;
    public bool occupied;
    public GameObject customer;
    public GameObject dish;
    public TextMeshProUGUI OrderText;

    public FloatUnityEvent OnScoreEvent = new FloatUnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        radius = .90f;
        sphereCastPos = this.transform.position;
        direction = transform.TransformDirection(new Vector3(0, 1f, 0));
        occupied = false;
        Debug.DrawRay(sphereCastPos, direction);
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDish();
    }

    public void SetCustomer(GameObject customer)
    {
        this.customer = customer;

        OrderText.text =
            $"Customer Order:\n {customer.GetComponent<Customer>().food} w\\ Spice Level: {customer.GetComponent<Customer>().spiceLevel}";
    }

    void CheckDish()
    {
        int layerMask = 1 << 8;

        if (Physics.SphereCast(transform.position-new Vector3(0,2,0), radius, direction, out RaycastHit hit, Mathf.Infinity,layerMask))
        {
            if (hit.collider.CompareTag("Dish") && Input.GetKeyDown(KeyCode.E) && dish == null)
            {
                dish = hit.collider.gameObject;
                hit.collider.gameObject.transform.parent.gameObject.GetComponent<Player>().item=null;
                dish.transform.SetParent(transform);
                dish.transform.localPosition = new Vector3(-1, dish.GetComponent<Renderer>().bounds.size.y*3.5f, 0);
                dish.GetComponent<Rigidbody>().detectCollisions = false;
//                if (hit.collider.gameObject.name == customer.GetComponent<Customer>().food)
//                {
//                    //customer.GetComponent<Customer>().spiceLevel == hit.collider.gameObject.GetComponent<Dish>().Spiciness
//                    print("success");
//                }
                OnScoreEvent.Invoke(10);

            }
        }
    }
}
