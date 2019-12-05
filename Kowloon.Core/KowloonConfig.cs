using System;
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
            (5, 5),
            (2, 2),
            // Bottom 1-row
            (0, 1),
        };

        private static readonly (double Offset, int Width)[] _RowDescriptions =
        {
            (0, 1),
            (-1.25, 2),
            (-1.75, 3),
            (0.5, 4),
            (-1.8, 5),
            (-1.5, 5),
            (1.5, 5),
            (1.2, 4),
            (0.8, 4),
            (-0.7, 3),
            (0.6, 3),
            (1.4, 2),
            (0.5, 2),
            (1.5, 1),
        };

        public static readonly ReadOnlyCollection<(int StartIndex, int EndIndex)> ApartmentRanges = Array.AsReadOnly(_ApartmentRanges);
        public static readonly ReadOnlyCollection<(double Offset, int Width)> RowDescriptions = Array.AsReadOnly(_RowDescriptions);

        public static readonly double MinimumLeftOffset;
        public static readonly double MaximumRightOffset;

        static KowloonConfig()
        {
            MinimumLeftOffset = 0.0;
            MaximumRightOffset = 0.0;

            double offset = 0.0;
            int totalApartmentCount = 0;
            foreach ((double rowOffset, int width) in RowDescriptions)
            {
                offset += rowOffset;
                totalApartmentCount += width;

                if (offset < MinimumLeftOffset)
                { MinimumLeftOffset = offset; }

                double rightOffset = offset + width;
                if (rightOffset > MaximumRightOffset)
                { MaximumRightOffset = rightOffset; }
            }

            if (ApartmentRanges.Count != totalApartmentCount)
            { Console.Error.WriteLine($"Kowloon configuraiton error: Row descriptions have {totalApartmentCount} apartments, but {ApartmentRanges.Count} are defined."); }
        }
    }
}
