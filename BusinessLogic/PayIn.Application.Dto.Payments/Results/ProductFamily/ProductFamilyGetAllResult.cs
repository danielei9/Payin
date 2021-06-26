namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ProductFamilyGetAllResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public string FamilyConcessionName { get; set; }

    }
}