using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StepsController : ControllerBase
{
    private const int MinimumSteps = 10_000;
    private const int MaximumSteps = 18_000;

    [HttpGet]
    public ActionResult<StepCounterResult> GetSteps()
    {
        var profile = CreateProfile();

        return Ok(new StepCounterResult(
            DateOnly.FromDateTime(DateTime.Today),
            profile.Steps,
            MaximumSteps,
            profile.DistanceKm,
            profile.Calories));
    }

    private static StepProfile CreateProfile()
    {
        var steps = Random.Shared.Next(MinimumSteps, MaximumSteps + 1);
        var strideMeters = Random.Shared.NextDouble() * 0.2 + 0.65;
        var distanceKm = Math.Round(steps * strideMeters / 1_000, 2);

        // Approximate walking calories for different body weights and walking speeds.
        var caloriesPerKgPerKm = Random.Shared.NextDouble() * 0.2 + 0.45;
        var weightKg = Random.Shared.Next(55, 96);
        var calories = Math.Round(distanceKm * weightKg * caloriesPerKgPerKm);

        return new StepProfile(steps, distanceKm, calories);
    }
}

public sealed record StepCounterResult(
    DateOnly Date,
    int Steps,
    int Goal,
    double DistanceKm,
    double Calories);

internal sealed record StepProfile(int Steps, double DistanceKm, double Calories);
