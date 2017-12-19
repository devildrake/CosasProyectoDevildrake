namespace FragmentDataNamespace {
    [System.Serializable]
    public class FragmentData {
        public bool picked;
        public string levelName;
        public FragmentData(bool p, string l) {
            picked = p;
            levelName = l;
        }
    }
}