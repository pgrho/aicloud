namespace Shipwreck.AICloud
{
    public static class Speakers
    {
        public static Speaker[] GetKnownSpeakers()
            => new[] {
                Nozomi,
                Sumire,
                Maki,
                Kaho,
                Akari,
                Nanako,
                Reina,
                Seiji,
                Hiroshi,
                Osamu,
                Taichi,
                Koutarou,
                Anzu,
                Yuuto,
                Chihiro,
                NozomiWithEmotion,
                MakiWithEmotion,
                ReinaWithEmotion,
                TaichiWithEmotion,
                MiyabiWest,
                YamatoWest,
            };

        public static readonly Speaker Nozomi = new Speaker("nozomi", "のぞみ", false);
        public static readonly Speaker Sumire = new Speaker("sumire", "すみれ", false);
        public static readonly Speaker Maki = new Speaker("maki", "まき", false);
        public static readonly Speaker Kaho = new Speaker("kaho", "かほ", false);
        public static readonly Speaker Akari = new Speaker("akari", "あかり", false);
        public static readonly Speaker Nanako = new Speaker("nanako", "ななこ", false);
        public static readonly Speaker Reina = new Speaker("reina", "れいな", false);
        public static readonly Speaker Seiji = new Speaker("seiji", "せいじ", true);
        public static readonly Speaker Hiroshi = new Speaker("hiroshi", "ひろし", true);
        public static readonly Speaker Osamu = new Speaker("osamu", "おさむ", true);
        public static readonly Speaker Taichi = new Speaker("taichi", "たいち", true);
        public static readonly Speaker Koutarou = new Speaker("koutarou", "こうたろう", true);
        public static readonly Speaker Anzu = new Speaker("anzu", "あんず", false);
        public static readonly Speaker Yuuto = new Speaker("yuuto", "ゆうと", true);
        public static readonly Speaker Chihiro = new Speaker("chihiro", "ちひろ", false);
        public static readonly Speaker NozomiWithEmotion = new Speaker("nozomi_emo", "のぞみ (感情対応)", false, supportsJoy: true, supportsSadness: true, supportsAnger: true);
        public static readonly Speaker MakiWithEmotion = new Speaker("maki_emo", "まき (感情対応)", false, supportsJoy: true, supportsSadness: true, supportsAnger: true);
        public static readonly Speaker ReinaWithEmotion = new Speaker("reina_emo", "れいな  (感情対応)", false, supportsJoy: true);
        public static readonly Speaker TaichiWithEmotion = new Speaker("taichi_emo", " たいち (感情対応)", true, supportsJoy: true);
        public static readonly Speaker MiyabiWest = new Speaker("miyabi_west", "みやび", false, isWest: true);
        public static readonly Speaker YamatoWest = new Speaker("yamato_west", "やまと", true, isWest: true);
    }
}