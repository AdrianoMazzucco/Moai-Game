using System.Collections;
using QFSW.MOP2;
using UnityEngine;

public static class Util
{
    public static Vector3 GetRandomPointOnTerrain(Terrain terrain, Vector2 minXZ, Vector2 maxXZ) 
    {
        var point = new Vector3(Random.Range(minXZ.x, maxXZ.x),
            0,
            Random.Range(minXZ.y, maxXZ.y));

        point.y = terrain.SampleHeight(point) + terrain.GetPosition().y;

        return point;
    }

    public static Vector2 GetRandomPointBetweenTwoRadii(float InnerRadius, float OuterRadius)
    {
        return Random.insideUnitCircle.normalized * Random.Range(InnerRadius, OuterRadius);
    }

    public static IEnumerator ReleaseAfterDelay(ObjectPool pool, GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        pool.Release(gameObject);
    }

    public static bool CompareWithLayerMask(LayerMask mask,GameObject go)
    {

        return ((mask.value & (1 << go.layer)) > 0);
    }
    
    
    
}
