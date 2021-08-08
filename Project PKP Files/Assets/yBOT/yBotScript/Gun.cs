using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 1f;
    public float range = 100f;
    public float fireRate = 15f;
    public Camera fpsCam;
    public ParticleSystem muzzle;
    public GameObject impactEffect;
    public float impactForce = 30f;
    private float nextTimeToFire = 0f;
    
    public Animator animRecoil;
    int RecoilAnimation;
    public float animationPlayTransition = 0.15f;

  
    private void Start()
    {
        RecoilAnimation = Animator.StringToHash("Rifle Recoil");
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            
        }   

    }

    private void Shoot()
    {
        //animRecoil.CrossFade(RecoilAnimation, animationPlayTransition);
        muzzle.Play();
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {          
            Debug.Log(hit.transform.name); 

            Target target = hit.transform.GetComponent<Target>();

            if(target != null)
            {
                target.takeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal * impactForce);
            }

            animRecoil.CrossFade(RecoilAnimation, animationPlayTransition);

            GameObject impactGameObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameObject, 1f);
            
        }

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.name == "Player")
    //    {
    //        Destroy(impactEffect, 0f);
    //    }
    //}
}
