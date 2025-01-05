namespace RandevuPlus.API.Shared.Dtos
{
    public class PaginatedResponse<T>
    {
        public int PageNumber { get; set; }  // Mevcut sayfa numarası
        public int PageSize { get; set; }    // Sayfa başına öğe sayısı
        public int TotalCount { get; set; }  // Toplam öğe sayısı
        public int TotalPages { get; set; }  // Toplam sayfa sayısı
        public List<T> Items { get; set; }  // Sayfadaki öğeler
    }
}
