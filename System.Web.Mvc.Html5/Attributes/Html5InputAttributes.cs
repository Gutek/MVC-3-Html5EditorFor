namespace System.Web.Mvc
{
    public class Html5RangeAttribute : HtmlAttribute
    {
        public int? Min { get; set; }
        public int? Max { get; set; }
        public Html5RangeAttribute() : base("type", "range") {}

        public Html5RangeAttribute(int min, int max) : base("type", "range")
        {
            Min = min;
            Max = max;
        }

        public override void OnMetadataCreated(ModelMetadata metadata)
        {
 	        base.OnMetadataCreated(metadata);

            if(Min.HasValue)
            {
                metadata.AdditionalValues[string.Format(Prefix, "min")] = Min;
            }
            
            if(Max.HasValue)
            {
                metadata.AdditionalValues[string.Format(Prefix, "max")] = Max;
            }
        }
    }

    public class Html5StepAttribute : HtmlAttribute
    {
        public Html5StepAttribute(int value) : base("step", value) {}
        public Html5StepAttribute(double value) : base("step", value) {}
    }

    public class Html5ColorAttribute : HtmlAttribute
    {
        public Html5ColorAttribute() : base("type", "color") { }
    }

    public class Html5TimeAttribute : HtmlAttribute
    {
        public Html5TimeAttribute() : base("type", "time") { }
    }

    public class Html5WeekAttribute : HtmlAttribute
    {
        public Html5WeekAttribute() : base("type", "week") { }
    }

    public class Html5MonthAttribute : HtmlAttribute
    {
        public Html5MonthAttribute() : base("type", "month") { }
    }

    public class HtmlDateAttribute : HtmlAttribute
    {
        public HtmlDateAttribute() : base("type", "date") { }
    }

    public class Html5DateTimeAttribute : HtmlAttribute
    {
        public Html5DateTimeAttribute() : base("type", "datetime") { }
    }

    public class Html5DateTimeLocalAttribute : HtmlAttribute
    {
        public Html5DateTimeLocalAttribute() : base("type", "datetime-local") { }
    }

    public class Html5SearchAttribute : HtmlAttribute
    {
        public Html5SearchAttribute() : base("type", "search") { }
    }
    public class Html5EmailAttribute : HtmlAttribute
    {
        public Html5EmailAttribute() : base("type", "search") { }
    }
    public class Html5PasswordAttribute : HtmlAttribute
    {
        public Html5PasswordAttribute() : base("type", "password") { }
    }
    public class Html5UrlAttribute : HtmlAttribute
    {
        public Html5UrlAttribute() : base("type", "url") { }
    }

    public class Html5MaxAttribute : HtmlAttribute
    {
        public Html5MaxAttribute(int value) : base("max", value) {}
    }

    public class Html5MinAttribute : HtmlAttribute
    {
        public Html5MinAttribute(int value) : base("max", value) {}
    }

    public class Html5UseDefaultMinMaxAttribute : HtmlAttribute
    {
        public Html5UseDefaultMinMaxAttribute() : base("use-min-max", true) {}
    }
}