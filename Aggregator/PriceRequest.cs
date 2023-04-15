using EnsureThat;

namespace Aggregator;

public sealed record PriceRequest
{
    public PriceRequest(RiskData riskData)
    {
        EnsureArg.IsNotNull(riskData, nameof(riskData));

        RiskData = riskData;
    }

    public RiskData RiskData { get; }
}