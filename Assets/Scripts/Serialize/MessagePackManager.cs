using UnityEngine;
using MessagePack;
using MessagePack.Resolvers;

public class MessagePackManager : MonoBehaviour
{
    void Awake () {
        MessagePack.Resolvers.CompositeResolver.RegisterAndSetAsDefault(
            // use generated resolver first, and combine many other generated/custom resolvers
            MessagePack.Resolvers.GeneratedResolver.Instance,
            // finally, use builtin/primitive resolver(don't use StandardResolver, it includes dynamic generation)
            MessagePack.Resolvers.BuiltinResolver.Instance,
            AttributeFormatterResolver.Instance,
            MessagePack.Resolvers.PrimitiveObjectResolver.Instance,
            // enable extension packages first
            MessagePack.Unity.UnityResolver.Instance,
            // finaly use standard(default) resolver
            StandardResolver.Instance
        );
    }
}
