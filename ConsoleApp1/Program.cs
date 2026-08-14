namespace ConsoleApp1
{
    internal class Program
    {
        static List<int> Findduplicates(List<int> talliste)
        {
            Dictionary<int, bool> setTal = new Dictionary<int, bool>();
            List<int> duplicates = new List<int>();

            foreach (int i in talliste)
            {
                if (setTal.ContainsKey(i) && !duplicates.Contains(i))
                {
                    duplicates.Add(i);
                }
                else
                {
                    setTal[i] = true;
                }
            }
            return duplicates;
        }

        static void Main(string[] args)
        {
            List<int> talliste = new List<int> { 1,1,3,4,5,6,6,7,7,9};
            List<int> duplicates = Findduplicates(talliste);
            Console.WriteLine(string.Join(",",duplicates));
        }
    }
}



