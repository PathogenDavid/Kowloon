using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kowloon.Core
{
    public static class KowloonConfig
    {
        public static bool IsValidRange((int StartIndex, int EndIndex) apartmentDefinition, int ledCount)
            => IsValidRange(apartmentDefinition.StartIndex, apartmentDefinition.EndIndex, ledCount);

        public static bool IsValidRange(int startIndex, int endIndex, int ledCount)
            => endIndex >= startIndex && startIndex >= 0 && endIndex >= 0 && startIndex < ledCount && endIndex < ledCount;

        private static readonly (int StartIndex, int EndIndex)[] _ApartmentRanges =
        {
            // Top 1-row
            (196, 198),
            // 2-row
            (191, 194),
            (186, 189),
            // 3-row
            (172, 174),
            (177, 179),
            (182, 185),
            // 4-row
            (168, 171),
            (163, 166),
            (158, 162),
            (154, 157),
            // 5-row
            (129, 132),
            (134, 137),
            (140, 143),
            (146, 148),
            (150, 153),
            // 5-row
            (125, 128),
            (120, 123),
            (114, 117),
            (108, 112),
            (103, 106),
            // 5-row
            (80, 83),
            (85, 88),
            (90, 92),
            (95, 98),
            (99, 102),
            // 4-row
            (76, 79),
            (71, 74),
            (65, 69),
            (60, 64),
            // 4-row
            (41, 44),
            (45, 49),
            (51, 55),
            (57, 59),
            // 3-row
            (37, 40),
            (32, 36),
            (28, 31),
            // 3-row
            (15, 18),
            (19, 23),
            (24, 27),
            // 2-row
            (12, 14),
            (7, 10),
            // 2-row
            (5, 6), // LED 6 is not pointing in the box, but I think that's because it's bent back and aiming into the room.
            (2, 2),
            // Bottom 1-row
            (0, 1),
        };

        public static readonly ReadOnlyCollection<(int StartIndex, int EndIndex)> ApartmentRanges = Array.AsReadOnly(_ApartmentRanges);
    }
}
