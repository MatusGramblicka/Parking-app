using System;

namespace Entities.Enums;

[Flags]
public enum FailureHandlingStrategy
{
    LogFailure = 0,
    DeactivateSubscription = 1
}