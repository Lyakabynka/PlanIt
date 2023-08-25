using System.Text.Json.Serialization;

namespace PlanIt.Plan.Domain.Enums;

public enum PlanType
{
    Notification,
    OpenBrowser,
    OpenDesktop,
    VoiceCommand,
    Volume,
    WeatherCommand,
    FocusOn
}