using UnityEngine;

public interface IItemInteractable
{
    /// <summary>
    /// Interacts with object.
    /// </summary>
    /// <param name="gameObject">interacting gameobject</param>
    /// <returns>true if interaction was succesful, else false</returns>
    bool Interact(GameObject gameObject);
}