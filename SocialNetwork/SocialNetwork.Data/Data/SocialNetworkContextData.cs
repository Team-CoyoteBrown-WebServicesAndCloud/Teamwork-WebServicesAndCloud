namespace SocialNetwork.Data.Data
{

    public class SocialNetworkContextData : SocialNetworkData
    {
        // This constructor gives to SocialNetworkData context for work.
        public SocialNetworkContextData() : base(new SocialNetworkContext())
        {
        }
    }
}
