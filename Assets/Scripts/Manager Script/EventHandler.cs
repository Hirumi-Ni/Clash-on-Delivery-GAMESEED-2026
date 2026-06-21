using UnityEngine;
using System;

public static class EventHandler
{
    //contoh template buat event actionnya
    public static event Action<int> OnTombolBerangkatDipencet; 
    //jadi event ini manggil method di delivery system / timer system buat jalanin timernya dari parameter int yang dikirim

    //btw namanya buat contoh aja makanya panjang
    public static void WhenTombolBerangkatDipencet(int durasiPerjalanan)
    {
        OnTombolBerangkatDipencet?.Invoke(durasiPerjalanan);
    }
}
