using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

public class SwipeCardHandler : MonoBehaviour
{
    [Header("Settings")]
    [field: SerializeField] public int matchTentative = 5;

    [Header("References")]
    [SerializeField] private SwipeBoundary boundary;
    [SerializeField] private SwipeCard swipeCardPrefab;
    [SerializeField] private RectTransform swipeCardContainer;
    public ObjectPool<SwipeCard> CardPool { get; private set; }

    [Header("Events")]
    public UnityEvent onMatchSessionEnd;


    private void Awake()
    {
        CardPool = new ObjectPool<SwipeCard>(
            createFunc: () =>
            {
                var card = Instantiate(swipeCardPrefab, transform);
                card.boundary = boundary;
                return card;
            },
            actionOnGet: (tile) => tile.gameObject.SetActive(true),
            actionOnRelease: (tile) => tile.gameObject.SetActive(false),
            actionOnDestroy: (tile) => Destroy(tile.gameObject),
            collectionCheck: true,
            defaultCapacity: 3,
            maxSize: 5
        );

        boundary.OnSwipeRight.AddListener(TryMatch);
        boundary.OnSwipeLeft.AddListener(EndSwipe);
    }

    public void StartMatchSession()
    {
        matchTentative = 5;
        LoadNextCard();
    }

    [Button] private void TryMatch(SwipeCard card)
    {
        // Simulate match logic
        matchTentative--;
        EndSwipe(card);
    }
    [Button] private void EndSwipe(SwipeCard card)
    {
        CardPool.Release(card);

        if(matchTentative <= 0)
        {
            Debug.Log("No more match attempts left.");
            onMatchSessionEnd?.Invoke();
        }
        else
        {
            LoadNextCard();
        }
    }

    private void LoadNextCard()
    {
        LoadCard();
    }
    private void LoadCard()
    {
        SwipeCard newCard = CardPool.Get();
        newCard.transform.SetParent(swipeCardContainer, false);
        newCard.RectTransform.anchoredPosition = Vector2.zero;
    }
}
