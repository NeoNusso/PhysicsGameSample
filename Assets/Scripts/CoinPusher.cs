using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPusher : MonoBehaviour
{
    public Rigidbody _mover;
    public GameObject _coin;
    public float _width;
	// Update is called once per frame
	Vector3 _defaultPos;
	public float _coinSpawnWidth;
	private void Awake()
	{
		_defaultPos = _mover.position;
	}
	void FixedUpdate()
    {
#if true
		_mover.MovePosition( _defaultPos + new Vector3(0.0f, 0.0f, Mathf.Sin(Time.time) * _width) );
#else
		_mover.position = ( _defaultPos + new Vector3(0.0f, 0.0f, Mathf.Sin(Time.time) * _width) );
#endif
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var position = _coin.transform.position;
			position.x += Random.Range(-_coinSpawnWidth, _coinSpawnWidth);
			var inst = Instantiate(_coin, position, _coin.transform.rotation);
            inst.gameObject.SetActive(true);

        }
	}
}
