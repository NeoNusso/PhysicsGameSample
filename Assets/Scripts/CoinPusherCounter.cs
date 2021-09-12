using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinPusherCounter : MonoBehaviour
{
    int _count;
    public Text _text;

    void OnTriggerEnter(Collider collider)
    {
        _count++;
        _text.text = _count.ToString();
    }
}
