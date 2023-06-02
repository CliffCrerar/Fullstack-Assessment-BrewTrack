using System;

namespace BrewTrack.Helpers
{
    public interface IDataPages<T> where T : class
    {
        int Count { get; }
        IList<DataPage<T>> Pages { get; }
        IDictionary<int, string> PageKeysLookup { get; }
        IDictionary<string, int> KeysPageLookup { get; }
    }
    public interface IDataPage<T>
    {
        string PageKey { get;  }
        int Page { get; }   
        int OfPages { get; }
        ArraySegment<T> Data { get; }
        IDictionary<int, string>? PageKeysLookup { get; set; }
        IDictionary<string, int>? KeysPageLookup { get; set; }
    }
    public class DataPage<T> : IDataPage<T> where T : class
    {
        public string PageKey { get; }
        public int Page { get; }
        public int OfPages { get; }
        public ArraySegment<T> Data { get; }
        public IDictionary<int, string>? PageKeysLookup { get; set; }
        public IDictionary<string, int>? KeysPageLookup { get; set; }
        public DataPage(ArraySegment<T> data, int page, int ofPages, string? pageKeyPrefix)
        {
            PageKey = string.IsNullOrEmpty(pageKeyPrefix) ? Guid.NewGuid().ToString() : pageKeyPrefix + "-" + page.ToString();
            Data = data;
            OfPages = ofPages;
            Page = page;
        }
    }

    public class DataPages<T>: IDataPages<T> where T : class
    {
        public int Count { get; set; }
        public IList<DataPage<T>> Pages { get; set; }
        public IDictionary<int, string> PageKeysLookup { get; set; }
        public IDictionary<string, int> KeysPageLookup { get; set; }
    }

    public class DataBook<T> where T : class
    {
        private readonly int _totalPages;
        private readonly int _totalRecords;
        private readonly int _recordsPerPage;
        private readonly int _lastPageRecords;
        private readonly string _pageKeyPrefix;
        private readonly IList<T> _globalSet;
        private IList<DataPage<T>> _pages { get; }
        private IDictionary<int, string> _pageKeys;
        private IDictionary<string, int> _keysPage;
        public DataBook(IList<T> data, int recordsPerPage, string pageKeyPrefix = "Databook" )
        {
            _totalRecords = data.Count;
            _recordsPerPage = recordsPerPage;
            _totalPages = int.Parse(Math.Floor( decimal.Parse( _totalRecords.ToString() ) / decimal.Parse(_recordsPerPage.ToString())).ToString()) + 1;
            _lastPageRecords = _totalRecords % _recordsPerPage == 0 ? _recordsPerPage : _totalRecords % _recordsPerPage;
            _globalSet = data;
            _pages = new List<DataPage<T>>();
            _pageKeyPrefix = pageKeyPrefix;
            _populateObject();
        }

        private void _populateObject()
        {
            var offset = 0; 
            var data = _globalSet.ToArray();
            for(int i  = 0; i < _totalPages; i++)
            {
                bool isLastPage = i == data.Length -1;
                int recordsPerpage = isLastPage ? _lastPageRecords : _recordsPerPage;
                ArraySegment<T> dataSlice = new ArraySegment<T>(data, offset, _recordsPerPage);
                DataPage<T> dataPage = new DataPage<T>(dataSlice, i, data.Length, _pageKeyPrefix);
                _pageKeys.Add(i, dataPage.PageKey);
                _keysPage.Add(dataPage.PageKey, i);
                _pages.Add(dataPage);
            }
        }
        public static DataBook<T> Create(IList<T> data, int recordsPerPage)
        {
            return new DataBook<T>(data, recordsPerPage);
        }
        public static DataBook<T> Create(IList<T> data, int recordsPerPage, string pageKeyPrefix)
        {
            return new DataBook<T>(data, recordsPerPage, pageKeyPrefix);
        }

        public DataPages<T> GetDataPages()
        {
            return Ensure.ArgumentNotNull( new DataPages<T>
            {
                Count = _totalPages,
                PageKeysLookup = _pageKeys,
                KeysPageLookup = _keysPage,
                Pages = _pages
            });
        }
    }
}
