using UnityEngine;
using System.Collections;

public class Message {
    public delegate void RegisterDeath(Organism organism);
    public static event RegisterDeath Death;

    public static void SubscribeDeath(RegisterDeath callback) {
        Death += callback;
    }

    public static void BroadcastDeath(Organism organism) {
        Death(organism);
    }
}
