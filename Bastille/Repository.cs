using System;
using System.Collections.Generic;

namespace Bastille
{
    /// <summary>
    /// This class is the entry point to the URL-saving system. It has UserTokensToUrls which backs the saving and fetching of usertoken-url pairs ( PROBLEM #1 ).
    /// It also has DomainsToUserTokens which backs the getUsersByDomain() method ( PROBLEM #2 )
    /// I spent ~ 1 - 1:30 on this including noodling around with Node.js a bit first and then opting for C#. I didnt use any external resources other than SO for looking up time complexity of C# Dictionary methods.
    /// 
    /// The only potentially-slow bit of this design is the List<string>.Contains() call in the StringToStringListMap class which will be O(N). This shouldn't be an issue unless users are storing millions of URLs each, which would be highly unlikely. But you never know...
    /// </summary>
    public class Repository
    {
        private StringToStringSetMap UserTokensToUrls = new StringToStringSetMap();

        private StringToStringSetMap DomainsToUserTokens = new StringToStringSetMap();  // see getUsersByDomain()

        /// <summary>
        /// This would call the supplied extractDomain method
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string extractDomain(string url)
        {
            return "";  // TODO
        }
        
        /**
        Input: 
        userToken(String), URL(String)

        Return: 
        A boolean value of whether or not the URL was successfully saved. 
        If the URL has been saved for the user previously, this function
        should not save it and return false.
        */
        public bool saveUrl(string userToken, string url)
        {
            // Stick the domain in a separate map for O(1) retrieval
            DomainsToUserTokens.saveKeyValue(extractDomain(url), userToken);

            // First off, validate the URL. Ignore the result. This will be GC'd easy enough.
            var uri = new Uri(url);

            return UserTokensToUrls.saveKeyValue(userToken, url);
        }

        /**
         * removeUrl(userToken, URL)

            Input: 
            userToken(String), URL(String)

            Return: 
            A boolean value of whether or not the URL was successfully deleted. 
            If the URL to be deleted had never been saved, the function should 
            return false.
            NOTE: also returns false if the token was never saved before. Also, should refactor the "is token-url pair known?" logic out.
        */
        public bool removeUrl(string userToken, string url)
        {
            DomainsToUserTokens.removeValue(extractDomain(url), userToken);     
            return UserTokensToUrls.removeValue(userToken, url);
        }


        /**
         * getUrls(userToken)

            Input: 
            userToken(String)

            Return: 
            A collection of all the URLs that user has saved, if any.
        */
        public ICollection<string> getUrls(string userToken)
        {
            return UserTokensToUrls.getValues(userToken);
        }


        /**
         * getUsersByDomain(domain)

            Input: 
            domain(String)

            Return: 
            A collection of user tokens who have saved URLs that belong to that domain.

            NOTE: so the trade off here is time v.s. space as usual. These days of enormous amounts of RAM and services like AWS Elasticache,
            I favour using up space since you can easily get more. Well, more easily than you can get "more" speed that is. Hence Im opting 
            for using up more space by storing the domains in addition to the URLs.
        */
        public ICollection<string> getUsersByDomain(string domain)
        {
            return DomainsToUserTokens.getValues(domain);
        }
    }
}
