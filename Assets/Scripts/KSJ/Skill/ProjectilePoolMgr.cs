using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolMgr : MonoBehaviour
{
    [SerializeField] float startCreateCount;
    [SerializeField] GameObject projectile;

    private List<Projectile> projectiles = new List<Projectile>();


    //설정해둔 startCreateCount만큼 투사체 사전생성
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

    //시전요청을 받게되면 만들어 둔 투사체Pool에서 사용중이 아닌(비활성) 오브젝트에 관련정보 제공하고 활성화
    //>> 추후 제작해 둔 Pool이 모두 사용중인 경우 추가 Pool을 생성하는 코드 필요
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
