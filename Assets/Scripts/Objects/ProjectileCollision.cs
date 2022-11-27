using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Projectile Collision Data", menuName = "Projectile/Projectile Collision Data", order = 0)]
public class ProjectileCollision : ScriptableObject
{
    public List<CollisionData> colData;

    public string GetSoundForTag (string targetTagName)
    {
        if (colData.Any(t => t.tagName == targetTagName))
        {
            return colData.First(t => t.tagName == targetTagName).audioName;
        }
        return null;
    }
}
