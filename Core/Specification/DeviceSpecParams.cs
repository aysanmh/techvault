
namespace Core.Specification
{
    public class DeviceSpecParams :PagingParams
    {

        private List<string> _brands = [];

        public List<string> Brands
        {
            get => _brands;

            set
            {
                _brands = value.SelectMany(x => x.Split(',' ,
                StringSplitOptions.RemoveEmptyEntries)).ToList();
            }
            
        }

        private List<string> _deviceGroups =[];

        public List<string> DeviceGroups
        {
            get => _deviceGroups;

            set
            {
                _deviceGroups = value.SelectMany(x => x.Split (',' ,
                StringSplitOptions.RemoveEmptyEntries)).ToList();
            }
        }


        public string? Sort { get; set; }


        private string? _search;
        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }

        
    }
}