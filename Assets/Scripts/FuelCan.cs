using UnityEngine;

public class FuelCan : MonoBehaviour
{
    private Animator animator;
    private new Collider2D collider2D;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    public AudioClip fuelCanSound;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        // get audio source component
        audioSource = GetComponent<AudioSource>();
    }

    public void Collect()
    {
        // Actions when the fuel can is collected
        spriteRenderer.enabled = false;
        collider2D.enabled = false;
    }

    public void ResetFuelCan()
    {
        // Reset the fuel can's visibility and collider
        spriteRenderer.enabled = true;
        collider2D.enabled = true;

        // Reset the animation to the initial state
        animator.Play("fuelCanSpin", -1, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("CollectFuel");

            audioSource.PlayOneShot(fuelCanSound);
        }
    }
}
