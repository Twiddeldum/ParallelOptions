namespace ParallelOptions;

using System.Threading.Tasks;

public class Variant2
{
    private const int MaxListValue = 100;
    private List<int> list1 = new();
    private List<int> list2 = new();
    private List<int> list3 = new();

    public async Task Run()
    {
        Console.WriteLine("Starting...");
        FillListWithValues(list1);
        FillListWithValues(list2);
        FillListWithValues(list3);

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 3
        };

        var parallelRuns = new List<Task>();
        parallelRuns.Add(Task.Run(() => RunList(list1, parallelOptions, 1)));
        parallelRuns.Add(Task.Run(() => RunList(list2, parallelOptions, 2)));
        parallelRuns.Add(Task.Run(() => RunList(list3, parallelOptions, 3)));
        parallelRuns.Add(Task.Run(() =>
        {
            for (var i = 0; i <= MaxListValue; i++)
            {
                Console.WriteLine("---------------------------");
                Thread.Sleep(TimeSpan.FromMilliseconds(10000));
            }
        }));


        await Task.WhenAll(parallelRuns);
    }

    void RunList(IList<int> list, ParallelOptions parallelOptions, int listNo)
    {
        Parallel.ForEach(list, parallelOptions, (index) => DisplaySomething(listNo, index));
    }

    static void FillListWithValues(ICollection<int> list)
    {
        for (var i = 0; i <= MaxListValue; i++)
        {
            list.Add(i);
        }
    }

    void DisplaySomething(int listNo, int index)
    {
        Console.WriteLine($"{DateTime.Now} List no: {listNo}, Thread no: {index}");
        Thread.Sleep(TimeSpan.FromMilliseconds(10000));
    }
}