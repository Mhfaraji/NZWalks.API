namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string  Name { get; set; }
        public string  Description { get; set; }  // توضیحات
        public double LengthInKm { get; set; }
        public string? WalkImageURL   { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }


        // navigation properties

        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }


    }




    // ئتهداعربقیثسصثیقبفعلغهاخعتاخغلفبق7ثی
    // تسهاتعاعلغعبفغ
}
