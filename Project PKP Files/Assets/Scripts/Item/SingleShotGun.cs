using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            //Debug.Log("We Hit  " + hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((WeaponInfo)itemInfo).damage);

            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point , hit.normal);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 _hitPos, Vector3 _hitNormal)
    {
        //Debug.Log(_hitPos);

        Collider[] col = Physics.OverlapSphere(_hitPos, 0.3f);
        if(col.Length != 0)
        {
            GameObject bulletImpactObject = Instantiate(bulletImpactPrefabs, _hitPos + _hitNormal * 0.001f, Quaternion.LookRotation(_hitNormal, Vector3.up) * bulletImpactPrefabs.transform.rotation);
            Destroy(bulletImpactObject, 10f);
            bulletImpactObject.transform.SetParent(col[0].transform);
        }

       
    }
}
