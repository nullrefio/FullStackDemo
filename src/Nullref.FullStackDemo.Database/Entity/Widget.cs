using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nullref.FullStackDemo.Database.Entity
{
    public enum FruitConstants
    {
        Apple = 1,
        Pear = 2,
        Orange = 3,
    }

    public class Widget : IDatabaseItem<Guid>
    {
        public readonly struct MaxLengthValues
        {
            public const int Code = 100;
            public const int Description = 500;
            public const int State = 50;
        }

        public readonly struct DefaultValues
        {
            public const FruitConstants MyFruit = FruitConstants.Apple;
            public const bool IsActive = true;
            public const string State = "Georgia";
        }

        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PrimaryKey => Id;

        [Required]
        [MaxLength(MaxLengthValues.Code)]
        public string Code { get; set; }

        [Required]
        [DefaultValue(DefaultValues.IsActive)]
        public bool IsActive { get; set; } = DefaultValues.IsActive;

        [Required]
        [MaxLength(MaxLengthValues.Description)]
        public string Description { get; set; }

        [Required]
        [MaxLength(MaxLengthValues.State)]
        [DefaultValue(DefaultValues.State)]
        public string State { get; set; } = DefaultValues.State;

        [Required]
        [DefaultValue(DefaultValues.MyFruit)]
        public FruitConstants MyFruit { get; set; } = DefaultValues.MyFruit;
    }
}
