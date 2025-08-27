using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        // Pastikan objek selalu tegak lurus, tidak peduli rotasi parent-nya.
        transform.rotation = Quaternion.Inverse(transform.parent.rotation);
    }
}