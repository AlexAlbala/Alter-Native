using System;
using System.Collections.Generic;
using System.Text;

namespace Custom.Resources
{
    public class PooledResourceReference
    {
        public PooledResourceEntry reference;

        public PooledResourceReference(PooledResourceEntry reference)
        {
            this.reference = reference;
            System.Threading.Interlocked.Increment(ref reference.references);
        }
        ~PooledResourceReference()
        {
            System.Threading.Interlocked.Decrement(ref reference.references);
        }
    };

    public class PooledResourceEntry
    {
        public string key;
        public object resource;
        public ResourcePool pool;
        public int references = 0;

        public PooledResourceEntry(string key, object resource, ResourcePool pool)
        {
            this.key = key;
            this.resource = resource;
            this.pool = pool;
        }

        public int CompareTo(object o)
        {
            return key.CompareTo(o);
        }
    }

    public interface IResourceLoader
    {
        IDisposable loadResource(string name);
        PooledResourceReference wrapReference(PooledResourceEntry res);
    }

    public class ResourcePool
    {
        public SortedList<string, PooledResourceEntry> resources = new SortedList<string, PooledResourceEntry>();
        IResourceLoader loader;

        public ResourcePool(IResourceLoader loader)
        {
            this.loader = loader;
        }

        public PooledResourceReference allocate(string key)
        {
            PooledResourceEntry res;
            if (!resources.TryGetValue(key, out res))
            {
                object r = loader.loadResource(key);
                res = new PooledResourceEntry(key, r, this);
                resources.Add(key, res);
            }
            return loader.wrapReference(res);
        }

        public void removeUnused()
        {
            LinkedList<string> keys = new LinkedList<string>(resources.Keys);
            foreach (string s in keys)
            {
                PooledResourceEntry r = resources[s];
                if (r.references == 0)
                {
                    IDisposable d = (IDisposable)r.resource;
                    d.Dispose();
                    resources.Remove(s);
                }
            }
        }

        public override string ToString()
        {
            string s = "pool:";
            foreach (KeyValuePair<string, PooledResourceEntry> e in resources)
                s += " / " + e.Key + " " + e.Value.references;
            return s;
        }
    }
}
