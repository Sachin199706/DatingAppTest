namespace DatingApp.Helpers
{
    public class PaginationHeader(int currentpage,int itemperpage,int totalitem,int totalpages)
    {
        public int Currentpage { get; set; }=currentpage;
        public int Intemperate { get;set; }=itemperpage;
        public int Totalitem { get; set; } = totalitem;
        public int Totalpages { get; set; } =totalpages;

    }
}
