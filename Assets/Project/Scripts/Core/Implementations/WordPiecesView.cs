using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WordPiecesView : IWordPiecesView, ITickable, IDisposable, IInitializable
{
    public event Action<WordPiece> WordPieceSelected;
    public event Action<WordPiece> WordPieceMoving;
    public event Action<WordPiece> WordPieceReleased;
    public event Action<WordPiece> WordPieceDoubleClicked;

    private readonly Transform draggingParent;
    private readonly Canvas gameFieldCanvas;

    private readonly Dictionary<int, WordPiece> wordPieces =
        new();
    private WordPiece selectedWordPiece;
    private RectTransform gameFieldCanvasRectTransform;

    private Dictionary<int, WordPieceInitialState> wordPiecesInitialStates
        = new();
    
    [Inject]
    public WordPiecesView([Inject(Id = "DraggingParent")]Transform draggingParent,
        [Inject(Id = "GameFieldCanvas")]Canvas gameFieldCanvas)
    {
        this.draggingParent = draggingParent;
        this.gameFieldCanvas = gameFieldCanvas;
    }

    public void Initialize()
    {
        gameFieldCanvasRectTransform
            = gameFieldCanvas.GetComponent<RectTransform>();
    }


    public void SetWordPieces(List<WordPiece> pieces)
    {
        foreach (var wordPiece in wordPieces.Values)
        {
            UnsubscribeFromWordPieceEvents(wordPiece);
        }

        wordPieces.Clear();
        wordPiecesInitialStates.Clear();

        foreach (var wordPiece in pieces)
        {
            wordPieces[wordPiece.Index] = wordPiece;
            SubscribeToWordPieceEvents(wordPiece);

            wordPiecesInitialStates[wordPiece.Index] = new WordPieceInitialState
            {
                OriginalParent = wordPiece.transform.parent,
                OriginalSiblingIndex = wordPiece.transform.GetSiblingIndex()
            };
        }
    }

    public void ReturnWordPieceToInitialPosition(WordPiece wordPiece)
    {
        if (wordPiecesInitialStates.TryGetValue(wordPiece.Index, out var state))
        {
            wordPiece.transform.SetParent(state.OriginalParent);
            wordPiece.transform.SetSiblingIndex(state.OriginalSiblingIndex);

            var rectTransform = wordPiece.rectTransform;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.anchoredPosition = Vector2.zero;

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(state.OriginalParent.GetComponent<RectTransform>());
        }
    }

    public void ResetAllWordPieces()
    {
        foreach (var wordPiece in wordPieces.Values)
        {
            ReturnWordPieceToInitialPosition(wordPiece);
        }
    }

    public IEnumerable<WordPiece> GetAllWordPieces() => wordPieces.Values;

    public bool TryGetWordPiece(int index, out WordPiece wordPiece)
    {
        return wordPieces.TryGetValue(index, out wordPiece);
    }

    public void ChangeWordPieceParent(int wordPieceIndex, Transform parent, bool nullifyPosition = false)
    {
        if (wordPieces.TryGetValue(wordPieceIndex, out var wordPiece))
        {
            wordPiece.ChangeParent(parent, nullifyPosition);
        }
    }

    private void SubscribeToWordPieceEvents(WordPiece wordPiece)
    {
        wordPiece.Selected += HandleWordPieceSelected;
        wordPiece.Released += HandleWordPieceReleased;
        wordPiece.DoubleClicked += HandleWordPieceDoubleClicked;
    }

    private void UnsubscribeFromWordPieceEvents(WordPiece wordPiece)
    {
        wordPiece.Selected -= HandleWordPieceSelected;
        wordPiece.Released -= HandleWordPieceReleased;
        wordPiece.DoubleClicked -= HandleWordPieceDoubleClicked;
    }

    private void HandleWordPieceSelected(WordPiece wordPiece)
    {
        selectedWordPiece = wordPiece;
        ChangeWordPieceParent(wordPiece.Index, draggingParent);
        WordPieceSelected?.Invoke(wordPiece);
    }

    private void HandleWordPieceReleased(WordPiece wordPiece)
    {
        WordPieceReleased?.Invoke(wordPiece);
        selectedWordPiece = null;
    }

    private void HandleWordPieceDoubleClicked(WordPiece wordPiece)
    {
        WordPieceDoubleClicked?.Invoke(wordPiece);
    }

    public void Tick()
    {
        if (selectedWordPiece == null) return;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            gameFieldCanvasRectTransform,
            Input.mousePosition,
            gameFieldCanvas.worldCamera,
            out Vector3 worldPos);

        selectedWordPiece.rectTransform.position = new Vector3(worldPos.x, worldPos.y, selectedWordPiece.rectTransform.position.z);
        WordPieceMoving?.Invoke(selectedWordPiece);
    }

    public void Dispose()
    {
        foreach (var wordPiece in wordPieces.Values)
        {
            UnsubscribeFromWordPieceEvents(wordPiece);
        }
    }
}