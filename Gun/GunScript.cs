using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weapons;

namespace weapons
{
    public class GunScript : MonoBehaviour
    {
        #region References
        PlayerController PCRef;
        #endregion
        #region variables
        //Gunstats
        [Header("Gunstats")]
        public float damage = 10f;
        public float range = 100f;
        public float rateOfFire;
        public int MagazineSize;
        public int currentAmmo;
        public float reloadTime;
        public float impactForce;
        public Camera playerCamera;
        private float nextTimeToFire = 0f;
        [Range(0, 1)]
        public float spreadFactor;
        float currentSpreadFactor;
        public Vector3 weaponSpread;


        //Effects
        [Header("Effects")]
        public ParticleSystem bulletTrail;
        public float bulletTrailSpeed;
        public Transform MuzzleShotTransform;
        public LayerMask mask;
        public bool invertMask;
        public ParticleSystem muzzleFlash;
        public GameObject impactEffect;

        //Weapon Feel
        [Header("WeaponFeel")]
        [SerializeField]
        GameObject weaponParent;
        public Vector3 currentWeaponPosition;
        public Vector3 ADSWeaponPosition;
        public Vector3 DefaultWeaponPosition;
        public Vector3 SprintWeaponPosition;
        public float ADS_Speed;

        //Bool Checks
        [Header("BoolChecks")]
        public bool playerIsSprinting = false;
        public bool playerIsADS = false;

        //Audio
        [Header("Audio")]
        public AudioSource myAudio;
        public AudioClip reloadSound;
        public AudioClip bulletSound;
        public AudioClip lastBulletSound;

        //Animation
        Animator GunAnimator;
        List<string> AnimTriggers = new List<string>();


        //FSM State Reference
        public readonly Player_IdleState playerIdleState = new Player_IdleState();
        public readonly Player_RunningState playerRunningState = new Player_RunningState();
        public readonly Player_JumpingState playerJumpingState = new Player_JumpingState();
        public readonly Player_SprintState playerSprintState = new Player_SprintState();
        #endregion
        // Use this for initialization

        private void Awake()
        {
            PCRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            myAudio = gameObject.GetComponent<AudioSource>();
            GunAnimator = gameObject.GetComponent<Animator>();
        }
        void Start()
        {
            // AnimTriggers
            AnimTriggers.Add("Running");
            AnimTriggers.Add("Idle");
            AnimTriggers.Add("Aiming");
            AnimTriggers.Add("NotAiming");
            AnimTriggers.Add("Hipshoot");
            AnimTriggers.Add("Reload");


            weaponParent = GameObject.FindGameObjectWithTag("WeaponParent");
            playerCamera = Camera.main;
            currentAmmo = MagazineSize;
        }

        // Update is called once per frame
        void Update()
        {
            PlayerInput();
            weaponParent.transform.localPosition = currentWeaponPosition;
            GunAnimations();
            LayerMask newMask = ~(invertMask ? ~mask.value : mask.value);
        }

        // Player Input.
        public void PlayerInput()
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / rateOfFire;
                Magazine();
            }

            if(Input.GetMouseButton(1) && !playerIsSprinting)
            {
                currentWeaponPosition =  Vector3.Lerp(currentWeaponPosition, ADSWeaponPosition, ADS_Speed);
                Camera.main.fieldOfView = 20;
                currentSpreadFactor = 0f;
                PCRef.currentSensitivity = PCRef.ADSSensitivity;
                playerIsADS = true;
            }
            else
            {
               currentWeaponPosition = Vector3.Lerp(currentWeaponPosition, DefaultWeaponPosition, ADS_Speed);
               Camera.main.fieldOfView = 60;
               currentSpreadFactor = spreadFactor;
                PCRef.currentSensitivity = PCRef.mouseSensitivity;
                playerIsADS = false;
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
            MuzzleShotTransform.transform.eulerAngles = weaponSpread;
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

            myAudio.clip = bulletSound;
            myAudio.PlayDelayed(0.1f);
            GunAnimations();
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
                ProjectileBulletTrail();
            }
            if(currentAmmo == 0)
            {
                myAudio.clip = lastBulletSound;
                myAudio.Play();
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

        void ProjectileBulletTrail()
        {
            ParticleSystem newGOBulletTrail = Instantiate(bulletTrail, MuzzleShotTransform.transform.position, MuzzleShotTransform.transform.localRotation);
            Debug.Log(newGOBulletTrail.transform.rotation);
            newGOBulletTrail.GetComponent<Rigidbody>().AddForce(transform.forward * bulletTrailSpeed);
        }

        public void GunAnimations()
        {
            // States
            //("Running")
            //("Idle")
            //("HipShoot")
            //("Aiming")
            //("NotAiming")

            if (PCRef.currentState == PCRef.playerIdleState)
            {
                GunAnimator.ResetTrigger("Running");
                GunAnimator.ResetTrigger("HipShoot");
                GunAnimator.SetTrigger("Idle");


            }
            else if(PCRef.currentState == PCRef.playerRunningState)
            {
                GunAnimator.ResetTrigger("Idle");
                GunAnimator.ResetTrigger("HipShoot");
                GunAnimator.SetTrigger("Running");
            }

            if (Input.GetButtonDown("Fire1") && currentAmmo > 0)
            {
                ResetAllTriggers();
                GunAnimator.SetTrigger("HipShoot");

            }
            if (playerIsADS)
            {
                GunAnimator.ResetTrigger("NotAiming");
                GunAnimator.SetTrigger("Aiming");
            }
            else
            {
                GunAnimator.ResetTrigger("Aiming");
                GunAnimator.SetTrigger("NotAiming");
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GunAnimator.ResetTrigger("Idle");
                GunAnimator.ResetTrigger("Aiming");
                GunAnimator.SetTrigger("Reload");
            }
         }

        void ResetAllTriggers()
        {

            GunAnimator.ResetTrigger(AnimTriggers.Count);
        }


        // TODO Get working.
        void ResetTriggers(string triggers, string trigger1 = "1", string trigger2 = "2", string trigger3 = "3")
        {
            List<string> trig = new List<string>();
            foreach(string item in trig){
                if(item.Contains(triggers) 
                    || item.Contains(trigger1)
                    || item.Contains(trigger2)
                    || item.Contains(trigger3))
                {
                    Debug.Log("Duplicate!");
                    return;
                }
                else
                {
                    trig.Add(triggers);
                    Debug.Log(trig[0]);
                    trig.Add(trigger1);
                    trig.Add(trigger2);
                    trig.Add(trigger3);
                }
            }

            if (trig[0] != null)
            {
                GunAnimator.ResetTrigger(trig[0]);
            }
            if (trig[1] != null)
            {
                GunAnimator.ResetTrigger(trig[1]);
            }
            if (trig[2] != null)
            {
                GunAnimator.ResetTrigger(trig[2]);
            }
            if (trig[3] != null)
            {
                GunAnimator.ResetTrigger(trig[3]);
            }

            trig.Clear();
        }
    }
}
