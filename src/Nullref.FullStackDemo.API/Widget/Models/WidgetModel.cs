using Nullref.FullStackDemo.CommonModels.Attributes;
using Nullref.FullStackDemo.Database.Entity;
using static Nullref.FullStackDemo.Database.Entity.Widget;

namespace Nullref.FullStackDemo.API.Widget.Models
{
    [ReadOnly(true)]
    public class WidgetModel : WidgetUpdateModel
    {
        [Required]
        public Guid Id { get; internal set; }

        [Required]
        [LocalDateTimeRange]
        public NodaTime.LocalDateTime CreatedDate { get; internal set; } = DateTime.UtcNow.ToLocalDateTime();
    }

    public class WidgetUpdateModel : AbstractValidatableModel, IModel
    {
        [AutoTrim]
        [Searchable]
        [Sortable(IsDefault = true)]
        [Required]
        [MaxLength(MaxLengthValues.Code)]
        [Description("The widget code")]
        [DefaultValue("My defined default value")]
        public string Code { get; set; }

        [Sortable]
        [Required]
        [DefaultValue(DefaultValues.IsActive)]
        public bool IsActive { get; set; } = DefaultValues.IsActive;

        [AutoTrim]
        [Searchable]
        [Required]
        [MaxLength(MaxLengthValues.Description)]
        [Description("A tooltip for description")]
        [DefaultValue("what....??")]
        [DisplayName("My custom header")]
        public string Description { get; set; }

        [AutoTrim]
        [Searchable]
        [Sortable]
        [Required]
        [MaxLength(MaxLengthValues.State)]
        [DisplayName("State of residence")]
        [Description("What state is this thing from?")]
        [DefaultValue(DefaultValues.State)]
        public string State { get; set; } = DefaultValues.State;

        [Required]
        [DefaultValue(DefaultValues.MyFruit)]
        public FruitConstants MyFruit { get; set; } = DefaultValues.MyFruit;
    }
}
