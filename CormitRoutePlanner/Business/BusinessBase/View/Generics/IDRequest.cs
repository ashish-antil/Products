#region

using System;
using System.ServiceModel;
using Imarda.Lib;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
    /// <summary>
    ///     Encapsulates a request to perform some operation, whose function or target will
    ///     differ based on the given ID (eg. get an object with the given ID).
    /// </summary>
    [MessageContract]
    [Serializable]
    public class IDRequest : ParameterMessageBase, IWithOptions
    {
        /// <summary>
        ///     Empty request, e.g. when only Parameters are needed
        /// </summary>
        public IDRequest()
        {
        }

        /// <summary>
        ///     Request with one ID.
        /// </summary>
        /// <param name="id">The id</param>
        public IDRequest(Guid id)
        {
            Initialize(id);
        }

        /// <summary>
        ///     Request with one ID and options.
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="options">The retrieval options</param>
        public IDRequest(Guid id, RetrievalOptions options)
        {
            Initialize(id, null, options);
        }

        /// <summary>
        ///     Request with one ID, SessionID and optionsl options.
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="sessionId">The session id</param>
        /// <param name="options">The options</param>
        public IDRequest(Guid id, Guid sessionId, RetrievalOptions options = RetrievalOptions.None)
        {
            Initialize(id, sessionId, options);
        }

        /// <summary>
        ///     Use this method as opposed to constructors to initialize IDRequest.
        ///     This is required because of the nature of SessionID storage.
        /// </summary>
        private void Initialize(Guid id, Guid? sessionId = null, RetrievalOptions? flags = null)
        {
            ID = id;

            if (sessionId.HasValue)
            {
                SessionID = sessionId.Value;
            }

            if (flags.HasValue)
            {
                Options = flags.Value;
            }
        }

        /// <summary>
        ///     Create ID Request and add a bunch of keys with their values. The keys have to be strings,
        ///     the values any non-null values which can get converted to strings by ToString().
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key1">first key</param>
        /// <param name="nonNullValue1">first value</param>
        /// <param name="args">subsequent key/value pairs, keys must be string, values non null</param>
        /// <example>
        ///     new IDRequest(id, "CompanyID", Guid.Empty, "Start", new DateTime(2010,8,11));
        /// </example>
        public IDRequest(Guid id, string key1, object nonNullValue1, params object[] args)
        {
            Initialize(id);

            if (nonNullValue1 != null)
            {
                AddParameters(key1, nonNullValue1);
                AddParameters(args);
            }
        }

        [MessageBodyMember]
        public Guid ID { get; set; }

        [MessageBodyMember]
        public string Path { get; set; }

        [MessageBodyMember]
        public RetrievalOptions Options { get; set; }

        /// <summary>
        ///     Check if all options included.
        /// </summary>
        /// <param name="options">one or more EntityOptions options</param>
        /// <returns>true if all the passed options are set</returns>
        public bool HasAll(RetrievalOptions options)
        {
            return (Options & options) == options;
        }

        /// <summary>
        ///     Check if one or more options included.
        /// </summary>
        /// <param name="options">one or more EntityOptions options</param>
        /// <returns>true if one or more of the options are set</returns>
        public bool HasSome(RetrievalOptions options)
        {
            return (Options & options) != 0;
        }

        public override string ToString()
        {
            return string.Format("IDRequest({0}, {1} params, opt {2}, CompID={3})"
                                 , ID.ShortString()
                                 , (HasParameters ? Parameters.Count : 0)
                                 , Options
                                 , CompanyID.ShortString());
        }
    }
}