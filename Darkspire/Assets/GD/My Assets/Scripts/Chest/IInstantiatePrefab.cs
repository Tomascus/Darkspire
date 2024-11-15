
using UnityEngine;

public interface IInstantiatePrefab //any class that implements this interface must have the Instantiate method
{
  GameObject Instantiate(Transform transform); //force classes to have this method with where to create the obect 
}
