using System;
using System.Collections.Generic;
using Progress;
using UnityEngine;
using Zenject;

public class LevelView : MonoBehaviour, ILevelView
{
    public event Action<WordPiece> WordPieceSelected;
    public event Action<WordPiece> WordPieceReleased;
    public event Action<WordPiece> WordPieceDoubleClicked;
    public event Action ValidateLevelPressed;
    public event Action NextLevelPressed;
    public event Action GoToMenuPressed;

    [SerializeField] private Canvas gameFieldCanvas;
    [SerializeField] private WordPiecesView wordPiecesView;
    [SerializeField] private WordSlotsView wordSlotsView;
    [SerializeField] private LevelControlsView levelControlsView;
    [SerializeField] private LevelCompleteMessageView levelCompleteMessageView;
    [SerializeField] private GameObject simpleLoadingScreen; 

    private void Awake()
    {
        wordPiecesView.Initialize(gameFieldCanvas);
    }

    private void OnEnable()
    {
        wordPiecesView.WordPieceSelected += OnWordPieceSelected;
        wordPiecesView.WordPieceReleased += OnWordPieceReleased;
        wordPiecesView.WordPieceDoubleClicked += OnWordPieceDoubleClicked;

        levelCompleteMessageView.NextLevelPressed += OnNextLevelPressed;
        levelCompleteMessageView.GoToMenuPressed += GoTomenuPressed;

        levelControlsView.GoToMenuPressed += GoTomenuPressed;
    }

    

    private void OnDisable()
    {
        wordPiecesView.WordPieceSelected -= OnWordPieceSelected;
        wordPiecesView.WordPieceReleased -= OnWordPieceReleased;
        wordPiecesView.WordPieceDoubleClicked -= OnWordPieceDoubleClicked;

        levelCompleteMessageView.NextLevelPressed -= OnNextLevelPressed;
        levelCompleteMessageView.GoToMenuPressed -= GoTomenuPressed;

        levelControlsView.GoToMenuPressed -= GoTomenuPressed;
    }

    public void SetUIElements(List<WordPieceSlot> slots, List<WordPiece> wordPieces)
    {
        wordSlotsView.SetWordPieceSlots(slots);
        wordPiecesView.SetWordPieces(wordPieces);
        simpleLoadingScreen.gameObject.SetActive(false);
    }

    public void ReturnWordPieceToInitialPosition(WordPiece wordPiece)
    {
        wordPiecesView.ReturnWordPieceToInitialPosition(wordPiece);
    }

    public IEnumerable<WordPiece> GetAllWordPieces()
    {
        return wordPiecesView.GetAllWordPieces();
    }

    public void UpdateUIFromMappings(IReadOnlyDictionary<int, int> mappings)
    {
        wordSlotsView.UpdateSlotsFromMappings(mappings);
        foreach (var mapping in mappings)
        {
            if (wordSlotsView.TryGetSlot(mapping.Value, out var slot))
            {
                wordPiecesView.ChangeWordPieceParent(mapping.Key, slot.transform, true);
            }
        }
    }

    public WordPieceSlot FindClosestEmptySlot(WordPiece wordPiece, float maxDistance = 10f)
    {
        return wordSlotsView.FindClosestEmptySlot(wordPiece.rectTransform.position, maxDistance);
    }

    public void ResetAllWordPiecesToInitialPositions()
    {
        wordSlotsView.ResetAllSlots();
        wordPiecesView.ResetAllWordPieces();
    }

    private void OnWordPieceSelected(WordPiece wordPiece)
    {
        WordPieceSelected?.Invoke(wordPiece);
    }

    private void OnWordPieceReleased(WordPiece wordPiece)
    {
        WordPieceReleased?.Invoke(wordPiece);
    }

    private void OnWordPieceDoubleClicked(WordPiece wordPiece)
    {
        WordPieceDoubleClicked?.Invoke(wordPiece);
    }

    private void OnValidateLevelPressed()
    {
        ValidateLevelPressed?.Invoke();
    }

    private void OnNextLevelPressed()
    {
        NextLevelPressed?.Invoke();
    }

    public void ShowLevelCompletedMessage(IReadOnlyList<string> words)
    {
        levelCompleteMessageView.ShowMessage(words);
    }

    private void GoTomenuPressed()
    {
        GoToMenuPressed?.Invoke();
    }
}