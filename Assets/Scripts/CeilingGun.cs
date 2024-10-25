using System.Collections;
using UnityEngine;

public class CeilingGun : MonoBehaviour
{
    public GameObject bulletPrefab;     // Bullet prefab
    public Transform firePoint;         // The point from where bullets will be fired
    public float fireInterval = 4;     // Time interval between shots
    public float burstInterval = 0.2f;  // Time interval between bullets within a burst
    public int bulletsPerBurst = 2;     // Number of bullets per burst

    // Animator references for the Gun and Ceiling Light
    public Animator gunAnimator;        // Reference to the child Gun's Animator
    public Animator ceilingLightAnimator; // Reference to the Ceiling Light's Animator


    private void Start()
    {
        // Start the firing coroutine
        StartCoroutine(FireRoutine());
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            // Trigger the gun firing animation
            gunAnimator.SetTrigger("Fire");

            // Trigger the ceiling light animation
            ceilingLightAnimator.SetTrigger("LightFlash");

            // Fire a burst of bullets
            for (int i = 0; i < bulletsPerBurst; i++)
            {
                Fire();
                yield return new WaitForSeconds(burstInterval);
            }

            // Wait before the next burst
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void Fire()
    {

        // Create a bullet instance
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            // Set direction according to the firePoint's rotation
            bulletScript.SetDirection(firePoint.right); // Adjust if necessary
        }

        //Play sound clip attached to bullet prefab
        bullet.GetComponent<AudioSource>().Play();
    }
}
