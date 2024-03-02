
using UnityEngine;

public interface ITypeMove
{
    Vector3 Move();
}

public class TypeMoveController : MoveController
{
    ITypeMove[] typeMoves;
    [SerializeField]
    Transform body;

    private void Start()
    {
        typeMoves = GetComponents<ITypeMove>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        foreach (ITypeMove typeMove in typeMoves)
        {
            direction += typeMove.Move();
        }

        body.transform.up = direction;
        Move(direction);
    }
}
