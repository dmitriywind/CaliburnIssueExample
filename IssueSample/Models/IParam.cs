namespace IssueSample.Models
{
    public interface IParam<in TParam>
    {
        void BindData(TParam item);
    }
}
