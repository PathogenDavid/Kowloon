namespace Kowloon.Core
{
    public interface IRangedTest : IRenderer
    {
        /// <summary>The minimum valid value (inclusive)</summary>
        int MinimumValue { get; }
        /// <summary>The maximum valid value (exclusive)</summary>
        int MaximumValue { get; }
        int Value { get; set; }
    }
}
