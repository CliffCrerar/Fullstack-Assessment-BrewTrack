using System;

namespace BrewTrack.Helpers
{
    internal interface IDataPage<T>
    {
        string PageKey { get;  }
        int Page { get; }   
        int OfPages { get; }
        ArraySegment<T> Data { get; }
    }
    internal class DataPage<T> : IDataPage<T> where T : class
    {
        public string PageKey { get; }
        public int Page { get; }
        public int OfPages { get; }
        public ArraySegment<T> Data { get; }
        public DataPage(ArraySegment<T> data, int page, int ofPages, string? pageKeyPrefix)
        {
            PageKey = string.IsNullOrEmpty(pageKeyPrefix) ? Guid.NewGuid().ToString() : pageKeyPrefix + "-" + page.ToString();
            Data = data;
            OfPages = ofPages;
            Page = page;
        }
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
    }
}
