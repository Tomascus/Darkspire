using DG.Tweening;
using UnityEngine;

public class YouDiedTextAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform youDiedText; // The RectTransform of the "You Died" text
    [SerializeField] private Vector2 finalPosition = new Vector2(0, 200); // Final anchored position
    [SerializeField] private float animationDuration = 1f; // Duration of the movement

    private Vector2 startingPosition;

    void Start()
    {
        

        DOTween.SetTweensCapacity(200, 50);
        startingPosition = new Vector2(0, 0);
        youDiedText.anchoredPosition = startingPosition;

        Debug.Log("Starting Position: " + startingPosition);
        Debug.Log("Final Position: " + finalPosition);

        youDiedText.DOAnchorPos(finalPosition, animationDuration)
            .SetEase(Ease.OutBack)
            .OnUpdate(() => Debug.Log("Current Position: " + youDiedText.anchoredPosition))
            .OnComplete(() => Debug.Log("Animation Complete!"));
        

    }
}
