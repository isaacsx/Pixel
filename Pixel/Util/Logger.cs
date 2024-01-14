using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Discord;
using System;

namespace Pixel.Util
{
    public abstract class Logger
    {
        public static Task OnLogAsync(ILogger logger, LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Verbose:
                case LogSeverity.Info:
                    logger.LogInformation(msg.ToString());
                    break;
                case LogSeverity.Warning:
                    logger.LogWarning(msg.ToString());
                    break;
                case LogSeverity.Error:
                    logger.LogError(msg.ToString());
                    break;
                case LogSeverity.Critical:
                    logger.LogCritical(msg.ToString());
                    break;
                case LogSeverity.Debug:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Task.CompletedTask;
        }
    }
}