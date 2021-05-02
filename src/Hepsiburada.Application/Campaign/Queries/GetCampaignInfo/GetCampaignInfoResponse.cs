namespace Hepsiburada.Application.Campaign.Queries.GetCampaignInfo
{
    public class GetCampaignInfoResponse
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int TargetSales { get; set; }
        public int TotalSales { get; set; }
        public decimal Turnover { get; set; }
        public decimal AverageItemPrice { get; set; }
    }
}
