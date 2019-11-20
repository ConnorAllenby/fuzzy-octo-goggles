using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weapons;

namespace weapons
{
    public class GunScript : MonoBehaviour
    {
        public float damage = 10f;
        public float range = 100f;
        public Camera playerCamera;
        public float impactForce;
        public float rateOfFire;
        private float nextTimeToFire = 0f;
        public float ADS_Speed;
        public LayerMask mask;
        public bool invertMask;
        public ParticleSystem muzzleFlash;
        public GameObject impactEffect;
        public int MagazineSize;
        public int currentAmmo;
        public float reloadTime;

        [Range(0,1)]
        public float spreadFactor;
        float currentSpreadFactor;

        [SerializeField]
        GameObject weaponParent;
        public Vector3 currentWeaponPosition;
        public Vector3 ADSWeaponPosition;
        public Vector3 DefaultWeaponPosition;
        public Vector3 SprintWeaponPosition;

        public bool playerIsSprinting = false;
        public bool playerIsADS = false;

        public AudioSource myAudio;
        public AudioClip reloadSound;
        public AudioClip bulletSound;
        public ParticleSystem bulletTrail;
        // Use this for initialization

        private void Awake()
        {
            myAudio = gameObject.GetComponent<AudioSource>();
        }
        void Start()
        {
            weaponParent = GameObject.FindGameObjectWithTag("WeaponParent");
            playerCamera = Camera.main;
            currentAmmo = MagazineSize;



        }

        // Update is called once per frame
        void Update()
        {
            PlayerInput();
            weaponParent.transform.localPosition = currentWeaponPosition;

            LayerMask newMask = ~(invertMask ? ~mask.value : mask.value);
        }

        // Player Input.
        public void PlayerInput()
        {

            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / rateOfFire;
                Magazine();
                myAudio.clip = bulletSound;
                myAudio.PlayDelayed(0.1f);

            }

            if(Input.GetMouseButton(1) && !playerIsSprinting)
            {
               currentWeaponPosition =  Vector3.Lerp(currentWeaponPosition, ADSWeaponPosition, ADS_Speed);
               Camera.main.fieldOfView = 20;
                currentSpreadFactor = 0f;
            }
            else
            {
                currentWeaponPosition = Vector3.Lerp(currentWeaponPosition, DefaultWeaponPosition, ADS_Speed);
                Camera.main.fieldOfView = 60;
                currentSpreadFactor = spreadFactor;
            }

            if (Input.GetKey(KeyCode.R))
            {
                ReloadWeapon();
                myAudio.clip = reloadSound;
                myAudio.PlayDelayed(0.1f);
            }

        }

        //Shooting Logic.
        public void Shoot()
        {



            LayerMask newMask = ~(invertMask ? ~mask.value : mask.value);
            Vector3 weaponSpread = playerCamera.transform.forward;
            weaponSpread.x += Random.Range(-currentSpreadFactor, currentSpreadFactor);
            weaponSpread.y += Random.Range(-currentSpreadFactor, currentSpreadFactor);
            weaponSpread.z+= Random.Range(-currentSpreadFactor, currentSpreadFactor);
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position,weaponSpread, out hit, range, newMask))
            {
                EnemyController enemy = hit.transform.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        enemy.Die();
                    }

                Debug.Log(hit.transform.name);
                ShootyTarget target = hit.transform.GetComponent<ShootyTarget>();
                if (target != null)
                {
                    target.TakeDamage(10f);
                    
                }
                

                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

                Destroy(impactGo, 1f);
            }

        }

        //Logic for the magazing.
        //Checks the players current ammo.
        public void Magazine()
        {

            if (currentAmmo > 0)
            {
                Shoot();
                currentAmmo--;
                Debug.Log(currentAmmo);
                muzzleFlash.Play();
                bulletTrail.Play();
            }
            else if (currentAmmo <= 0)
            {
                currentAmmo = 0;
            }
        }

        //Reload Function, can be expanded later on.
        public void ReloadWeapon()
        {
            StartCoroutine(ReloadTime());
        }

        //Reload Time 
        IEnumerator ReloadTime()
        {

            yield return new WaitForSeconds(reloadTime);
            currentAmmo = MagazineSize;

        }
    }
}
