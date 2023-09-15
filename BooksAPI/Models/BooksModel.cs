using System.Drawing.Printing;

namespace BooksAPI.Models
{
    public class BooksModel
    {

        public int id { get; set; }

        public string title { get; set; }
    

        public string publisher_name { get; set; }
        public string publishedDate { get; set; }

        public string description { get; set; }
        public int pub_id { get; set; }

        public string author_name { get; set; }

        public int  auth_id{ get; set;}


        public string language { get; set; }

        public string maturityRating { get; set; }

        public int pageCount { get; set; }

        public string categories { get; set; }

        public decimal retailPrice { get; set; }
    }


    public class GoogleBooksApiResponse
    {
        public string kind { get; set; }
        public int totalItems { get; set; }
        public List<GoogleBooksApiItem> items { get; set; }

        public int id { get; set; }

        public string title { get; set; }

        public string publisher_name { get; set; }
        public string publishedDate { get; set;}

        public int publisher_id { get; set; }

        public string author_name { get; set;}

        public int auth_id { get; set;}

        public int pub_id { get; set;}
        public decimal retailPrice { get; set; }

        public string description { get; set; }

        public int pageCount { get; set;}

        public string categories { get; set; }

        public string maturityRating { get; set;}

        public int author_id {  get; set; }




    }

    public class GoogleBooksApiItem
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string etag { get; set; }
        public string selfLink { get; set; }
        public GoogleBooksVolumeInfo volumeInfo { get; set; }
    }

    public class GoogleBooksVolumeInfo
    {
        public List<string> authors { get; set; }
        public string title { get; set; }
        public string publisher { get; set; }
        public string publishedDate { get; set; }

        public decimal retailPrice { get; set; }
        public string description { get; set; }

        public string language {  get; set; }

        public string maturityRating { get; set; }

        public int  pageCount { get; set; }

        //public string categories { get; set; }


    }

}
