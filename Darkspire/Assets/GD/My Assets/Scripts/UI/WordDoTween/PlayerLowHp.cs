using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Import the DoTween namespace

public class HealthEffect : MonoBehaviour
{
    [SerializeField] private Image edgeEffectImage;
    [SerializeField] private PlayerUI playerHealth;
    [SerializeField] private float fadeDuration = 1f; // Duration of the fade animation

    private float lowHp;
    private Tween fadeTween; // To store the DoTween animation

    void Start()
    {
        // Start with the image fully transparent
        edgeEffectImage.color = new Color(edgeEffectImage.color.r, edgeEffectImage.color.g, edgeEffectImage.color.b, 0);
        lowHp = playerHealth.getMaxHealth() * 0.2f; // Calculate low health threshold
    }

    void Update()
    {
        if (playerHealth.getCurrentHealth() <= lowHp)
        {
            if (fadeTween == null || !fadeTween.IsActive()) // Check if the animation is already running
            {
                // Create the pulsating effect up to 0.1 alpha
                fadeTween = edgeEffectImage.DOFade(0.2f, fadeDuration / 2) // Smoothly fade to 0.1
                    .SetLoops(-1, LoopType.Yoyo) // Loop back and forth
                    .SetEase(Ease.InOutSine) // Use smooth easing for transitions
                    .From(0.05f); // Start from the minimum alpha
            }
        }
        else
        {
            // Stop the pulsating effect and fade out to fully transparent
            fadeTween?.Kill(); // Stop any existing tween
            fadeTween = edgeEffectImage.DOFade(0f, fadeDuration) // Fade out smoothly
                .OnComplete(() => fadeTween = null); // Reset the tween reference after completion
        }
    }
}
