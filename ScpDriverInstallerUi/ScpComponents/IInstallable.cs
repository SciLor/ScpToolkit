namespace ScpDriverInstaller.ScpComponents
{
    public interface IInstallable
    {
        void Install();
        bool IsRebootRequired
        {
            get;
        }
    }
}