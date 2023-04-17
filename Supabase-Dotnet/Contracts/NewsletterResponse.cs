using System;
namespace Supabase_Dotnet.Contracts
{
    public class CreateNewsletterResponse
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int ReadTime { get; set; }

        public DateTime CreatedAt { get; set; }

    }

}

