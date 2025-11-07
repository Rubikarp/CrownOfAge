using UnityEngine;

public class Transform_EmptyContent : MonoBehaviour
{
    public void Empty()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);
    }
}