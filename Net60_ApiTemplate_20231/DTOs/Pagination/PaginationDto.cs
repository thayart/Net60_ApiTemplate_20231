namespace Net60_ApiTemplate_20231.DTOs
{
    public class PaginationDto
    {
        public bool? isActive { get; set; }
        public int Page { get; set; } = 1;
        
        private int recordsPerPage = 10;

        private readonly int maxRecordsPerPage = 50;

        public int RecordsPerPage
        {
            get
            {
                return recordsPerPage;
            }
            set
            {
                recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
            }
        }
    }
}