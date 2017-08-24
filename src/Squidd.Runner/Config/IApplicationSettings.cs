using System;

namespace Squidd.Runner.Config
{
    public interface IApplicationSettings
    {
        string GetTemporaryDirectoryPath();
        string GetPairId();
        string GetPairPassphrase();
        void SetPairId(Guid pairId);
    }
}