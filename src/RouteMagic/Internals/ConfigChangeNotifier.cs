using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace RouteMagic.Internals
{
    public class ConfigFileChangeNotifier
    {
        private ConfigFileChangeNotifier(Action<string> changeCallback)
            : this(HostingEnvironment.VirtualPathProvider, changeCallback)
        {
        }

        private ConfigFileChangeNotifier(VirtualPathProvider vpp,
            Action<string> changeCallback)
        {
            _vpp = vpp;
            _changeCallback = changeCallback;
        }

        readonly VirtualPathProvider _vpp;
        readonly Action<string> _changeCallback;

        // When the file at the given path changes, 
        // we'll call the supplied action.
        public static void Listen(string virtualPath, Action<string> action)
        {
            var notifier = new ConfigFileChangeNotifier(action);
            notifier.ListenForChanges(virtualPath);
        }

        void ListenForChanges(string virtualPath)
        {
            // Get a CacheDependency from the BuildProvider, 
            // so that we know anytime something changes
            var virtualPathDependencies = new List<string> { virtualPath };
            CacheDependency cacheDependency = _vpp.GetCacheDependency(
              virtualPath, virtualPathDependencies, DateTime.UtcNow);
            HttpRuntime.Cache.Insert(virtualPath /*key*/,
              virtualPath /*value*/,
              cacheDependency,
              Cache.NoAbsoluteExpiration,
              Cache.NoSlidingExpiration,
              CacheItemPriority.NotRemovable,
              OnConfigFileChanged);
        }

        void OnConfigFileChanged(string key, object value,
          CacheItemRemovedReason reason)
        {
            // We only care about dependency changes
            if (reason != CacheItemRemovedReason.DependencyChanged)
                return;

            _changeCallback(key);

            // Need to listen for the next change
            ListenForChanges(key);
        }
    }
}
