namespace MoravianStar_Demo.Common.Jobs.Common
{
    public interface IJobFlow
    {
        void Process(params object[] args);
    }
}