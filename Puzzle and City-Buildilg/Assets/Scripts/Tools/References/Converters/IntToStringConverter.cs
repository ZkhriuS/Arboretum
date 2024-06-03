namespace Tools.References.Converters
{
    public class IntToStringConverter : ValueToValueConverter<int, string>
    {
        public override void Convert(int value)
        {
            var result = value.ToString();

            OnValueConverted(result);
        }
    }
}