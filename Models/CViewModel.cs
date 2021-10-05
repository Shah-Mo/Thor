using System.ComponentModel.DataAnnotations;

namespace Thor.Models
{
    public enum CType : uint
    {
        Type0 = 0,
        Type1,
        Type2
    }

    public enum CFormat : uint
    {
        Format0,
        Format1,
        Format2,
        Format3,
        Format4
    }

    public class CViewModel
    {
        const uint MaximumSize = 160000;
        const uint MaximumNumberOfItems = 10000;

        [Required]
        [Range(0, 2, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public CType Type { get; set; }

        [Required]
        [Range(0, 4, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Format { get; set; }

        [Required]
        [Range(0, MaximumSize, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public uint Size { get; set; }

        [Range(1, MaximumNumberOfItems, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public uint NumberOfItems { get; set; } = 1;  // default value is 1
    }
}
