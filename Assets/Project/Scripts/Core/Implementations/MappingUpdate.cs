using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MappingUpdate
{
    public enum UpdateType { Add, Remove, Clear, InitializeAll }

    public UpdateType Type { get; }
    public int? WordPieceId { get; }
    public int? SlotId { get; }

    // Конструкторы для разных типов обновлений
    public static MappingUpdate AddOrUpdate(int wordPieceId, int slotId) =>
        new MappingUpdate(UpdateType.Add, wordPieceId, slotId);

    public static MappingUpdate Remove(int wordPieceId) =>
        new MappingUpdate(UpdateType.Remove, wordPieceId, null);

    public static MappingUpdate Clear() =>
        new MappingUpdate(UpdateType.Clear, null, null);
    public static MappingUpdate InitializeAll() =>
new MappingUpdate(UpdateType.InitializeAll, null, null);

    private MappingUpdate(UpdateType type, int? wordPieceId, int? slotId)
    {
        Type = type;
        WordPieceId = wordPieceId;
        SlotId = slotId;
    }
}