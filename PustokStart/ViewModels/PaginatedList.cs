namespace PustokStart.ViewModels
{
    public class PaginatedList<T>
    {
        public PaginatedList(List<T> items,int pageIndex,int totalPage)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPage = totalPage;
            
        }
        public int PageIndex { get; }
        public int TotalPage { get; }
        public List<T> Items { get; }
        public bool HasNext => PageIndex < TotalPage;
        public bool HasPrevious => PageIndex > 1;

        public static PaginatedList<T> Create (IQueryable<T> query,int page,int size)
        {
            int total = (int)Math.Ceiling(query.Count()/(double)size);
            return new PaginatedList<T>(query.Skip((page - 1) * size).Take(size).ToList(), page, total);
        }

    }
}
