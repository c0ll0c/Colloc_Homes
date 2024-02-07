using UnityEngine;

public class HandleCollider
{
    public string name;
    public GameObject collider;

    public HandleCollider(string _name, GameObject _collider)
    {
        this.name = _name;
        this.collider = _collider;
    }
}
