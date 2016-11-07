using System.Collections.Generic;

namespace Bastille
{
    /// <summary>
    /// Yes, pretty controversial using inheritance for code reuse! I personally think this approach is demonised unfairly. In giant projects,
    /// it makes sense though - you need that extra insulation. Otherwise it's been very convenient in my 20 years of development.
    /// </summary>
    public class StringToStringSetMap
    {
        private Dictionary<string, HashSet<string>> Impl = new Dictionary<string, HashSet<string>>();
        /**
        Input: 
        userToken(String), URL(String)

        Return: 
        A boolean value of whether or not the URL was successfully saved. 
        If the URL has been saved for the user previously, this function
        should not save it and return false. 
        */
        public bool saveKeyValue(string key, string value)
        {
            if (!Impl.ContainsKey(key))
            {
                Impl[key] = new HashSet<string>() { value };
                return true;
            }

            return Impl[key].Add(value);
        }

        /**
         * getUrls(userToken)

            Input: 
            userToken(String)

            Return: 
            A collection of all the URLs that user has saved, if any.
        */
        public ICollection<string> getValues(string key)
        {
            var l = new HashSet<string>();     // probably don't need to initialise this.
            Impl.TryGetValue(key, out l);
            return l;   // l will be null if the user token wasn't found
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
        public bool removeValue(string key, string value)
        {
            if (!Impl.ContainsKey(key))
                return false;   // this isn't mentioned in the spec but makes sense to me.

            var l = getValues(key);
            return l.Remove(value);

            // TODO should we delete empty lists? Probably not, as long as saveKeyValue() etc. handle empty lists, which they do.
        }
    }
}
