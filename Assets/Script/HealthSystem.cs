using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    public event EventHandler OnDamaged;
    // public event EventHandler OnDied;

    [SerializeField] private int healthAmountMax;
    private int healthAmount;

    private void Awake() {
        healthAmount = healthAmountMax;
    }

    public void Damage(int damageAmount) {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (IsDead()) {
            // OnDied?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }

    public bool IsDead() {
        return healthAmount == 0;
    }

    public bool IsFullHeath() {
        return healthAmount == healthAmountMax;
    }

    public int GetHealthAmount() {
        return healthAmount;
    }

    public float GetHealthAmountNormalized() {
        return (float) healthAmount / healthAmountMax;
    }

    public void SetHealthAmountMax(int healthAmountMax, bool updateHealthAmount) {
        this.healthAmountMax = healthAmountMax;

        if (updateHealthAmount) {
            healthAmount = healthAmountMax;
        }
    }
}
