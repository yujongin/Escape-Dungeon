using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject target;
    Vector3 offsetPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offsetPos =  transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + offsetPos;
    }
}
