using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBilet.CaseStudy.ApplicationCore.Queries
{
    public class GetLocationQuery : IRequest<LocationResult>
    {
        public string Search { get; set; }

        public GetLocationQuery(string search)
        {
            Search = search;
        }
    }

    public class LocationResult
    {
        public LocationResult(IList<Item> items)
        {
            Items = items;
        }

        public IList<Item> Items { get; set; }
        
        public class Item
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }
}