namespace itec_mobile_api_final.Base
{
    public class IsReadOnlyAttribute : BasicAttribute
    
    {
        public bool Is => (bool) base.Value;

        public IsReadOnlyAttribute(bool value = true) : base("readOnly", value)
        {
        }
    }
}