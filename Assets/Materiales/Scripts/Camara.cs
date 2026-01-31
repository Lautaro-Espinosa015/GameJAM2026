using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform elPersonaje; //Elejir el personaje 
    public float suavizado = 5f;  //PAra que la Camara se vas rapida,suabe o mas lento
    public float distanciaZ = -15f; // Qué tan atrás está la cámara

    void LateUpdate()
    {
        // Adonde el personaje esta llendo
        // dirección que usa la X y la Y del personaje distancia Z
        Vector3 destino = new Vector3(elPersonaje.position.x, elPersonaje.position.y+100, distanciaZ);

        // 2. Movemos la cámara desde donde está hacia el "destino" poco a poco
        transform.position = Vector3.Lerp(transform.position, destino, suavizado * Time.deltaTime);
    }
}
