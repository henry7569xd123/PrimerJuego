using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    public float speed = 1f;
    public float minX;
    public float maxX;
    public float waitingTime = 2f;

    private GameObject _target;


    // Start is called before the first frame update
    void Start()
    {
        UpdateTarget();
        StartCoroutine("PatrolToTarget");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateTarget()
    {
        // Si en el primer tiempo creamos el target en la izquierda
        if(_target == null)
        {
            _target = new GameObject("Target");
            _target.transform.position = new Vector2(minX, transform.position.y);
            transform.localScale = new Vector3(-1, 1, 1);
            return;
        }

        // Si nosostros estamos en la izquierda, cambiamos el target a la derecha
        if(_target.transform.position.x == minX)
        {
            _target.transform.position = new Vector2(maxX, transform.position.y);
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Si nosostros estamos en la derecha, cambiamos el target a la izquierda
        else if (_target.transform.position.x == maxX)
        {
            _target.transform.position = new Vector2(minX, transform.position.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // Courotine para mover al enemigo
    private IEnumerator PatrolToTarget()
    {
        while(Vector2.Distance(transform.position, _target.transform.position) > 0.05f)
        {
            Vector2 direction = _target.transform.position - transform.position;
            float xDirection = direction.x;

            transform.Translate(direction.normalized * speed * Time.deltaTime);

            yield return null; 
        }

        // En este punto he alcanzado al target, vamos a setear nuestra posicion 
        //Debug.Log("Target alcanzado");
        transform.position = new Vector2(_target.transform.position.x, transform.position.y);

        // Y vamos a esperar un momento 
        //Debug.Log("Esperamos por " + waitingTime + "Segundos");
        yield return new WaitForSeconds(waitingTime);

        // Una vez esperando camos a restaurar el patrol
        //Debug.Log("Espera terminada, vamos a movernos de nuevo");
        UpdateTarget();
        StartCoroutine("PatrolToTarget");
    }
}
