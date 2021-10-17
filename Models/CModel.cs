using System.ComponentModel.DataAnnotations;
using Thor.Helpers;

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

    public class CModel
    {
        const uint MaximumSize = 160000;
        const uint MaximumNumberOfItems = 10000;

        [Required]
        [Range(0, 2, ErrorMessage = Constants.Messages.CModelError)]
        public CType Type { get; set; }

        [Required]
        [Range(0, 4, ErrorMessage = Constants.Messages.CModelError)]
        public int Format { get; set; }

        [Required]
        [Range(0, MaximumSize, ErrorMessage = Constants.Messages.CModelError)]
        public uint Size { get; set; }

        [Range(1, MaximumNumberOfItems, ErrorMessage = Constants.Messages.CModelError)]
        public uint NumberOfItems { get; set; } = 1;  // default value is 1
    }
}
