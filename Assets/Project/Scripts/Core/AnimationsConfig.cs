using UnityEngine;

[CreateAssetMenu(fileName = "AnimationsConfig", menuName = "Game/AnimationsConfig")]
public class AnimationsConfig : ScriptableObject
{
    [Header("WordPieces Settings")]
    public float delayBetween = 0.1f;
    public float wordPieceAppearDuration = 0.5f;
    
    [Header("WordPiecesSlotContainer Settings")]
    public float slotContainerAppearDuration = 0.5f;
} 
