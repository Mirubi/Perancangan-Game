using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int maxHealth = 100;
    protected int currentHealth;

    public Animator animator;
    private CharacterHealthUI healthUI;

    public virtual void Start()
    {
        currentHealth = maxHealth;
        healthUI = GetComponentInChildren<CharacterHealthUI>();
        if (healthUI != null)
        {
            healthUI.UpdateHealth();
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (healthUI != null)
        {
            healthUI.UpdateHealth();
        }
        if (this.CompareTag("Enemy")) // Pastikan hanya enemy yang memicu hitstop
    {
        if (HitStopManager.Instance != null)
            HitStopManager.Instance.DoHitStop(0.0f); // hitstop 0.05 detik
    }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public virtual void Die()
    {
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}