using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class GameObjectExtensions
{
	#region Instantiate

	public static GameObject Instantiate(this GameObject gameObject, Vector3 position)
    {
        return Instantiate(gameObject, position, Quaternion.identity, null);
    }

    public static GameObject Instantiate(this GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        return Instantiate(gameObject, position, rotation, null);
    }

    public static GameObject Instantiate(this GameObject gameObject, Vector3 position, Transform parent,
        bool worldPositionStays = true)
    {
        return Instantiate(gameObject, position, Quaternion.identity, parent, worldPositionStays);
    }

    public static GameObject Instantiate(this GameObject gameObject, Vector3 position, Quaternion rotation,
        Transform parent, bool worldPositionStays = true)
    {
        var instance = Object.Instantiate(gameObject, position, rotation) as GameObject;
        instance.transform.SetParent(parent, worldPositionStays);
        instance.transform.parent = parent;

        return instance;
    }

	#endregion

	#region LayerMask

	public static string LayerToName(this GameObject gameObject)
	{
		return LayerMask.LayerToName(gameObject.layer);
	}

	#endregion

	#region GetComponents

	public static IEnumerable<T> GetComponentsInChildrenOnly<T>(this GameObject gameObject) where T : MonoBehaviour
	{
		return gameObject.GetComponentsInChildren<T>().Where(x => x.transform != gameObject.transform);
	}

	#endregion
}