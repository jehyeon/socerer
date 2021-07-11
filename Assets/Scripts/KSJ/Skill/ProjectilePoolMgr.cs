using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolMgr : MonoBehaviour
{
    [SerializeField] float startCreateCount;

    [SerializeField] GameObject projectile;


    private List<Projectile> projectiles = new List<Projectile>();

    private GameObject tempObject;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < startCreateCount; i++)
        {
            /*
            tempObject = Instantiate(projectile, transform);
            tempObject.SetActive(false);*/

            projectiles.Add(Instantiate(projectile, transform).GetComponent<Projectile>());
        }
    }

    // Update is called once per frame
    public void ActiveSkill(int _id, int _casterInstanceID, Vector3 _position, Quaternion _rotation)
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            if(!projectiles[i].gameObject.activeSelf)
            {
                projectiles[i].ActiveProjectile(_id, _casterInstanceID, _position, _rotation);
                break;
            }          
        }
    }
}
